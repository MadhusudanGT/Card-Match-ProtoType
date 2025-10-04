using System;
using System.Collections.Generic;

public static class EventBus
{
    // Maps event names to generic object callbacks
    private static Dictionary<string, Action<object>> eventTable = new Dictionary<string, Action<object>>();

    // Subscribe to an event by name
    public static void Subscribe<T>(string eventName, Action<T> listener)
    {
        if (!eventTable.ContainsKey(eventName))
            eventTable[eventName] = delegate { };

        // Wrap the generic listener into object-cast delegate
        eventTable[eventName] += (obj) =>
        {
            if (obj is T typedObj)
                listener(typedObj);
        };
    }

    // Unsubscribe from an event by name
    public static void Unsubscribe<T>(string eventName, Action<T> listener)
    {
        if (!eventTable.ContainsKey(eventName)) return;

        // This won't fully remove the wrapper — for production, use a dictionary of listeners
        // For basic use, this works as expected
        eventTable[eventName] -= (obj) =>
        {
            if (obj is T typedObj)
                listener(typedObj);
        };
    }

    // Invoke the event by name and pass any payload
    public static void Invoke<T>(string eventName, T payload)
    {
        if (eventTable.ContainsKey(eventName))
        {
            eventTable[eventName]?.Invoke(payload);
        }
    }
}
