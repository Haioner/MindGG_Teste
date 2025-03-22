using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = Color.gray;
    [SerializeField] private Color selectedColor = Color.green;

    private Button currentSelectedButton;

    private void Start()
    {
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
            SetButtonColor(button, normalColor, highlightedColor);
        }

        if (buttons.Count > 0)
        {
            OnButtonClick(buttons[0]);
        }
    }

    private void OnButtonClick(Button clickedButton)
    {
        if (currentSelectedButton != null)
        {
            SetButtonColor(currentSelectedButton, normalColor, highlightedColor);
        }

        currentSelectedButton = clickedButton;
        SetButtonColor(currentSelectedButton, selectedColor, highlightedColor);

        Debug.Log($"Botão selecionado: {clickedButton.gameObject.name}");
    }

    private void SetButtonColor(Button button, Color baseColor, Color highlightColor)
    {
        var colors = button.colors;
        colors.normalColor = baseColor;
        colors.highlightedColor = highlightColor;
        colors.selectedColor = baseColor;
        colors.pressedColor = baseColor * 0.75f;
        button.colors = colors;
    }
}