using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab; // Сюда перетащишь префаб
    public Transform cardParent;  // Сюда перетащишь объект CardList

    public void AddNewCard()
    {
        Instantiate(cardPrefab, cardParent);
    }
}