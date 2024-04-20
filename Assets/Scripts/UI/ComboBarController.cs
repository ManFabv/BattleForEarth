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

    private void Start()
    {
        View.Setup(comboBarConfig.MaxComboValue, comboBarConfig.ShotTypeConfig.CrosshairColor, comboBarConfig.Icon);
    }

    public void OnComboAchieved(DamageTypeConfig vulnerability)
    {
        View.IncrementSlider(comboBarConfig.CalculateComboIncrement(vulnerability));

        //if the bar was already full, then we are waiting the player to activate the power up, we don't do anything else
        if(Model.IsWaitingForActivation)
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

        Model.IsWaitingForActivation = true; //bar can now be activated
    }

    //TODO: create a new GameplayEventConfig for activating a power up and on the player controller we
    //trigger the event passing the damage type as an argument. If the damage type is equals to this
    // combo bar and we have the Model.IsWaitingForActivation == true, we can activate the power up
    // and reset the slider and Model.IsWaitingForActivation = false
}
