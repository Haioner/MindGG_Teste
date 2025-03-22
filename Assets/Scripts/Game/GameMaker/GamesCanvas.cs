using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamesCanvas : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private Transform contentParent;
    [SerializeField] private List<Sprite> gameImages;
    [SerializeField] private TextMeshProUGUI emptyText;
    [SerializeField] private TextMeshProUGUI totalProfit;

    private Dictionary<PublishedGame, GameObject> gameItems = new Dictionary<PublishedGame, GameObject>();

    private void OnEnable()
    {
        PublishedGamesController.OnGameCreated += AddGameItem;
        PublishedGamesController.OnGameRemoved += RemoveGameItem;
        PublishedGamesController.OnGameUpdated += UpdateGameItems;
        PublishedGamesController.OnTotalChanged += UpdateTotalProfit;
        UpdateTotalProfit(0);
    }

    private void OnDisable()
    {
        PublishedGamesController.OnGameCreated -= AddGameItem;
        PublishedGamesController.OnGameRemoved -= RemoveGameItem;
        PublishedGamesController.OnGameUpdated -= UpdateGameItems;
        PublishedGamesController.OnTotalChanged -= UpdateTotalProfit;
    }

    private void AddGameItem(PublishedGame game)
    {
        emptyText.enabled = false;

        GameObject newItem = objectPool.GetObject_SetPosAndRot(contentParent.position, Quaternion.identity);

        GameItem item = newItem.GetComponent<GameItem>();
        Sprite randomImage = gameImages[Random.Range(0, gameImages.Count)];
        item.SetGameInfo(game.GamePopularity, game.GameProfit, game.GameName, randomImage);

        gameItems[game] = newItem;
        SortGameItems();
    }

    private void RemoveGameItem(PublishedGame game)
    {
        if (gameItems.TryGetValue(game, out GameObject item))
        {
            objectPool.DisableObject(item);
            gameItems.Remove(game);

            if (gameItems.Count <= 0)
                emptyText.enabled = true;
        }
    }

    private void UpdateGameItems()
    {
        SortGameItems();

        foreach (var kvp in gameItems)
        {
            PublishedGame game = kvp.Key;
            GameObject item = kvp.Value;

            GameItem gameItem = item.GetComponent<GameItem>();
            gameItem.SetGameInfo(game.GamePopularity, game.GameProfit);
        }
    }

    private void UpdateTotalProfit(double total)
    {
        totalProfit.text = $"<sprite=0>{NumberConverter.ConvertNumberToString(total.ToString(), false)}";
    }

    private void SortGameItems()
    {
        List<PublishedGame> sortedGames = new List<PublishedGame>(gameItems.Keys);
        sortedGames.Sort((a, b) => b.GamePopularity.CompareTo(a.GamePopularity));

        for (int i = 0; i < sortedGames.Count; i++)
        {
            GameObject item = gameItems[sortedGames[i]];
            item.transform.SetSiblingIndex(i);
        }
    }

}