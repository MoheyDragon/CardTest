using UnityEngine;
using UnityEngine.UI;

public class GridManager : Singletons<GridManager>
{
    [SerializeField] int cardsCount;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Sprite[] cardFaces;
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    private void Start()
    {
        ShuffleCardsToPickFrom();
        SetupGridLayout();
        PopulateGridWithCards();
        ShuffleCreatedCards();
    }
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
    private int[] FindClosestBalancedGrid(int cardsCount)
    {
        int[] rowColumnsCombination = new int[2];
        int closestDifference = int.MaxValue;

        for (int i = 1; i <= Mathf.Sqrt(cardsCount); i++)
        {
            if (cardsCount % i == 0)
            {
                int quotient = cardsCount / i;
                int difference = Mathf.Abs(quotient - i);

                if (difference < closestDifference)
                {
                    closestDifference = difference;
                    rowColumnsCombination[0] = i;
                    rowColumnsCombination[1] = quotient;
                }
            }
        }
        return rowColumnsCombination;
    }

    [Space]
    [Header("Grid Spacing")]
    [SerializeField] float minimumSpacing = 10f; 
    [SerializeField] float spacingPercatageToCell=0.5f;
    private void SetupGridLayout()
    {
        int[] balancedGrid = FindClosestBalancedGrid(cardsCount);
        int columns = balancedGrid[1];
        int rows = balancedGrid[0];

        RectTransform parentRect = gridLayoutGroup.GetComponent<RectTransform>();

        float cellWidth = parentRect.rect.width / (columns+spacingPercatageToCell);
        float cellHeight = parentRect.rect.height / (rows+ spacingPercatageToCell);

        float maxSpacingX = (parentRect.rect.width - columns * cellWidth) / (columns - 1);
        float maxSpacingY = (parentRect.rect.height - rows * cellHeight) / (rows - 1);
        float maxSpacing = Mathf.Min(maxSpacingX, maxSpacingY);

        float spacing = Mathf.Max(minimumSpacing, maxSpacing);

        float cellSizeX = (parentRect.rect.width - (columns - 1) * spacing) / columns;
        float cellSizeY = (parentRect.rect.height - (rows - 1) * spacing) / rows;

        float cellSize = Mathf.Min(cellSizeX, cellSizeY);

        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        gridLayoutGroup.spacing = new Vector2(spacing, spacing);
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;
    }

    bool repeatCard;
    int frontFaceIndex;

    Transform gridParent;
    private void PopulateGridWithCards()
    {
        repeatCard = true;
        gridParent = gridLayoutGroup.transform;
        for (int i = 0; i < cardsCount; i++)
            CreateCard(i);
    }
    private void CreateCard(int i)
    {
        Card newCard = Instantiate(cardPrefab, gridParent).transform.GetChild(0).GetComponent<Card>();
        newCard.transform.parent.name = "card " + i;
        newCard.SetFrontFace(cardFaces[frontFaceIndex], frontFaceIndex);
        if (!repeatCard)
            frontFaceIndex++;
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
            gridParent.GetChild(indices[i]).SetSiblingIndex(i);
        }
    }
}
