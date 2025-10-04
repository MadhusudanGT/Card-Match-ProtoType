using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
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
        gridImage.enabled = IsFaceUp;
        gridImage.sprite = sprite;
    }

    protected override void Flip()
    {
        if (isMatched) { return; }

        IsFaceUp = !IsFaceUp;
        EventBus.Invoke<Grid>(GameEvents.FLIP_ACTION, this);

        transform.DORotate(new Vector3(0, 90, 0), 0.1f)
       .OnComplete(() =>
       {
           transform.DORotate(new Vector3(0, 0, 0), 0.1f)
               .OnComplete(() =>
               {
                   gridImage.enabled = IsFaceUp;
                   gridImage.sprite = sprite;
               });
       });
    }

    public void WrongMatch()
    {
        gridImage.enabled = false;
        Flip();
    }
    void ReleaseGrid(string reason)
    {
        transform.SetParent(null);
        PoolController.Instance?.DespawnCard(this);
    }

    public void SetMatched()
    {
        isMatched = true;
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            ReleaseGrid("");
        });
    }
}
