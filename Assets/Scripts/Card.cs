using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    Button button;

    Image cardImage;
    Sprite frontFace;
    Sprite backFace;
    Color backFaceColor;
    private int cardIndex;

    float flipDuration;
    bool isBackSideUp;
    bool isFlipping;

    private void Start()
    {
        AssignReferences();
        ResetValues();
    }
    private void AssignReferences()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
        cardImage = GetComponent<Image>();
        backFace = GetComponent<Image>().sprite;
    }
    private void ResetValues()
    {
        isFlipping = false;
        isBackSideUp = true;
        flipDuration = CardsManager.Singleton.FlipDuration;
        backFaceColor = cardImage.color;
    }
    public void SetFrontFace(Sprite frontFace, int cardIndex)
    {
        this.frontFace = frontFace;
        this.cardIndex = cardIndex;
    }
    private void OnButtonClicked()
    {
        if (isFlipping) return;
        StartCoroutine(CO_FlipCard());
    }

    bool faceChanged;
    IEnumerator CO_FlipCard()
    {
        isFlipping = true;
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 180.0f;
        float t = 0.0f;
        faceChanged = false;

        while (t < flipDuration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / flipDuration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            if (!faceChanged)
            {
                if (yRotation > 90.0f && yRotation < 270.0f)
                {
                    ChangeVisibleFace();
                    faceChanged = true;
                }
            }

            yield return null;
        }

        isFlipping = false;
    }
    private void ChangeVisibleFace()
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

}
