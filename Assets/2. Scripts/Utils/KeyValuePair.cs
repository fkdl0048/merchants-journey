using UnityEngine;

namespace Scripts.Utils
{
    [System.Serializable]
    public class KeyValuePair<T1, T2>
    {
        [field: SerializeField] public T1 value;
        [field: SerializeField] public T2 value2;

        public KeyValuePair() { }
    }
}