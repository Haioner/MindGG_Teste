using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameItem : MonoBehaviour
{
    [SerializeField] private Image gameImage;
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private TextMeshProUGUI gamePopularityText;
    [SerializeField] private TextMeshProUGUI gameProfitText;

    public void SetGameInfo(float popularity, float profit, string name = "", Sprite image = null)
    {
        if(image != null)
            gameImage.sprite = image;

        if(!string.IsNullOrEmpty(name))
            gameNameText.text = name;

        gamePopularityText.text = $"Fame: {NumberConverter.ConvertNumberToString(popularity.ToString())}";
        gameProfitText.text = $"Profit: {NumberConverter.ConvertNumberToString(profit.ToString(), false)}";
    }
}