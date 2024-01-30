using UnityEngine;

namespace PitchPerfect.Core
{
    public class Log
    {
        public static void Info(string message)
        {
#if UNITY_EDITOR
            Debug.Log($"{message}");
#endif
        }
        
        public static void Warning(string message)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=orange>{message}</color>");
#endif
        }
        
        public static void Error(string message)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=red>{message}</color>");
#endif
        }
    }
}