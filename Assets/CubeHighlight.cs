using UnityEngine;

public class CubeHighlight : MonoBehaviour
{
    public Color highlightColor = Color.yellow;

    Color originalColor;
    Material mat;

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null) return;                     // нет Renderer – просто выходим

        mat = renderer.material;                          // инстанс материала
        if (mat.HasProperty("_Color"))
            originalColor = mat.color;
    }

    void OnMouseEnter()
    {
        if (mat != null && mat.HasProperty("_Color"))
            mat.color = highlightColor;
    }

    void OnMouseExit()
    {
        if (mat != null && mat.HasProperty("_Color"))
            mat.color = originalColor;
    }
}
