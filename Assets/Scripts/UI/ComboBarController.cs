using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

public class ComboBarController : ValidatedMonoBehaviour
{
    [Tooltip("Config from where we take the values to config this bar")]
    public ComboBarConfig comboBarConfig;

    [HideInInspector, SerializeField, Self] Slider barSlider;

    private Image fillImage;

    private void Start()
    {
        barSlider.maxValue = comboBarConfig.MaxComboValue;
        barSlider.value = 0;
        
        fillImage = barSlider.fillRect.GetComponent<Image>();
        fillImage.color = comboBarConfig.ShotTypeConfig.CrosshairColor;
    }
}
