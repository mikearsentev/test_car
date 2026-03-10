using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button stopButton;
    [SerializeField] Button deleteButton;

    System.Action onStart;
    System.Action onStop;
    System.Action onDelete;

    public void Init(System.Action start, System.Action stop, System.Action delete)
    {
        onStart = start;
        onStop = stop;
        onDelete = delete;

        startButton.onClick.AddListener(() => onStart?.Invoke());
        stopButton.onClick.AddListener(() => onStop?.Invoke());
        deleteButton.onClick.AddListener(() => onDelete?.Invoke());
    }
}
