using System;
using System.IO;
using UnityEngine;

namespace Game.Simulation
{
    public sealed class StateWriter : IDisposable
    {
        private readonly MemoryStream stream;
        private readonly BinaryWriter writer;

        public StateWriter()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public void WriteBool(bool value) => writer.Write(value);
        public void WriteInt(int value) => writer.Write(value);
        public void WriteFloat(float value) => writer.Write(value);
        public void WriteString(string value) => writer.Write(value ?? string.Empty);

        public void WriteVector2(Vector2 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
        }

        public void WriteVector3(Vector3 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
        }

        public byte[] ToArray()
        {
            writer.Flush();
            return stream.ToArray();
        }

        public void Dispose()
        {
            writer.Dispose();
            stream.Dispose();
        }
    }
}
