using UnityEngine;
using System.Collections.Generic;

public static class SaveManager
{
    private const string SaveKey = "GameState";

    public static void SaveGame(GameState state)
    {
        string json = JsonUtility.ToJson(state);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static GameState LoadGame()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
            return null;

        string json = PlayerPrefs.GetString(SaveKey);
        return JsonUtility.FromJson<GameState>(json);
    }

    public static void ClearSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
    }
}

[System.Serializable]
public class GameState
{
    public int numberOfTurns;
    public int matchedGridCount;
    public int row, col;
    public bool isMatched;
    public GameStatus gameStatus;
    public List<SaveGridData> grids;
}



[System.Serializable]
public class SaveGridData
{
    public Items itemType;
    public bool isFliped;
    public bool isMatched;
}