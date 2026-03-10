using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject codingWindow;   // CardList
    public GameObject openButton;     // общая кнопка

    public void ShowCodingPanel()
    {
        if (codingWindow != null)
            codingWindow.SetActive(true);

        if (openButton != null)
            openButton.SetActive(false);
    }

    public void HideCodingPanel()
    {
        if (codingWindow != null)
            codingWindow.SetActive(false);

        if (openButton != null)
            openButton.SetActive(true);
    }
}
