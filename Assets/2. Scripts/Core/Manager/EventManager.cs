using System;
using System.Collections.Generic;

namespace Scripts.Manager
{
    public class EventManager
    {
        private Dictionary<string, Action<object[]>> eventDictionary = new Dictionary<string, Action<object[]>>();

        public void AddListener(string eventName, Action<object[]> listener)
        {
            if (!eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] = listener;
            }
            else
            {
                eventDictionary[eventName] += listener;
            }
        }

        public void RemoveListener(string eventName, Action<object[]> listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] -= listener;
            }
        }

        public void TriggerEvent(string eventName, params object[] parameters)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName]?.Invoke(parameters);
            }
        }
    }
}