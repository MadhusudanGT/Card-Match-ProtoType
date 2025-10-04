using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GridBase : MonoBehaviour, IPointerClickHandler
{
    protected GridData gridData;
    public Sprite sprite { get => gridData.sprite; }
    public int gridId { get => gridData.gridID; }
    public Items itemsType { get => gridData.itemType; }
    public bool IsFaceUp { get; protected set; } = false;
    public bool isMatched = false;
    public virtual void Init(GridData data)
    {
        isMatched = true;
        this.gridData = data;
        IsFaceUp = true;
        UpdateVisual();
        StartCoroutine(StartTheGame());
    }

    IEnumerator StartTheGame()
    {
        yield return new WaitForSeconds(1f);
        IsFaceUp = false;
        UpdateVisual();
        isMatched = false;
    }
    protected abstract void UpdateVisual();
    protected abstract void Flip();
    public void OnPointerClick(PointerEventData eventData)
    {
        Flip();
    }
}
