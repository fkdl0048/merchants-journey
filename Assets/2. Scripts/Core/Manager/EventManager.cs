using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Manager
{
    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<string, Action<object[]>> eventDictionary = new Dictionary<string, Action<object[]>>();
    
        public void AddListener(string eventName, Action<object[]> listener)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException(nameof(eventName));
            
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
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException(nameof(eventName));
            
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] -= listener;
            }
        }
    
        public void TriggerEvent(string eventName, params object[] parameters)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException(nameof(eventName));
            
            if (eventDictionary.ContainsKey(eventName))
            {
                try
                {
                    eventDictionary[eventName]?.Invoke(parameters);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error triggering event {eventName}: {e.Message}");
                }
            }
        }
    
        public void ClearAllEvents()
        {
            eventDictionary.Clear();
        }
    }
}