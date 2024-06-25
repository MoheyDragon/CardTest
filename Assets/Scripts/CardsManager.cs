using UnityEngine;

public class CardsManager:Singletons<CardsManager>
{
    [SerializeField] float flipDuration = 1.0f;
    public float FlipDuration => flipDuration;
    [SerializeField] int cardsCount;
    [SerializeField] Card cardPrefab;
    [SerializeField] Sprite[] cardFaces;

    bool repeatCard;
    int currentCardIndex;
    private void ShuffleCardsToPickFrom()
    {
        for (int i = cardFaces.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = cardFaces[i];
            cardFaces[i] = cardFaces[randomIndex];
            cardFaces[randomIndex] = temp;
        }
    }
    private void Start()
    {
        ShuffleCardsToPickFrom();
        PopulateGridWithCards();
        ShuffleCreatedCards();
    }
    private void PopulateGridWithCards()
    {
        repeatCard = true;
        for (int i = 0; i < cardsCount; i++)
            CreateCard();
    }
    private void CreateCard()
    {
        Card newCard = Instantiate(cardPrefab, transform);
        newCard.SetFrontFace(cardFaces[currentCardIndex],currentCardIndex);
        if (!repeatCard)
            currentCardIndex++;
        repeatCard = !repeatCard;
    }
    public void ShuffleCreatedCards()
    {
        int[] indices = new int[cardsCount];

        for (int i = 0; i < cardsCount; i++)
        {
            indices[i] = i;
        }

        for (int i = 0; i < cardsCount; i++)
        {
            int randomIndex = Random.Range(0, cardsCount);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        for (int i = 0; i < cardsCount; i++)
        {
            transform.GetChild(indices[i]).SetSiblingIndex(i);
        }
    }
}
