using UnityEngine;

public class PoolController : Singleton<PoolController>
{
    [SerializeField] private Grid gridPrefab;
    [SerializeField] private int initialSize = 20;
    private PoolManager<Grid> cardPool;

    protected override void Awake()
    {
        base.Awake();
        cardPool = new PoolManager<Grid>(gridPrefab, initialSize, this.transform);
    }

    public Grid SpawnCard(Vector3 position, Quaternion rotation)
    {
        Grid card = cardPool.Get();
        card.transform.SetPositionAndRotation(position, rotation);
        return card;
    }

    public void DespawnCard(Grid card)
    {
        cardPool.ReturnToPool(card);
    }
}
