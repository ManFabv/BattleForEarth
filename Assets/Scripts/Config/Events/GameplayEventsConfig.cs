using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gameplay Event", menuName = "Events/Gameplay/Gameplay Event")]
public class GameplayEventsConfig : ScriptableObject
{
    private List<GameplayEventListener> listeners = new List<GameplayEventListener>();

    public void RegisterListener(GameplayEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameplayEventListener listener) 
    {
        listeners.Remove(listener);
    }

    public void RaiseGameplayEvent(DamageTypeConfig vulnerability)
    {
        foreach (GameplayEventListener listener in listeners)
        {
            if (listener.onEventRaised != null)
            {
                listener.onEventRaised.Invoke(vulnerability);
            }
        }
    }
}
