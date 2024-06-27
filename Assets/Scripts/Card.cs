using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    Button button;

    Image cardImage;
    Sprite frontFace;
    Sprite backFace;
    Color backFaceColor;

    private int frontFaceIndex;
    float flipDuration;

    bool isBackSideUp;
    bool isFlipping;
    bool isMarkedForWin;
    bool isEnteryFlip;

    Card comparingCard;
    public bool IsMatchingCard (Card pairingCard)=> frontFaceIndex==pairingCard.frontFaceIndex;
    // Pairing card is the card clicked with this card no matter if it's the correct one or not
    public void SetComparingCard(Card comparingCard)=> this.comparingCard = comparingCard; 
    private void Awake()
    {
        AssignReferences();
    }
    private void Start()
    {
        ResetValues();
        ShowFrontFaceThenHide();
        SubsribeToMatchManager();
    }
    private void SubsribeToMatchManager()
    {
        LevelManager.Singleton.OnLose += OnMatchEnd;
        LevelManager.Singleton.OnWin += OnMatchEnd;
    }
    public void SetFrontFace(Sprite frontFace, int cardIndex)
    {
        this.frontFace = frontFace;
        this.frontFaceIndex = cardIndex;
    }
    public  void ShowFrontFaceThenHide()
    {
        button.enabled = false;
        AlterVisibleFace();
        Invoke(nameof(FlipCard), CardManager.Singleton.ShowCardsDuration);
        Invoke(nameof(EnableCard), CardManager.Singleton.ShowCardsDuration);
    }
    private void EnableCard()
    {
        button.enabled = true;
    }
    public void FlipCard()
    {
        StartCoroutine(CO_FlipCard());
    }
    private void ClickOnCard()
    {
        if (isFlipping) return;
        if (!isBackSideUp) return;
        CardManager.Singleton.OnCardStartedFlip(this);
        FlipCard();
    }
    float startRotation = 0;
    float midRotation = 90;
    IEnumerator CO_FlipCard()
    {
        isFlipping = true;
        float t = 0.0f;
        while (t < flipDuration / 2)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, midRotation, t / (flipDuration / 2)) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

            yield return null;
        }

        AlterVisibleFace();

        t = 0.0f;

        while (t < flipDuration / 2)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(midRotation, startRotation, t / (flipDuration / 2)) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

            yield return null;
        }
        isFlipping = false;

        if (isEnteryFlip)
        {
            isEnteryFlip = false;
            yield break;
        }
        if (!comparingCard) yield break;
            SecondCardFlipped();

    }

    private void AlterVisibleFace()
    {
        if (isBackSideUp)
        {
            cardImage.sprite = frontFace;
            cardImage.color = Color.white;
        }
        else
        {
            cardImage.sprite = backFace;
            cardImage.color = backFaceColor;
        }
        isBackSideUp = !isBackSideUp;
    }
    private void AssignReferences()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickOnCard);
        cardImage = GetComponent<Image>();
        backFace = GetComponent<Image>().sprite;
    }
    private void ResetValues()
    {
        isFlipping = false;
        isBackSideUp = true;
        isEnteryFlip = true;
        flipDuration = CardManager.Singleton.FlipDuration;
        backFaceColor = cardImage.color;
    }
    public void MarkForWin()
    {
        isMarkedForWin = true;
    }
    private void SecondCardFlipped()
    {
        if (isMarkedForWin)
            CorrectMatch();
        else
            MissMatch();   
    }
    private void CorrectMatch()
    {
        LevelManager.Singleton.CorrectMatch();
        Destroy(comparingCard.gameObject);
        Destroy(gameObject);
    }
    private void MissMatch()
    {
        LevelManager.Singleton.WrongMatch();
        FlipCard();
        comparingCard.FlipCard();
        comparingCard = null;
    }
    private void OnMatchEnd()
    {
        enabled = false;
        button.enabled = false;
    }
    private void OnDisable()
    {
        UnSubsribeToMatchManager();
    }
    private void UnSubsribeToMatchManager()
    {
        LevelManager.Singleton.OnLose -= OnMatchEnd;
        LevelManager.Singleton.OnWin -= OnMatchEnd;
    }
}
