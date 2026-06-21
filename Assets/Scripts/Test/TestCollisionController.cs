using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;

namespace Game.Test
{
    public class TestCollisionController : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            this.Log(
                "OnCollisionEnter2D: \n" +
                $"gameObject: {collision.gameObject.name}\n" +
                $"collider: {collision.collider?.gameObject.name}\n" +
                $"rigidbody: {collision.rigidbody?.gameObject.name}\n" +
                $"otherRigidbody: {collision.otherRigidbody?.gameObject.name}\n" +
                $"otherCollider: {collision.otherCollider?.gameObject.name}"
            );
       
        }
    }
}
