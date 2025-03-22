using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    [SerializeField] private bool SetParent = false;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float yOffset = 0f;

    public void SpawnObject(GameObject gameObject)
    {
        Vector3 spawnPos = transform.position + new Vector3(xOffset, yOffset, 0);

        GameObject currentObject = Instantiate(gameObject, spawnPos, Quaternion.identity);
        if (SetParent)
            currentObject.transform.SetParent(transform);
    }
}
