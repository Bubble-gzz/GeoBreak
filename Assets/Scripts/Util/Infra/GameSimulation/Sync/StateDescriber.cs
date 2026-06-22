using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game.Simulation
{
    public static class StateSnapshotFormat
    {
        public const string FloatFormat = "F6";
        public static void AppendObjectTitle(StringBuilder sb, string id)
        {
            sb.AppendLine($"    [{id}]");
        }

        public static void AppendDescription(StringBuilder sb, string description)
        {
            sb.AppendLine($"        {description}");
        }
        public static void AppendObjectTypeName(StringBuilder sb, string typeName)
        {
            sb.AppendLine($"        type: {typeName}");
        }
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
