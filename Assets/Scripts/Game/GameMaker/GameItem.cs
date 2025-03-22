using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class GameItem : MonoBehaviour
{
    [SerializeField] private Image gameImage;
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private TextMeshProUGUI gamePopularityText;
    [SerializeField] private TextMeshProUGUI gameProfitText;
    [SerializeField] private List<DOTweenAnimation> dotAnims;

    public void SetGameInfo(float popularity, float profit, string name = "", Sprite image = null)
    {
        if(image != null)
            gameImage.sprite = image;

        if(!string.IsNullOrEmpty(name))
            gameNameText.text = name;

        gamePopularityText.text = $"<sprite=0>{NumberConverter.ConvertNumberToString(popularity.ToString())}";
        gameProfitText.text = $"<sprite=0>{NumberConverter.ConvertNumberToString(profit.ToString(), false)}";

        foreach (var dot in dotAnims)
        {
            dot.DORestart();
        }
    }
}