using System;
using System.IO;
using UnityEngine;

namespace Game.Simulation
{
    public sealed class StateReader : IDisposable
    {
        private readonly MemoryStream stream;
        private readonly BinaryReader reader;

        public StateReader(byte[] data)
        {
            stream = new MemoryStream(data);
            reader = new BinaryReader(stream);
        }

        public bool ReadBool() => reader.ReadBoolean();
        public int ReadInt() => reader.ReadInt32();
        public float ReadFloat() => reader.ReadSingle();
        public string ReadString() => reader.ReadString();

        public Vector2 ReadVector2()
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            return new Vector2(x, y);
        }

        public Vector3 ReadVector3()
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float z = reader.ReadSingle();
            return new Vector3(x, y, z);
        }

        public void Dispose()
        {
            reader.Dispose();
            stream.Dispose();
        }
    }
}
