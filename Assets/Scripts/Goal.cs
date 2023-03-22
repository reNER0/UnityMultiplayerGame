using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Goal : MonoBehaviour
    {
        public Action OnGoal;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Ball") 
            {
                OnGoal?.Invoke();
            }
        }
    }
}
