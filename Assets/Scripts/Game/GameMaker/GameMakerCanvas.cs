using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameMakerCanvas : MonoBehaviour
{
    [Header("Game Progress")]
    [SerializeField] private Slider gameProgressSlider;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private GameObject publishButton;

    [Header("Game Statistics")]
    [SerializeField] private TextMeshProUGUI statisticsText;

    private GameMakerController _gameMakerController;

    private void OnEnable()
    {
        _gameMakerController = GetComponent<GameMakerController>();

        _gameMakerController.OnGameProgressChanged += UpdateGameProgress;
        _gameMakerController.GetGameStatistics().OnValuesChanged += UpdateGameStatistics;
        _gameMakerController.OnProgressFinished += ResetGameProgressAndStatistics;

        UpdateGameProgress(0);
        UpdateGameStatistics();
    }

    private void OnDisable()
    {
        _gameMakerController.OnGameProgressChanged -= UpdateGameProgress;
        _gameMakerController.GetGameStatistics().OnValuesChanged -= UpdateGameStatistics;
        _gameMakerController.OnProgressFinished -= ResetGameProgressAndStatistics;
    }

    private void UpdateGameProgress(float progress)
    {
        publishButton.SetActive(progress > 0 ?  true : false);

        if(gameProgressSlider != null)
            gameProgressSlider.value = progress;

        if (progressText != null)
            progressText.text = $"Game Progress: {progress:F2}%";
    }

    private void UpdateGameStatistics()
    {
        statisticsText.text = GetStatisticsText();
    }

    private void ResetGameProgressAndStatistics(float value)
    {
        UpdateGameProgress(value);
        statisticsText.text = GetStatisticsText();
    }

    private string GetStatisticsText()
    {
        string programming = $"<color=#28ccdf><sprite=0>{NumberConverter.ConvertNumberToString(_gameMakerController.GetGameStatistics().ProgrammingValue.ToString())}</color>";
        string art = $"<color=#b6d53c><sprite=1>{NumberConverter.ConvertNumberToString(_gameMakerController.GetGameStatistics().ArtValue.ToString())}</color>";
        string sound = $"<color=#8e478c><sprite=2>{NumberConverter.ConvertNumberToString(_gameMakerController.GetGameStatistics().SoundValue.ToString())}</color>";
        string marketing = $"<color=#f47e1b><sprite=3>{NumberConverter.ConvertNumberToString(_gameMakerController.GetGameStatistics().MarketingValue.ToString())}</color>";
        string bugs = $"<color=#e6482e><sprite=4>{NumberConverter.ConvertNumberToString(_gameMakerController.GetGameStatistics().BugsValue.ToString())}</color>";

        return $"{programming}{art}{sound}{marketing}{bugs}";
    }
}
