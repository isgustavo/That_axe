using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventListener> listeners = new List<GameEventListener>();
    public Dictionary<string, int> intExtra = new Dictionary<string, int>();

    public void Raise()
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnEventRaised();
		}
	}

    public void Raise(string key, int value)
    {
        intExtra.Add(key, value);
        Raise();
    }

	public void RegisterListener(GameEventListener listener)
	{
        if(listener != null)
        {
            listeners.Add(listener);
        }
	}

    public void UnregisterListener(GameEventListener listener)
    {
        if (listener != null)
        {
            listeners.Remove(listener);
        }
    }
}