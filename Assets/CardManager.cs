using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] Transform cardParent;   // Canvas или пустой объект внутри Canvas
    [SerializeField] GameObject cardPrefab;  // твой префаб Card

    readonly List<CardView> cards = new();

    public void AddCard()
    {
        var go = Instantiate(cardPrefab, cardParent);
        var view = go.GetComponent<CardView>();

        view.Init(
            start: () =>
            {
                Debug.Log("START для этой карточки");
                // сюда потом подключишь запуск машины
            },
            stop: () =>
            {
                Debug.Log("STOP для этой карточки");
            },
            delete: () =>
            {
                cards.Remove(view);
                Destroy(go);     // ← крестик удаляет карточку
            });

        cards.Add(view);
    }
}
