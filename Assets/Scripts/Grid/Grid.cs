using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    void ReleaseGrid()
    {
        transform.SetParent(null);
        PoolController.Instance?.DespawnCard(this);
    }
}
