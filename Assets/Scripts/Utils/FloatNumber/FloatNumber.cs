using UnityEngine;
using TMPro;

public class FloatNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI floatNumberText;
    private ObjectPool objectPool;
    public event System.Action OnInitiateFloatNumber;

    public void InitFloatNumber(string text, ObjectPool objectPool, Color textColor = default)
    {
        floatNumberText.text = text;
        floatNumberText.color = textColor == default ? Color.white : textColor;
        this.objectPool ??= objectPool;
        OnInitiateFloatNumber?.Invoke();
    }

    public void OnCompleteFadeOut() => objectPool?.DisableObject(gameObject);
}
