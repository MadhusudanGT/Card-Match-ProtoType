using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] InputField rowInputValue = null;
    [SerializeField] InputField colInputValue = null;
    [SerializeField] Button generateBtn;
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] Transform parent;

    private int rows = 2;
    private int cols = 3;

    public int minRows = 1;
    public int maxRows = 10;
    public int minCols = 1;
    public int maxCols = 10;

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
        rows = 2;
        cols = 3;
        GenerateBtnClicked();
    }
    void GenerateBtnClicked()
    {
        if (!string.IsNullOrEmpty(rowInputValue.text) && !string.IsNullOrEmpty(colInputValue.text))
        {
            rows = int.Parse(rowInputValue.text);
            cols = int.Parse(colInputValue.text);
        }
        // If only row is filled, auto treat as col = row
        else if (!string.IsNullOrEmpty(rowInputValue.text))
        {
            rows = int.Parse(rowInputValue.text);
            cols = rows;
        }
        else if (!string.IsNullOrEmpty(colInputValue.text))
        {
            cols = int.Parse(colInputValue.text);
            rows = cols;
        }
        else
        {
            Debug.LogError("Please enter at least one value!");
            return;
        }

        rows = Mathf.Clamp(rows, minRows, maxRows);
        cols = Mathf.Clamp(cols, minCols, maxCols);

        rowInputValue.text = rows.ToString();
        colInputValue.text = cols.ToString();

        int totalCards = rows * cols;

        if (totalCards % 2 != 0)
        {
            Debug.LogWarning($"Odd number of cards ({totalCards}). Adjusting...");
            totalCards += 1;
            if (rows >= cols)
                rows += 1;
            else
                cols += 1;
        }

        if (gridLayoutGroup == null) { return; }
        gridLayoutGroup.constraintCount = rows;

        Debug.Log($"Generating grid: {rows} x {cols} = {totalCards} cards");
        GenerateGrid(totalCards);
    }

    void GenerateGrid(int totalCards)
    {
        for (int i = 0; i < totalCards; i++)
        {
            Grid card = PoolController.Instance?.SpawnCard(Vector3.zero, Quaternion.identity);
            card.transform.SetParent(parent);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;
        }
    }
}
