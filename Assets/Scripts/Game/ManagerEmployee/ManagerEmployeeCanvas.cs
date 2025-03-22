using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ManagerEmployeeCanvas : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI rarityText;

    [Header("CACHE")]
    [SerializeField] private ShopItem shopItem;
    [SerializeField] private List<Image> managerIcons = new List<Image>();
    private ManagerEmployeeController employeeController;

    [System.Serializable]
    public class NameData
    {
        public List<string> name;
        public List<string> lastName;
    }

    private NameData nameData;

    private void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("people_names");
        if (jsonFile != null)
        {
            nameData = JsonUtility.FromJson<NameData>(jsonFile.text);

            if (nameData == null || nameData.name == null || nameData.lastName == null)
            {
                Debug.LogError("Failed to load name data or invalid JSON structure.");
            }
        }
        else
        {
            Debug.LogError("JSON file 'people_names' not found in Resources folder.");
        }
    }

    private void OnEnable()
    {
        employeeController = GetComponentInParent<ManagerEmployeeController>();
        employeeController.OnSelectRarity += UpdateUI;
    }

    private void OnDisable()
    {
        employeeController.OnSelectRarity -= UpdateUI;
    }

    private void UpdateUI(Rarity rarity, float workTime)
    {
        rarityText.text = $"{rarity.raritys.ToString()} {NumberConverter.GetTimePassed(workTime)}";

        if (managerIcons.Count > 0)
        {
            icon.enabled = true;
            int randIcon = Random.Range(0, managerIcons.Count);
            icon.sprite = managerIcons[randIcon].sprite;
        }

        if (nameData != null && nameData.name.Count > 0 && nameData.lastName.Count > 0)
        {
            string randomName = nameData.name[Random.Range(0, nameData.name.Count)];
            string randomSurname = nameData.lastName[Random.Range(0, nameData.lastName.Count)];
            nameText.text = $"{randomName} {randomSurname}";
        }
        else
        {
            Debug.LogWarning("Name data is missing or empty. Unable to generate a random name.");
            nameText.text = "Unknown";
        }
    }
}