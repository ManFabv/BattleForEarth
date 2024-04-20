using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

public class ComboBarController : ValidatedMonoBehaviour
{
    [Header("Config"), Tooltip("Config from where we take the values to config this bar")]
    public ComboBarConfig comboBarConfig;

    [Header("References")]
    public Image Icon;

    [HideInInspector, SerializeField, Self] Slider barSlider;
    [HideInInspector, SerializeField, Self] GameplayEventListener onComboAchievedListener;

    private Image fillImage;

    private void Start()
    {
        barSlider.maxValue = comboBarConfig.MaxComboValue;
        barSlider.value = 0;
        
        fillImage = barSlider.fillRect.GetComponent<Image>();
        fillImage.color = comboBarConfig.ShotTypeConfig.CrosshairColor;

        Icon.color = comboBarConfig.ShotTypeConfig.CrosshairColor;
        Icon.sprite = comboBarConfig.Icon;
    }

    public void OnComboAchieved(DamageTypeConfig vulnerability)
    {
        barSlider.value += comboBarConfig.CalculateComboIncrement(vulnerability);

        //TODO: here we should not continue triggering the event after the first time
        // and then when we use this power up, we should reset the value and start
        // triggering the event again when it's full
        if(comboBarConfig.IsBarFull(barSlider))
        {
            comboBarConfig.RaiseOnBarFull();
        }
    }

    public void OnBarFull(DamageTypeConfig vulnerability)
    {
        //TODO: here we can activate an effect on the UI
    }
}
