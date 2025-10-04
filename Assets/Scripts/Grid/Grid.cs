using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : GridBase
{
    [SerializeField] Image gridImage;
    private void OnEnable()
    {
        EventBus.Subscribe<string>(GameEvents.RELEASE_ALL_GRIDS, ReleaseGrid);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<string>(GameEvents.RELEASE_ALL_GRIDS, ReleaseGrid);
    }

    protected override void UpdateVisual()
    {
        gridImage.sprite = sprite;
    }

    protected override void Flip()
    {
        ReleaseGrid("");
    }
    void ReleaseGrid(string reason)
    {
        transform.SetParent(null);
        PoolController.Instance?.DespawnCard(this);
    }
}
