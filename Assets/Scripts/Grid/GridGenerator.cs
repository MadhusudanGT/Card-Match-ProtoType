using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] InputField rowInputValue = null;
    [SerializeField] InputField colInputValue = null;
    [SerializeField] InputField totalTurns = null;
    [SerializeField] GridLayoutGroup gridLayoutGroup = null;
    [SerializeField] ScrollRect scrollRect = null;
    [SerializeField] Transform parent;

    [Space(5)]
    [Header("Grid Config")]
    [SerializeField] GridConfig gridConfig = null;

    private List<GridData> gameGridsData = new List<GridData>();
    [field: SerializeField] List<Grid> isFlipedGrid;
    [field: SerializeField] List<Grid> isMatchedGrid;

    private void Start()
    {
        totalTurns.text = gridConfig.numberOfTurns.ToString();
    }
    public void InitBoard()
    {
        Debug.Log("init board");
        GenerateBtnClicked();
    }
    private void GenerateBtnClicked()
    {
        if (!string.IsNullOrEmpty(rowInputValue.text) && !string.IsNullOrEmpty(colInputValue.text))
        {
            gridConfig.rows = int.Parse(rowInputValue.text);
            gridConfig.cols = int.Parse(colInputValue.text);
        }
        else if (!string.IsNullOrEmpty(rowInputValue.text))
        {
            gridConfig.rows = int.Parse(rowInputValue.text);
            gridConfig.cols = gridConfig.rows;
        }
        else if (!string.IsNullOrEmpty(colInputValue.text))
        {
            gridConfig.cols = int.Parse(colInputValue.text);
            gridConfig.rows = gridConfig.cols;
        }
        else
        {
            Debug.LogError("Please enter at least one value!");
            return;
        }

        gridConfig.rows = Mathf.Clamp(gridConfig.rows, gridConfig.minRows, gridConfig.maxRows);
        gridConfig.cols = Mathf.Clamp(gridConfig.cols, gridConfig.minCols, gridConfig.maxCols);


        int totalCards = gridConfig.rows * gridConfig.cols;

        if (totalCards % 2 != 0)
        {
            Debug.LogWarning($"Odd number of cards ({totalCards}). Adjusting...");
            totalCards += 1;
            if (gridConfig.rows >= gridConfig.cols)
                gridConfig.rows += 1;
            else
                gridConfig.cols += 1;
        }

        rowInputValue.text = gridConfig.rows.ToString();
        colInputValue.text = gridConfig.cols.ToString();
        totalCards = gridConfig.cols * gridConfig.rows;

        ToggleScrollRect();

        if (gridLayoutGroup == null) { return; }
        gridLayoutGroup.constraintCount = gridConfig.rows;

        Debug.Log($"Generating grid: {gridConfig.rows} x {gridConfig.cols} = {totalCards} cards");
        StartCoroutine(StartTheProcess(totalCards));
    }

    IEnumerator StartTheProcess(int totalGrids)
    {
        GenerateGridDatas(totalGrids);
        ReleaseAllGrids();
        yield return new WaitForEndOfFrame();
        GenerateGrid(totalGrids);
    }
    void GenerateGrid(int totalCards)
    {
        for (int i = 0; i < totalCards; i++)
        {
            Grid card = PoolController.Instance?.SpawnCard(Vector3.zero, Quaternion.identity);
            if (card == null) continue;

            card.name = $"Card {i:D2}";
            card.Init(gameGridsData[i]);

            card.transform.SetParent(parent, false);
            card.transform.localScale = Vector3.one;
        }
    }

    public void SetTheGridLayout(int row)
    {
        gridLayoutGroup.constraintCount = row;
    }

    public void GenerateGridByData(SaveGridData gridData, int index)
    {
        Grid card = PoolController.Instance?.SpawnCard(Vector3.zero, Quaternion.identity);
        if (card == null) return;
        GridData data = gridConfig.listOfItems.FirstOrDefault(item => item.itemType == gridData.itemType);

        card.Init(data);

        card.transform.SetParent(parent, false);
        card.transform.localScale = Vector3.one;


        if (!gridData.isMatched && gridData.isFliped)
        {
            isFlipedGrid.Add(card);
        }

        if (gridData.isFliped && gridData.isMatched)
        {
            isMatchedGrid.Add(card);
        }
    }

    public void FlipTheGridFromSaveData()
    {
        for (int i = 0; i < isFlipedGrid.Count; i++)
        {
            isFlipedGrid[i].FlipedGridLoadData();
        }
        for (int i = 0; i < isMatchedGrid.Count; i++)
        {
            isMatchedGrid[i].IsMatchedGrid();
        }
        isFlipedGrid.Clear();
        isMatchedGrid.Clear();
    }
    void ToggleScrollRect()
    {
        if (scrollRect == null) { return; }
        scrollRect.vertical = gridConfig.cols >= 5;
    }

    public void ReleaseAllGrids()
    {
        if (parent == null) return;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            Grid card = child.GetComponent<Grid>();
            if (card != null)
            {
                card.IsFaceUp = false;
                card.isMatched = false;
                child.SetParent(null);
                PoolController.Instance.DespawnCard(card);
            }
        }
    }

    public void GenerateGridDatas(int totalGrid)
    {
        gameGridsData.Clear();

        int requiredPairs = totalGrid / 2;

        // Check if we have enough unique items
        if (gridConfig.listOfItems.Count < requiredPairs)
        {
            Debug.LogError("Not enough unique items to fill the grid!");
            requiredPairs = 15;
        }

        List<GridData> selectedItems = gridConfig.listOfItems.Take(requiredPairs).ToList();

        foreach (GridData item in selectedItems)
        {
            gameGridsData.Add(item);
            gameGridsData.Add(item);
        }

        // Shuffle the final list
        gameGridsData = gameGridsData.OrderBy(x => Random.value).ToList();
    }

    public List<SaveGridData> GetGridData()
    {
        List<SaveGridData> data = new List<SaveGridData>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            Grid card = child.GetComponent<Grid>();
            if (card == null) continue;

            SaveGridData saveGridData = new SaveGridData();
            saveGridData.itemType = card.itemsType;
            saveGridData.isFliped = card.IsFaceUp;
            saveGridData.isMatched = card.isMatched;
            data.Add(saveGridData);
        }
        return data;
    }

    public void UpdateTurnsValueChange()
    {
        int turnsCount = int.Parse(totalTurns.text);
        if (turnsCount > 10 && turnsCount < 50)
        {
            totalTurns.textComponent.color = Color.green;
            GameManager.Instance?.UpdateTurnsText(turnsCount);

        }
        else
        {
            totalTurns.textComponent.color = Color.red;
        }
    }
}
