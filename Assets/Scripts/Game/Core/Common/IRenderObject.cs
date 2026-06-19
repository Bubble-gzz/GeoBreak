using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Render
{    
    public interface IRenderObject<T>
    {
        void Render(T data, float deltaTime);
    }
}