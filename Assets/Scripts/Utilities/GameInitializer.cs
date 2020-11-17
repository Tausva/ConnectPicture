using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab = default;
    [SerializeField] private GameObject nodeTextPrefab = default;
    [SerializeField] private GameObject ropePrefab = default;
    
    [Space]
    [SerializeField] private Transform canvas = default;

    private void Awake()
    {
        ObjectPool.Initialize();

        ObjectPool.InitializeNewPool(ObjectPoolEnum.Node, 16, nodePrefab);
        ObjectPool.InitializeNewPoolInParent(ObjectPoolEnum.NodeText, 16, nodeTextPrefab, canvas);
        ObjectPool.InitializeNewPool(ObjectPoolEnum.Rope, 16, ropePrefab);
    }
}