using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singletons<CardManager>
{
    public Action OnCardFlipped;
    Card lastFlippedCard;
    [SerializeField] float flipDuration = 1.0f;
    [SerializeField] float showCardsAtStartDuration=3;
    public float FlipDuration => flipDuration;
    public float ShowCardsDuration => showCardsAtStartDuration;
    public void OnCardStartedFlip(Card card)
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
        OnCardFlipped.Invoke();
    }
}
