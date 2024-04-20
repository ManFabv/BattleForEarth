using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

public class ComboBarView : ValidatedMonoBehaviour
{
    [Header("References")]
    public Image Icon;

    [HideInInspector, SerializeField, Self] public Slider barSlider;

    private Image fillImage;

    private void Awake()
    {
        fillImage = barSlider.fillRect.GetComponent<Image>();
    }

    public void Setup(int maxSliderValue, Color barColor, Sprite icon)
    {
        barSlider.maxValue = maxSliderValue;
        barSlider.value = 0;
        fillImage.color = barColor;

        Icon.color = barColor;
        Icon.sprite = icon;
    }

    public void IncrementSlider(int increment)
    {
        barSlider.value += increment;
    }

    public float SliderValue => barSlider.value;
}
