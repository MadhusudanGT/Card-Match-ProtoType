using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public class GridGenerator : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] InputField rowInputValue = null;
    [SerializeField] InputField colInputValue = null;
    [SerializeField] Button generateBtn = null;
    [SerializeField] GridLayoutGroup gridLayoutGroup = null;
    [SerializeField] ScrollRect scrollRect = null;
    [SerializeField] Transform parent;

    [Space(5)]
    [Header("Grid Config")]
    [SerializeField] GridConfig gridConfig = null;

    private List<GridData> gameGridsData = new List<GridData>();

    private void OnEnable()
    {
        generateBtn.onClick.AddListener(GenerateBtnClicked);
    }

    private void OnDisable()
    {
        generateBtn.onClick.RemoveListener(GenerateBtnClicked);
    }

    private void Start()
    {
        gridConfig.rows = 5;
        gridConfig.cols = 5;
        GenerateBtnClicked();
    }

    void GenerateBtnClicked()
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
}
