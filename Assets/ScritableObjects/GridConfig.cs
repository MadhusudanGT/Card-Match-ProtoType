using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GridConfig", menuName = "ScriptableObjects/GridConfig", order = 1)]
public class GridConfig : ScriptableObject
{
    [Header("Grid Settings")]
    public int rows = 2;
    public int cols = 3;

    public int minRows = 1;
    public int maxRows = 10;
    public int minCols = 1;
    public int maxCols = 10;
    public float spacingX = 150f;
    public float spacingY = 200f;

    public List<GridData> listOfItems = new List<GridData>();

    public int numberOfTurns = 10;
}
