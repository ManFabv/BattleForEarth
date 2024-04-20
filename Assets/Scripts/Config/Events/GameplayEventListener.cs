using UnityEngine;
using UnityEngine.Events;

public class GameplayEventListener : MonoBehaviour
{
    [field: SerializeField, Header("Events")]
    public GameplayEventsConfig gameplayEventConfig {  get; private set; }
    [SerializeField]
    public UnityEvent<DamageTypeConfig> onEventRaised;

    private void OnEnable()
    {
        gameplayEventConfig.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameplayEventConfig.UnregisterListener(this);
    }
}
