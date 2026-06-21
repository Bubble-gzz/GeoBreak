using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.EngineFacade
{
    public class UnityTransform : ITransform
    {
        public Transform transform { get; set; }
        public Vector3 position { get => transform.position; set => transform.position = value; }
        public Quaternion rotation { get => transform.rotation; set => transform.rotation = value; }
        public Vector3 scale { get => transform.localScale; set => transform.localScale = value; }
    }
}