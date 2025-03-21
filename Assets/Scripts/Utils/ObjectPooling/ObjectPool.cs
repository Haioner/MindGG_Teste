using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;

    private List<GameObject> currentPool = new();
    private GameObject poolParent;

    private void Awake()
    {
        CreateParent();
        CreatePool();
    }

    #region Public Methods
    public void DisableObject(GameObject obj) => obj.SetActive(false);
    public void DisableAllObjects() => currentPool.ForEach(obj => obj.SetActive(false));
    public float GetActivePoolCount() => currentPool.Count(item => item.activeInHierarchy);

    public GameObject GetObject() { return currentPool.FirstOrDefault(obj => !obj.activeInHierarchy) ?? CreateObject(); }
    public GameObject GetObject_SetPosAndRot(Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetObject();
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }
    #endregion

    #region Private Methods
    private void CreateParent()
    {
        if (contentParent == null)
            poolParent = new GameObject { name = gameObject.name + "_Parent" };
        else
            poolParent = contentParent;
    }
    private void CreatePool() => Enumerable.Range(0, poolSize).ToList().ForEach(_ => CreateObject());

    private GameObject CreateObject()
    {
        GameObject obj = Instantiate(prefab, poolParent.transform);
        obj.SetActive(false);
        currentPool.Add(obj);
        return obj;
    }
    #endregion
}