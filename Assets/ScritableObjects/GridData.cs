using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName = "ScriptableObjects/GridData", order = 1)]
public class GridData : ScriptableObject
{
    [field: SerializeField] public Sprite sprite { get; private set; }
    [field: SerializeField] public int gridID { get; private set; }
    [field: SerializeField] public Items itemType { get; private set; }
}

public enum Items
{
    BAR_1,
    BAR_2,
    BAR_3,
    SEVEN,
    TEN,
    A_SYMBOL,
    BELL,
    CHERRIES,
    GRAPE,
    J_SYMBOL,
    LEMON,
    K_SYMBOL,
    MELON,
    ORRANGE,
    Q_SYMBOL,
    WILD
}