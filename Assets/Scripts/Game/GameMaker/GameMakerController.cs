using System.Collections;
using UnityEngine;

public class GameMakerController : MonoBehaviour, IManagerTask
{
    public static GameMakerController instance;

    [SerializeField] private GameStatistics gameStatistics;

    [Header("Bugs")]
    [SerializeField] private float bugChanceMultiplier = 0.3f;
    [SerializeField] private float bugRate = 1f;
    [SerializeField] private float maxBugChance = 0.5f;

    public event System.Action OnTaskFinished;
    public event System.Action<float> OnGameProgressChanged;
    public event System.Action<float> OnProgressFinished;
    public event System.Action<GameStatistics, float> OnPublishGame;

    private float _currentGameProgress;
    private Coroutine _gameProgressCoroutine;
    private Coroutine _bugCoroutine;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        gameStatistics.OnValuesChanged += StartGameMaker;
    }

    private void OnDisable()
    {
        gameStatistics.OnValuesChanged -= StartGameMaker;
    }

    public float GetCurrentGamePercentage() => _currentGameProgress;
    public GameStatistics GetGameStatistics() => gameStatistics;

    public void PublishGame()
    {
        if (_currentGameProgress <= 0) return;

        OnPublishGame?.Invoke(gameStatistics, _currentGameProgress);
        RestartGameMaker();
        OnProgressFinished?.Invoke(_currentGameProgress);
    }

    private void StartGameMaker()
    {
        if (_gameProgressCoroutine == null)
        {
            _gameProgressCoroutine = StartCoroutine(GameProgress());
            _bugCoroutine = StartCoroutine(AddBugsOverTime());
        }
    }

    private void RestartGameMaker()
    {
        if (_gameProgressCoroutine != null)
        {
            StopCoroutine(_gameProgressCoroutine);
            _gameProgressCoroutine = null;
        }

        if(_bugCoroutine != null)
        {
            StopCoroutine(_bugCoroutine);
            _bugCoroutine = null;
        }

        _currentGameProgress = 0;
        gameStatistics.ResetWhitoutEvent();
    }

    public void StartTask()
    {
        if (_currentGameProgress >= 100)
            PublishGame();
    }

    #region Progress
    private IEnumerator GameProgress()
    {
        while (_currentGameProgress < 100)
        {
            _currentGameProgress += Time.deltaTime * PlayerManager.instance.GameProgressSpeedMultiplier;
            OnGameProgressChanged?.Invoke(_currentGameProgress);
            yield return null;
        }
        _currentGameProgress = Mathf.RoundToInt(100);
        OnGameProgressChanged?.Invoke(_currentGameProgress);
        OnTaskFinished?.Invoke();
    }
    #endregion

    #region Bugs
    private IEnumerator AddBugsOverTime()
    {
        while (true)
        {
            float randBug = Random.value;
            //The more average game statistics, the more bugs
            float mediumValue = gameStatistics.GetAvarageValue();
            float adjustedBugChance = Mathf.Clamp(bugChanceMultiplier * (mediumValue / 100), 0, maxBugChance);
            if (randBug <= adjustedBugChance)
            {
                gameStatistics.BugsValue += Random.Range(1, 3);
            }
            yield return new WaitForSeconds(bugRate);
        }
    }
    #endregion
}
