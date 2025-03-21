using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameMakerCanvas : MonoBehaviour
{
    [Header("Game Progress")]
    [SerializeField] private Slider gameProgressSlider;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("Game Statistics")]
    [SerializeField] private TextMeshProUGUI statisticsText;

    private GameMakerController _gameMakerController;

    private void OnEnable()
    {
        _gameMakerController = GetComponent<GameMakerController>();

        _gameMakerController.OnGameProgressChanged += UpdateGameProgress;
        _gameMakerController.GetGameStatistics().OnValuesChanged += UpdateGameStatistics;
        _gameMakerController.OnProgressFinished += ResetGameProgressAndStatistics;

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
        return $"Programming: {NumberConverter.ConvertNumberToString(_gameMakerController.GetGameStatistics().ProgrammingValue.ToString())}\n" +
               $"Art: {NumberConverter.ConvertNumberToString(_gameMakerController.GetGameStatistics().ArtValue.ToString())}\n" +
               $"Sound: {NumberConverter.ConvertNumberToString(_gameMakerController.GetGameStatistics().SoundValue.ToString())}\n" +
               $"Marketing: {NumberConverter.ConvertNumberToString(_gameMakerController.GetGameStatistics().MarketingValue.ToString())}\n" +
               $"Bugs: {NumberConverter.ConvertNumberToString(_gameMakerController.GetGameStatistics().BugsValue.ToString())}";
    }
}
