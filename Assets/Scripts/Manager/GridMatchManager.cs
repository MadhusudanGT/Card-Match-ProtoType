using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMatchManager : MonoBehaviour
{
    [SerializeField] GridConfig gridConfig;
    private Queue<Grid> flippedCards = new Queue<Grid>();
    public int matchedPairs;

    public bool isCheckingMatch = false;
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
        if (isCheckingMatch)
        {
            return;
        }

        if (flippedCards.Contains(grid)) return;
        flippedCards.Enqueue(grid);

        //Debug.Log(isCheckingMatch + "...isCheckingMatch.." + flippedCards.Count);
        if (flippedCards.Count == 2)
        {
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        Grid first = flippedCards.Dequeue();
        Grid second = flippedCards.Dequeue();
        isCheckingMatch = true;
        if (first.itemsType == second.itemsType)
        {
            SoundManager.Instance?.PlayMatch();
            first.SetMatched();
            second.SetMatched();
            matchedPairs++;
            EventBus.Invoke<int>(GameEvents.MATCHED_PAIRS, matchedPairs);
            int total = gridConfig.rows * gridConfig.cols / 2;
            if (matchedPairs == total)
            {
                EventBus.Invoke<bool>(GameEvents.GAME_END, true);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            SoundManager.Instance?.PlayMismatch();
            first.WrongMatch();
            second.WrongMatch();
        }
        //Debug.Log(first.itemsType + "..." + second.itemsType);
        isCheckingMatch = false;
    }
}
