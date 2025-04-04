using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PublishedGame
{
    public string GameName;
    public float GamePopularity;
    public float GameProfit;
}

[System.Serializable]
public class GameNamesData
{
    public List<string> names;
    public List<string> lastNames;
}

public class PublishedGamesController : MonoBehaviour
{
    public static event System.Action<PublishedGame> OnGameCreated;
    public static event System.Action<PublishedGame> OnGameRemoved;
    public static event System.Action OnGameUpdated;
    public static event System.Action<double> OnTotalChanged;

    [SerializeField] private float popularityDecreaseRate = 0.01f;
    [SerializeField] private Vector2 randomPopularityDecrease, randomCoinsMultipliers;

    [Space][SerializeField] private List<PublishedGame> publishedGames = new List<PublishedGame>();

    private CoinsController _coinsController;
    private GameMakerController _gameMakerController;
    private GameNamesData gameNamesData;

    private void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("gameNames");
        if (jsonFile != null)
            gameNamesData = JsonUtility.FromJson<GameNamesData>(jsonFile.text);
        

        _coinsController = FindFirstObjectByType<CoinsController>();
        _gameMakerController = GameMakerController.instance;
        _gameMakerController.OnPublishGame += Publish;
        InvokeRepeating(nameof(UpdatePublishedGames), 5f, 5f);
    }

    private void OnDisable()
    {
        _gameMakerController.OnPublishGame -= Publish;
        CancelInvoke(nameof(UpdatePublishedGames));
    }

    public void Publish(GameStatistics gameStatistics, float gameProgress)
    {
        float averageStats = gameStatistics.GetAvarageValue();
        float normalizedProgress = Mathf.Clamp01(gameProgress / 100f);
        float gamePopularity = (averageStats + normalizedProgress) / 2f;

        string randomGameName = GenerateRandomGameName();

        PublishedGame newPublishedGame = new PublishedGame
        {
            GameName = randomGameName,
            GamePopularity = gamePopularity
        };

        publishedGames.Add(newPublishedGame);
        OnGameCreated?.Invoke(newPublishedGame);
    }

    private string GenerateRandomGameName()
    {
        if (gameNamesData == null || gameNamesData.names == null || gameNamesData.names.Count == 0)
            return "Unknown Game";
        
        string name = gameNamesData.names[Random.Range(0, gameNamesData.names.Count)];
        string lastName = "";

        if (gameNamesData.lastNames != null && gameNamesData.lastNames.Count > 0)
            lastName = Random.value > 0.5f ? " " + gameNamesData.lastNames[Random.Range(0, gameNamesData.lastNames.Count)] : "";

        return name + lastName;
    }

    private void UpdatePublishedGames()
    {
        double totalProfit = 0;
        for (int i = publishedGames.Count - 1; i >= 0; i--)
        {
            PublishedGame game = publishedGames[i];

            if (string.IsNullOrEmpty(game.GameName))
            {
                Debug.LogError("Game name is null or empty. Skipping...");
                publishedGames.RemoveAt(i);
                OnGameRemoved?.Invoke(game);
                continue;
            }

            float popularityDecrease = Random.Range(randomPopularityDecrease.x, randomPopularityDecrease.y) * 5f * (1f + game.GamePopularity);
            game.GamePopularity = Mathf.Max(0, game.GamePopularity - popularityDecrease);

            float coinsGenerated = game.GamePopularity * Random.Range(randomCoinsMultipliers.x, randomCoinsMultipliers.y);
            game.GameProfit = coinsGenerated;
            if (coinsGenerated > 0)
            {
                _coinsController.ChangeCoins((long)coinsGenerated);
                totalProfit += coinsGenerated;
            }

            if (game.GamePopularity <= 0)
            {
                Debug.Log($"Game removed: {game.GameName}");
                publishedGames.RemoveAt(i);
                OnGameRemoved?.Invoke(game);
            }
        }
        OnTotalChanged?.Invoke(totalProfit);
        OnGameUpdated?.Invoke();
    }
}