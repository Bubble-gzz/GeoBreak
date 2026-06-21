using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game.Simulation
{
    public static class StateDescriberRegistry
    {
        private static readonly Dictionary<string, Action<StateReader, StringBuilder>> describers = new();

        public static void Register<T>(Action<StateReader, StringBuilder> describer) where T : ISimObject
        {
            describers[typeof(T).FullName] = describer;
        }

        public static void Describe(string typeName, byte[] data, StringBuilder sb)
        {
            if (!describers.TryGetValue(typeName, out Action<StateReader, StringBuilder> describer))
            {
                sb.AppendLine($"        (no describer for {typeName})");
                return;
            }
            if (data == null || data.Length == 0) return;
            using StateReader reader = new StateReader(data);
            describer(reader, sb);
        }
    }

    public static class StateSnapshotFormat
    {
        public const string FloatFormat = "F6";

        public static void AppendFloat(StringBuilder sb, string name, float value)
        {
            sb.AppendLine($"        {name}: {value.ToString(FloatFormat)}");
        }

        public static void AppendBool(StringBuilder sb, string name, bool value)
        {
            sb.AppendLine($"        {name}: {value.ToString().ToLower()}");
        }

        public static void AppendVector2(StringBuilder sb, string name, Vector2 value)
        {
            sb.AppendLine($"        {name}: ({value.x.ToString(FloatFormat)}, {value.y.ToString(FloatFormat)})");
        }
    }
}
