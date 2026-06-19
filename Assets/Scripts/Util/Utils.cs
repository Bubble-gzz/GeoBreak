using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
namespace Game.Util
{
    public static class Utils
    {
        #region Logging
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(string msg)
        {
            UnityEngine.Debug.LogError($"[{typeof(Utils).FullName}] " + msg);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(string msg)
        {
            UnityEngine.Debug.LogWarning($"[{typeof(Utils).FullName}] " + msg);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string msg)
        {
            UnityEngine.Debug.Log($"[{typeof(Utils).FullName}] " + msg);
        }
        #endregion
    }
}