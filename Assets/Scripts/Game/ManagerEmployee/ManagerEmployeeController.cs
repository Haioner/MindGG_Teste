using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ManagerEmployeeController : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private ManagerRaritySO managerRaritySO;
    private Rarity currentRarity;
    private float workTime;

    [Header("UI")]
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Manager Task Interface")]
    [SerializeField] private GameObject IManagerObject;
    private IManagerTask _iManagerTask;

    private float _currentTimeWorking;
    private Coroutine _timerCoroutine;

    public event System.Action<Rarity, float> OnSelectRarity;

    private void Awake()
    {
        startButton.onClick.AddListener(StartManager);

        _iManagerTask = IManagerObject.GetComponent<IManagerTask>();
        if(_iManagerTask != null)
            _iManagerTask.OnTaskFinished += AutomaticTaskStart;

        timeText.text = $"";
    }

    private void OnDisable()
    {
        _iManagerTask.OnTaskFinished -= AutomaticTaskStart;
    }

    private void AutomaticTaskStart()
    {
        //If the manager is working, the manager will start the task
        if (_currentTimeWorking > 0)
            _iManagerTask.StartTask();
    }

    public void StartManager()
    {
        if (_currentTimeWorking <= 0)
        {
            _currentTimeWorking = workTime;
            AutomaticTaskStart();
            startButton.interactable = false;
            if (_timerCoroutine == null)
                _timerCoroutine = StartCoroutine(DecreaseTimeWorking());
        }
    }

    public void SelectRarity()
    {
        int randRarirty = Random.Range(0, managerRaritySO.RarityList.Count);
        currentRarity = managerRaritySO.RarityList[randRarirty];

        float randTime = Random.Range(currentRarity.timePitch.x, currentRarity.timePitch.y);
        workTime = randTime;

        OnSelectRarity?.Invoke(currentRarity, workTime);
    }

    public Rarity GetCurrentRarity() => currentRarity;

    public void ResetManager()
    {
        timeText.text = $"";
        startButton.interactable = true;
        _currentTimeWorking = 0;
        _timerCoroutine = null;
    }

    private IEnumerator DecreaseTimeWorking()
    {
        while (_currentTimeWorking > 0)
        {
            _currentTimeWorking -= Time.deltaTime;
            timeText.text = $"{NumberConverter.GetTimePassed(_currentTimeWorking)}";
            yield return null;
        }

        ResetManager();
    }
}
