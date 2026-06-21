using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.EngineFacade
{
    public interface ITransform
    {
        Vector3 position { get; set; }
        Quaternion rotation { get; set; }
        Vector3 scale { get; set; }
    }
    public interface IRigidbody2D
    {
        Vector2 velocity { get; set; }
        Vector2 position { get; set; }
    }
}