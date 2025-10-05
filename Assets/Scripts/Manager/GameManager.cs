using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private GridGenerator gridSpawner;
    [SerializeField] private GridMatchManager gridMatchManager;
    [SerializeField] GridConfig gridConfig = null;
    [SerializeField] int numberOfTurnsLeft, matchedGridCount = 0;
    [SerializeField] GameStatus gameState;
    private void Awake()
    {
        gameState = global::GameStatus.Completed;
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

    private void Start()
    {
        LoadGame();
    }
    void GameStatus(bool gameStatus)
    {
        uiManager.GameEndStatus(gameStatus);
        gameState = global::GameStatus.Completed;
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
        matchedGridCount = matchedPair;
        uiManager.MatchedItems(matchedPair);
    }

    public void Replay()
    {
        gameState = global::GameStatus.InProgress;
        gridSpawner.InitBoard();
        numberOfTurnsLeft = gridConfig.numberOfTurns;
        matchedGridCount = 0;
        gridMatchManager.matchedPairs = 0;
        uiManager.MatchedItems(matchedGridCount);
        uiManager.UpdateTurnLeftTxt(numberOfTurnsLeft);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    public void SaveGame()
    {
        GameState state = new GameState
        {
            numberOfTurns = numberOfTurnsLeft,
            matchedGridCount = matchedGridCount,
            gameStatus = gameState,
            row = gridConfig.rows,
            col = gridConfig.cols,
            grids = gridSpawner.GetGridData()
        };

        SaveManager.SaveGame(state);
    }

    private List<SaveGridData> savedData = new List<SaveGridData>();
    public void LoadGame()
    {
        GameState state = SaveManager.LoadGame();
        if (state != null && state.gameStatus == global::GameStatus.InProgress)
        {
            numberOfTurnsLeft = state.numberOfTurns;
            matchedGridCount = state.matchedGridCount;
            gridMatchManager.matchedPairs = matchedGridCount;
            gameState = state.gameStatus;
            savedData = state.grids;
            gridSpawner.SetTheGridLayout(state.row);
            for (int i = 0; i < savedData.Count; i++)
            {
                gridSpawner.GenerateGridByData(savedData[i], i);
            }
            gridConfig.rows = state.row;
            gridConfig.cols = state.col;
            savedData.Clear();
            Invoke(nameof(ArrangeFlipedGrids), 1.5f);
            Debug.Log("Game resumed!..." + gridConfig.rows + "..." + gridConfig.cols);
        }
    }

    public void StartNewGame()
    {
        if (gameState != global::GameStatus.InProgress)
        {
            Replay();
        }
    }

    void ArrangeFlipedGrids()
    {
        uiManager.MatchedItems(matchedGridCount);
        uiManager.UpdateTurnLeftTxt(numberOfTurnsLeft);
        gridSpawner.FlipTheGridFromSaveData();
    }
    public void UpdateTurnsText(int turnsCount)
    {
        if (numberOfTurnsLeft == gridConfig.numberOfTurns)
        {
            gridConfig.numberOfTurns = turnsCount;
            numberOfTurnsLeft = gridConfig.numberOfTurns;
            uiManager.UpdateTurnLeftTxt(numberOfTurnsLeft);
        }
    }
}
public enum GameStatus
{
    InProgress,
    Completed
}