using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmployeeControllerData
{
    public EmployeeSO EmployeeData;
    public float InitialUnlockPrice = 1000;
    public int StartEmployeesCount = 0;
    public bool StartLocked = true;
    public EmployeesController OverrideController;
}

public class EmployeeControllerCreator : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float spawnYOffset = 1.5f;
    [SerializeField] private EmployeesController controllerPrefab;
    [SerializeField] private List<EmployeeControllerData> employeeControllerDataList = new List<EmployeeControllerData>();

    private List<EmployeesController> _currentEmployeesControllers = new List<EmployeesController>();

    private void Awake()
    {
        foreach (EmployeeControllerData controllerData in employeeControllerDataList)
        {
            //Calculate Spawn Position
            float yOffset = _currentEmployeesControllers.Count * spawnYOffset;
            Vector3 spawnPosition = spawnPos.position + new Vector3(0, yOffset, 0f);

            //Select Controller Prefab
            EmployeesController currentControllerPrefab = controllerPrefab;
            if (controllerData.OverrideController != null)
                currentControllerPrefab = controllerData.OverrideController;

            //Instantiate Controller
            EmployeesController controller = Instantiate(currentControllerPrefab, spawnPosition, Quaternion.identity, transform);
            controller.InitEmployeeData(controllerData);
            _currentEmployeesControllers.Add(controller);
        }
    }
}
