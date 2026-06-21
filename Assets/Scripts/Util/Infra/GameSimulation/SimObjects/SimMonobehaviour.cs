using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;
namespace Game.Simulation
{
    public abstract class SimMonobehaviour : MonoBehaviour, ISimObject
    {
        virtual public ISimWorld simWorld { get; set; }
        virtual public int tickOrder { get => TickOrder.DefaultOrder; }
        virtual public void Init() {}
        virtual public void Tick(TickContext tickCtx) {}
        virtual public void SerializeState(StateWriter writer) {}
        virtual public void DeserializeState(StateReader reader) {}
        virtual public void Render(float deltaTime) {}
        protected GameObject SimInstantiate(ISimWorld simWorld, GameObject prefab, Transform parent)
        {
            GameObject instance = Instantiate(prefab, parent);
            RegisterSimObjectsUnder(simWorld, instance.transform);
            return instance;
        }

        protected GameObject SimInstantiate(ISimWorld simWorld, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject instance = Instantiate(prefab, position, rotation, parent);
            RegisterSimObjectsUnder(simWorld, instance.transform);
            return instance;
        }
        protected void AutoFillSimObjectField<T>(
            ref T field,
            bool autoAdd = true,
            bool searchInChildren = true
        ) where T : SimMonobehaviour
        {
            if (field != null) return;

            field = searchInChildren ? GetComponentInChildren<T>() : GetComponent<T>();
            if (field != null) {
                this.Log("Auto assigned " + typeof(T).Name, true);
                return;
            }

            if (!autoAdd) {
                this.LogError("No " + typeof(T).Name + " found", true);
                return;
            }

            field = gameObject.AddComponent<T>();
            this.Log("Auto added " + typeof(T).Name, true);
            if (simWorld != null) {
                simWorld.RegisterObject(field);
            }
        }

        private void RegisterSimObjectsUnder(ISimWorld simWorld, Transform root)
        {
            if (simWorld == null) return;
            var simObjects = Utils.FetchSimObjectsUnder(root);
            foreach (var simObject in simObjects)
            {
                simWorld.RegisterObject(simObject);
            }
        }
        protected void SimDestory()
        {
            if (simWorld != null) {
                var simObjects = Utils.FetchSimObjectsUnder(transform);
                foreach (var simObject in simObjects)
                {
                    simWorld.UnregisterObject(simObject);
                }
            }
            Destroy(gameObject);
        }
        protected virtual void SimOnCollisionEnter2D(SimCollision2D collision) {}
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject other = collision.rigidbody?.gameObject ?? collision.gameObject;
            SimOnCollisionEnter2D(new SimCollision2D(other));
        }
    }
}