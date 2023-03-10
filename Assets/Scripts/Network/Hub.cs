using Assets.Scripts.Network.Commands;
using System;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class Hub : MonoBehaviour
    {
        [SerializeField]
        protected string _ip;
        [SerializeField]
        protected int _port;


        protected static string CommandToString(ICommand cmd)
        {
            var json = JsonUtility.ToJson(cmd);
            return json;
        }

        protected static ICommand StringToCommand(string msg)
        {
            SerializableClass ctype = JsonUtility.FromJson<SerializableClass>(msg);
            Type t = Type.GetType(ctype.ClassName);
            ICommand gc = (ICommand)JsonUtility.FromJson(msg, t);
            return gc;
        }
    }
}
