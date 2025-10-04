using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GridBase : MonoBehaviour, IPointerClickHandler
{
    protected GridData gridData;
    public virtual void Init(GridData data)
    {
        this.gridData = data;
        UpdateVisual();
    }
    public Sprite sprite { get => gridData.sprite; }
    public int gridId { get => gridData.gridID; }
    public Items itemsType { get => gridData.itemType; }

    protected abstract void UpdateVisual();
    protected abstract void Flip();
    public void OnPointerClick(PointerEventData eventData)
    {
        Flip();
    }
}
