using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private GridGenerator gridSpawner;
    [SerializeField] GridConfig gridConfig = null;
    [SerializeField] int numberOfTurnsLeft = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        EventBus.Subscribe<bool>(GameEvents.GAME_END, GameStatus);
        EventBus.Subscribe<string>(GameEvents.TURNS, FlipAction);
        EventBus.Subscribe<int>(GameEvents.MATCHED_PAIRS, MatchedPair);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<bool>(GameEvents.GAME_END, GameStatus);
        EventBus.Unsubscribe<string>(GameEvents.TURNS, FlipAction);
        EventBus.Unsubscribe<int>(GameEvents.MATCHED_PAIRS, MatchedPair);
    }

    void GameStatus(bool gameStatus)
    {
        uiManager.GameEndStatus(gameStatus);
    }

    void FlipAction(string msg)
    {
        numberOfTurnsLeft--;
        uiManager.UpdateTurnLeftTxt(numberOfTurnsLeft);
        if (numberOfTurnsLeft <= 0)
        {
            EventBus.Invoke<bool>(GameEvents.GAME_END, false);
        }
    }

    void MatchedPair(int matchedPair)
    {
        uiManager.MatchedItems(matchedPair);
    }

    public void Replay()
    {
        gridSpawner.InitBoard();
        uiManager.MatchedItems(0);
        numberOfTurnsLeft = gridConfig.numberOfTurns;
        uiManager.UpdateTurnLeftTxt(numberOfTurnsLeft);
    }
}
