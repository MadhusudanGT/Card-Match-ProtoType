using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMatchManager : MonoBehaviour
{
    [SerializeField] GridConfig gridConfig;
    private List<Grid> flippedCards = new List<Grid>();
    public int matchedPairs;

    private bool isCheckingMatch = false;
    private void OnEnable()
    {
        EventBus.Subscribe<Grid>(GameEvents.FLIP_ACTION, OnGridFlipped);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<Grid>(GameEvents.FLIP_ACTION, OnGridFlipped);
    }

    void OnGridFlipped(Grid grid)
    {
        if (isCheckingMatch) return;
        if (flippedCards.Contains(grid)) return;

        flippedCards.Add(grid);

        if (flippedCards.Count == 2)
        {
            Grid grid1 = flippedCards[0];
            Grid grid2 = flippedCards[1];
            flippedCards.Remove(grid1);
            flippedCards.Remove(grid2);
            StartCoroutine(CheckMatch(grid1, grid2));
        }
    }

    IEnumerator CheckMatch(Grid first, Grid second)
    {
        isCheckingMatch = true;
        yield return new WaitForSeconds(0.3f);
        if (first.itemsType == second.itemsType)
        {
            first.SetMatched();
            second.SetMatched();
            matchedPairs++;
            EventBus.Invoke<int>(GameEvents.MATCHED_PAIRS, matchedPairs);
            int total = gridConfig.rows * gridConfig.cols / 2;
            Debug.Log(total + "..." + matchedPairs);
            if (matchedPairs == total)
            {
                Debug.Log("Winner!!");
                EventBus.Invoke<bool>(GameEvents.GAME_END, true);
            }
        }
        else
        {
            first.WrongMatch();
            second.WrongMatch();
        }

        flippedCards.Clear();
        isCheckingMatch = false;
    }
}
