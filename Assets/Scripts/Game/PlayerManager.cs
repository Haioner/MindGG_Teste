using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Game Progress")]
    public float GameProgressSpeedMultiplier = 1;
    [SerializeField] private float MaxProgressMultiplier = 30;
    [SerializeField] private float addProgressMultiplier = 0.05f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public float CalculateAdditionalValue(int levelAdd)
    {
        float potentialNewValue = GameProgressSpeedMultiplier + (levelAdd * addProgressMultiplier);
        float additionalValue = Mathf.Min(potentialNewValue, MaxProgressMultiplier) - GameProgressSpeedMultiplier;

        return additionalValue > 0 ? additionalValue : 0;
    }

    public void IncreaseGameProgressSpeed(int levelAdd)
    {
        float newSpeedMultiplier = GameProgressSpeedMultiplier + (levelAdd * addProgressMultiplier);
        GameProgressSpeedMultiplier = Mathf.Min(newSpeedMultiplier, MaxProgressMultiplier);
    }
}
