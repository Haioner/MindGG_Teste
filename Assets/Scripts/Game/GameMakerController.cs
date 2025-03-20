using System.Collections;
using UnityEngine;

public class GameMakerController : MonoBehaviour
{
    public static GameMakerController instance;

    [SerializeField] private float progressSpeedMultiplier = 1f;
    [SerializeField] private GameStatistics gameStatistics;

    public event System.Action<float> OnGameProgressChanged;
    public event System.Action<float> OnProgressFinished;

    private float _currentGameProgress;
    private Coroutine _gameProgressCoroutine;
    private CoinsController _coinsController;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        _coinsController = FindFirstObjectByType<CoinsController>();
    }

    private void OnEnable()
    {
        gameStatistics.OnValuesChanged += StartGameProgress;
    }

    private void OnDisable()
    {
        gameStatistics.OnValuesChanged -= StartGameProgress;
    }

    private void StartGameProgress()
    {
        if (_gameProgressCoroutine == null)
            _gameProgressCoroutine = StartCoroutine(GameProgress());
    }

    public float GetCurrentGamePercentage() => _currentGameProgress;
    public GameStatistics GetGameStatistics() => gameStatistics;

    public void PublishGame()
    {
        _coinsController.ChangeCoins(gameStatistics.GetMediumValue());
    }

    private IEnumerator GameProgress()
    {
        while (_currentGameProgress < 100)
        {
            _currentGameProgress += Time.deltaTime * progressSpeedMultiplier;
            OnGameProgressChanged?.Invoke(_currentGameProgress);
            yield return null;
        }

        PublishGame();
        ResetProgress();
        OnProgressFinished?.Invoke(_currentGameProgress);
    }

    private void ResetProgress()
    {
        _gameProgressCoroutine = null;
        _currentGameProgress = 0;
        gameStatistics.ResetWhitoutEvent();
    }
}
