using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Diagnostics;
namespace Game.Util
{
    public static class Extensions
    {
    #region MetaInformation
        public static string GetTransformPath(this Transform transform)
        {
            string path = $"{transform.GetSiblingIndex()}:{transform.name}";
            Transform current = transform.parent;
            while (current != null)
            {
                path = $"{current.GetSiblingIndex()}:{current.name}/{path}";
                current = current.parent;
            }
            return path;
        }
        public static string GetComponentKey(this MonoBehaviour component)
        {
            string result = $"{component.GetType().FullName}@{component.transform.GetTransformPath()}";
            Utils.Log(result);
            return result;
        }
        public static T GetAttribute<T>(this object obj) where T : Attribute
        {
            return obj.GetType().GetCustomAttribute<T>(true);
        }
    #endregion

    #region Logging
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(this object obj, string msg, bool showObjectPath = false)
        {
            string prefix = obj.GetLogPrefix(showObjectPath);
            UnityEngine.Debug.LogError($"[{prefix}] " + msg);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(this object obj, string msg, bool showObjectPath = false)
        {
            string prefix = obj.GetLogPrefix(showObjectPath);
            UnityEngine.Debug.LogWarning($"[{prefix}] " + msg);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(this object obj, string msg, bool showObjectPath = false)
        {
            string prefix = obj.GetLogPrefix(showObjectPath);
            UnityEngine.Debug.Log($"[{prefix}] " + msg);
        }
        private static string GetLogPrefix(this object obj, bool showObjectPath = false)
        {
            string prefix = obj.GetType().FullName;
            if (showObjectPath)
            {
                if (obj is MonoBehaviour component)
                {
                    prefix = $"{prefix}@{component.transform.GetTransformPath()}";
                }
            }
            return prefix;
        }
    #endregion
    }
}
