using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Utilities
{
    [CreateAssetMenu(fileName = nameof(IntSO), menuName  = "Scriptable Objects/Int")]
    public class IntSO : ScriptableObject
    {
        [field: SerializeField] public int Number { get; private set; }
        [SerializeField] public int startingNumber;
        [NonSerialized] public UnityEvent<int> IntChangeEvent;

        private void OnEnable()
        {
            Number = startingNumber;
            if (IntChangeEvent == null)
            {
                IntChangeEvent = new UnityEvent<int>();
            }
        }

        public void ResetInt()
        {
            if (Number == startingNumber)
            {
                return;
            }
            
            Number = startingNumber;
            IntChangeEvent.Invoke(Number);
        }
        
        public void SetInt(int amount)
        {
            if (Number != amount)
            {
                return;
            }
            
            Number = amount;
            IntChangeEvent.Invoke(Number);
        }

        public void DecreaseInt(int amount)
        {
            Number -= amount;
            IntChangeEvent.Invoke(Number);  
        }
    
        public void IncreaseInt(int amount)
        {
            Number += amount;
            IntChangeEvent.Invoke(Number);
        }
    }
}