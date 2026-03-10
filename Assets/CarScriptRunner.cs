using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PyLikeCarRunner : MonoBehaviour
{
    public Transform car;
    public TMP_InputField codeField;
    public float cellSize = 1f;
    public float moveSpeed = 2f;
    public float turnSpeedDeg = 90f;

    // Настройки препятствий
    public LayerMask obstacleMask;           // какие слои считаем препятствиями
    public float obstacleCheckRadius = 0.6f; // «ширина» машины для SphereCast
    public float obstacleCheckHeight = 0.5f; // высота точки начала SphereCast
    public float obstacleSkin = 0.1f;        // небольшой зазор

    enum CmdType { Forward, Back, Left, Right, Loop }

    abstract class Node { }

    class CommandNode : Node
    {
        public CmdType Type;
        public float Arg; // шаг или угол
    }

    class LoopNode : Node
    {
        public int Count;
        public List<Node> Body = new List<Node>();
    }

    public void OnPlayClicked()
    {
        string code = codeField.text;

        List<Node> program;
        if (!TryParseProgram(code, out program))
        {
            Debug.LogError("Ошибка в скрипте, см. лог выше");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(RunProgram(program));
    }

    bool TryParseProgram(string code, out List<Node> program)
    {
        program = new List<Node>();
        var lines = code.Split('\n');

        // стек для вложенных циклов
        var stack = new Stack<(int indent, LoopNode loop)>();
        stack.Push((0, null)); // корень

        int lineNum = 0;
        foreach (var rawLine in lines)
        {
            lineNum++;
            string line = rawLine.Replace("\t", "    "); // табы -> 4 пробела
            if (string.IsNullOrWhiteSpace(line)) continue;

            // комментарии
            int hash = line.IndexOf('#');
            if (hash >= 0) line = line.Substring(0, hash);
            if (string.IsNullOrWhiteSpace(line)) continue;

            int indent = 0;
            while (indent < line.Length && line[indent] == ' ') indent++;
            string trimmed = line.Trim();

            // выравниваем стек по отступам
            while (stack.Count > 1 && indent <= stack.Peek().indent)
                stack.Pop();

            if (trimmed.StartsWith("for ") && trimmed.EndsWith(":"))
            {
                // ожидаем формат: for i in range(N):
                int start = trimmed.IndexOf("range(");
                int end = trimmed.LastIndexOf(')');
                if (start < 0 || end < 0 || end <= start + 6)
                {
                    Debug.LogError($"[строка {lineNum}] Ожидалось for i in range(N):");
                    return false;
                }

                string numStr = trimmed.Substring(start + 6, end - (start + 6));
                if (!int.TryParse(numStr, out int count) || count <= 0)
                {
                    Debug.LogError($"[строка {lineNum}] Некорректный N в range(N)");
                    return false;
                }

                var loop = new LoopNode { Count = count };

                if (stack.Peek().loop == null)
                    program.Add(loop);
                else
                    stack.Peek().loop.Body.Add(loop);

                stack.Push((indent, loop));
            }
            else
            {
                CommandNode cmd = ParseCommand(trimmed, lineNum);
                if (cmd == null) return false;

                if (stack.Peek().loop == null)
                    program.Add(cmd);
                else
                    stack.Peek().loop.Body.Add(cmd);
            }
        }

        return true;
    }

    CommandNode ParseCommand(string text, int lineNum)
    {
        // forward(3)
        if (text.StartsWith("forward(") && text.EndsWith(")"))
        {
            string arg = text.Substring(8, text.Length - 9);
            if (!float.TryParse(arg, out float n))
            {
                Debug.LogError($"[строка {lineNum}] forward(N): N должно быть числом");
                return null;
            }
            return new CommandNode { Type = CmdType.Forward, Arg = n };
        }
        if (text.StartsWith("back(") && text.EndsWith(")"))
        {
            string arg = text.Substring(5, text.Length - 6);
            if (!float.TryParse(arg, out float n))
            {
                Debug.LogError($"[строка {lineNum}] back(N): N должно быть числом");
                return null;
            }
            return new CommandNode { Type = CmdType.Back, Arg = n };
        }
        if (text.StartsWith("left(") && text.EndsWith(")"))
        {
            string arg = text.Substring(5, text.Length - 6);
            if (!float.TryParse(arg, out float a))
            {
                Debug.LogError($"[строка {lineNum}] left(A): A должно быть числом");
                return null;
            }
            return new CommandNode { Type = CmdType.Left, Arg = a };
        }
        if (text.StartsWith("right(") && text.EndsWith(")"))
        {
            string arg = text.Substring(6, text.Length - 7);
            if (!float.TryParse(arg, out float a))
            {
                Debug.LogError($"[строка {lineNum}] right(A): A должно быть числом");
                return null;
            }
            return new CommandNode { Type = CmdType.Right, Arg = a };
        }

        Debug.LogError($"[строка {lineNum}] Неизвестная команда: {text}");
        return null;
    }

    IEnumerator RunProgram(List<Node> nodes)
    {
        foreach (var node in nodes)
            yield return ExecuteNode(node);
    }

    IEnumerator ExecuteNode(Node node)
    {
        if (node is CommandNode c)
        {
            switch (c.Type)
            {
                case CmdType.Forward:
                    yield return MoveForwardCells(c.Arg);
                    break;
                case CmdType.Back:
                    yield return MoveForwardCells(-c.Arg);
                    break;
                case CmdType.Left:
                    yield return Turn(-c.Arg);
                    break;
                case CmdType.Right:
                    yield return Turn(c.Arg);
                    break;
            }
        }
        else if (node is LoopNode loop)
        {
            for (int i = 0; i < loop.Count; i++)
            {
                foreach (var inner in loop.Body)
                    yield return ExecuteNode(inner);
            }
        }
    }

    IEnumerator MoveForwardCells(float cells)
    {
        float distance = cells * cellSize;
        float dir = Mathf.Sign(distance);
        float remaining = Mathf.Abs(distance);

        while (remaining > 0f)
        {
            float step = Mathf.Min(remaining, moveSpeed * Time.deltaTime);

            Vector3 stepVector = car.forward * dir * step;
            Vector3 origin = car.position + Vector3.up * obstacleCheckHeight;
            Vector3 direction = stepVector.normalized;
            float checkDistance = stepVector.magnitude + obstacleSkin;

            bool blocked = Physics.SphereCast(
                origin,
                obstacleCheckRadius,
                direction,
                out RaycastHit hit,
                checkDistance,
                obstacleMask
            );

            Debug.DrawRay(origin, direction * checkDistance, blocked ? Color.red : Color.green);

            if (blocked)
            {
                Debug.Log("Впереди препятствие: " + hit.collider.name);
                yield break; // просто останавливаемся
            }

            car.position += stepVector;
            remaining -= step;
            yield return null;
        }
    }

    IEnumerator Turn(float angle)
    {
        float startY = car.eulerAngles.y;
        float targetY = startY + angle;
        float duration = Mathf.Abs(angle) / turnSpeedDeg;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float y = Mathf.Lerp(startY, targetY, t / duration);
            car.rotation = Quaternion.Euler(0f, y, 0f);
            yield return null;
        }
    }
}
