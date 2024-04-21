using KBCore.Refs;
using UnityEngine;

[RequireComponent(typeof(ComboBarView))]
public class ComboBarController : ValidatedMonoBehaviour
{
    [Header("Config"), Tooltip("Config from where we take the values to config this bar")]
    public ComboBarConfig comboBarConfig;

    [HideInInspector, SerializeField, Self] GameplayEventListener onComboAchievedListener;
    [HideInInspector, SerializeField, Self] ComboBarView View;

    private ComboBarModel Model = new ComboBarModel();

    private bool IsBarFullAndReady(DamageTypeConfig vulnerability) => Model.IsWaitingForActivation && comboBarConfig.AreEqualDamageType(vulnerability);

    private void Start()
    {
        View.Setup(comboBarConfig.MaxComboValue, comboBarConfig.ShotTypeConfig.CrosshairColor, comboBarConfig.Icon);
    }

    public void OnComboAchieved(DamageTypeConfig vulnerability)
    {
        View.IncrementSlider(comboBarConfig.CalculateComboIncrement(vulnerability));

        //if the bar was already full, then we are waiting the player to activate the power up, we don't do anything else
        if(Model.IsWaitingForActivation || !comboBarConfig.AreEqualDamageType(vulnerability))
        {
            return;
        }

        //we check if the bar is full and then we raise the event
        if(comboBarConfig.IsBarFull(View.SliderValue))
        {
            comboBarConfig.RaiseOnBarFull();
        }
    }

    public void OnBarFull(DamageTypeConfig vulnerability)
    {
        //TODO: here we can activate an effect on the UI

        if(comboBarConfig.AreEqualDamageType(vulnerability))
        {
            Model.IsWaitingForActivation = true; //bar can now be activated
        }
    }

    public void OnActivationIntent(DamageTypeConfig vulnerability)
    {
        //if the bar is ready (full) and we they are the same damage type
        if(IsBarFullAndReady(vulnerability))
        {
            comboBarConfig.RiseOnActivated(vulnerability);
        }
    }

    public void OnComboBarActivated(DamageTypeConfig vulnerability)
    {
        //TODO: we can add another vfx here

        if (comboBarConfig.AreEqualDamageType(vulnerability))
        {
            Model.IsWaitingForActivation = false;
            View.ResetBar();
        }
    }

    public void TryToActivate(DamageTypeConfig vulnerability)
    {
        //if the bar is ready (full) and we they are the same damage type
        if (IsBarFullAndReady(vulnerability))
        {
            comboBarConfig.RiseOnActivationIntent(vulnerability);
        }
    }
}
