using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider levelSlider;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Values")]
    [SerializeField] private float currentMaxXP = 100f;
    [SerializeField] private float MaxXpMultiplier = 1.2f;
    [SerializeField] private float sliderLerpSpeed = 5f;

    public event System.Action<int> OnLevelUp;

    private int _currentLevel = 1;
    private float _currentXp = 0;
    private float _targetXp = 0;

    private void Start()
    {
        InitXP(currentMaxXP, MaxXpMultiplier);
    }

    private void Update()
    {
        if (levelSlider.value != _targetXp)
        {
            levelSlider.value = Mathf.Lerp(levelSlider.value, _targetXp, Time.deltaTime * sliderLerpSpeed);
            UpdateText();
        }
    }

    #region Custom Methods

    public float GetMaxXP() => currentMaxXP;
    public float GetCurrentXP() => _currentXp;
    public int GetCurrentLevel() => _currentLevel;

    public void InitXP(float startMaxXP, float maxXPMultiplier)
    {
        currentMaxXP = startMaxXP;
        MaxXpMultiplier = maxXPMultiplier;

        levelSlider.maxValue = currentMaxXP;
        levelSlider.value = _currentXp;
        UpdateText();
    }

    public void AddXp(float xp)
    {
        _currentXp += xp;
        if (_currentXp >= currentMaxXP)
        {
            _currentXp -= currentMaxXP;
            LevelUp();
        }
        _targetXp = _currentXp;
    }

    [ContextMenu("LevelUp")]
    private void LevelUp()
    {
        _currentLevel++;
        currentMaxXP *= MaxXpMultiplier;
        _targetXp = 0;

        levelSlider.maxValue = currentMaxXP;
        levelSlider.value = 0;

        UpdateText();

        OnLevelUp?.Invoke(_currentLevel);
    }

    private void UpdateText()
    {
        if (xpText != null)
            xpText.text = GetXpText();

        if (levelText != null)
            levelText.text = $"Lvl {_currentLevel}";
    }

    private string GetXpText()
    {
        string convertedXP = NumberConverter.ConvertNumberToString(_currentXp);
        string convertedMaxXP = NumberConverter.ConvertNumberToString(currentMaxXP);
        return $"{convertedXP}/{convertedMaxXP}";
    }
    #endregion
}
