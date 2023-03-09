using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    [Serializable]
    public class SerializableCommandClass
    {
        [SerializeField]
        private string serializedClassName;

        public SerializableCommandClass()
        {
            serializedClassName = this.GetType().ToString();
        }

        public string GetClassName => serializedClassName;
    }
}
