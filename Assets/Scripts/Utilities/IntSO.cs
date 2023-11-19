using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    [CreateAssetMenu(fileName = nameof(IntSO), menuName  = "Scriptable Objects/Int")]
    public class IntSO : ScriptableObject
    {
        [field: SerializeField] public int Number { get; private set; }
        [SerializeField] private int startingNumber;
        [NonSerialized] private UnityEvent<int> intChangeEvent;

        private void OnEnable()
        {
            Number = startingNumber;
            if (intChangeEvent == null)
            {
                intChangeEvent = new UnityEvent<int>();
            }
        }
        
        public void SetInt(int amount)
        {
            Number = amount;
            intChangeEvent.Invoke(Number);
        }

        public void DecreaseInt(int amount)
        {
            Number -= amount;
            intChangeEvent.Invoke(Number);  
        }
    
        public void IncreaseInt(int amount)
        {
            Number += amount;
            intChangeEvent.Invoke(Number);
        }
    }
}