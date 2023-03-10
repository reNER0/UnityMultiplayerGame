using Assets.Scripts.Network.Commands;
using System;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class Hub
    {
        [SerializeField]
        protected int _port = 3033;


        protected string CommandToString(ICommand cmd)
        {
            return JsonUtility.ToJson(cmd);
        }

        protected ICommand StringToCommand(string msg)
        {
            SerializableClass ctype = JsonUtility.FromJson<SerializableClass>(msg);
            Type t = Type.GetType(ctype.ClassName);
            ICommand gc = (ICommand)JsonUtility.FromJson(msg, t);
            return gc;
        }
    }
}
