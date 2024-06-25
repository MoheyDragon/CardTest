using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singletons<CardManager>
{
    Card lastFlippedCard;
    [SerializeField] float flipDuration = 1.0f;
    [SerializeField] float showCardsAtStartDuration=3;
    public float FlipDuration => flipDuration;
    public float ShowCardsDuration => showCardsAtStartDuration;
    protected override void Awake()
    {
        base.Awake();
        SubscribeToCardActions();
    }
    protected void SubscribeToCardActions()
    {
        Card.OnCardStartedFlip += OnCardStartedFlip;
    }
    private void OnCardStartedFlip(Card card)
    {
        if (lastFlippedCard == null)
            lastFlippedCard = card;
        else
        {
            if(lastFlippedCard.IsMatchingCard(card))
            {
                card.MarkForWin();
            }
            card.SetComparingCard(lastFlippedCard);
            lastFlippedCard = null;
        }
    }
}
