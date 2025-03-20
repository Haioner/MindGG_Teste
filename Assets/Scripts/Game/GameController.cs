using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStatistics
{
    public float ProgrammingValue;
    public float ArtValue;
    public float SoundValue;
    public float MarketingValue;
}

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] private float currentGamePercentage;
    [SerializeField] private GameStatistics gameStatistics;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public GameStatistics GetGameStatistics() => gameStatistics;

    public void PublishGame()
    {
        Debug.Log("Game published!");
    }
}
