using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner2D : MonoBehaviour
{
    public static ItemSpawner2D Instance;

    [Header("Префабы предметов")]
    public GameObject bananaPrefab;
    public GameObject foodPrefab;
    public GameObject slippersPrefab;
    public GameObject mousePrefab;

    [Header("Точки спавна")]
    public Transform[] bananaPoints;
    public Transform[] foodPoints;
    public Transform[] slippersPoints;
    public Transform[] mousePoints;

    private List<GameObject> _activeItems = new List<GameObject>();

    void Awake() => Instance = this;

    void Start()
    {
        // Спавним предметы при самом первом запуске игры
        SpawnAllItems();
    }

    // Вызываем этот метод в начале КАЖДОГО цикла
    public void SpawnAllItems()
    {
        // 1. Удаляем старые предметы, которые валяются на сцене
        foreach (var item in _activeItems)
        {
            if (item != null) Destroy(item);
        }
        _activeItems.Clear();

        // 2. Спавним новые
        SpawnGroup(bananaPrefab, bananaPoints);
        SpawnGroup(foodPrefab, foodPoints);
        SpawnGroup(slippersPrefab, slippersPoints);
        SpawnGroup(mousePrefab, mousePoints);
    }

    private void SpawnGroup(GameObject prefab, Transform[] points)
    {
        if (prefab == null || points == null) return;

        foreach (var point in points)
        {
            if (point != null)
            {
                GameObject obj = Instantiate(prefab, point.position, Quaternion.identity);
                _activeItems.Add(obj);
            }
        }
    }
}
