using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Game.Util;
namespace Game.Simulation
{
    public class InputProcessor : SingletonMonobehaviour<InputProcessor>
    {
        [SerializeField] private int inputDataCacheSize = 10;
        private Queue<InputData> inputDataCache;
        override protected void MyAwake()
        {
            inputDataCache = new Queue<InputData>(inputDataCacheSize);
        }
        void Update()
        {
            InputData inputData = GetInputSnapshot();
            inputDataCache.Enqueue(inputData);
            if (inputDataCache.Count > inputDataCacheSize)
            {
                inputDataCache.Dequeue();
            }
        }
        private InputData GetInputSnapshot()
        {
            InputData inputData = new InputData();
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                inputData.GetKeyDown[keyCode] = Input.GetKeyDown(keyCode);
                inputData.GetKeyUp[keyCode] = Input.GetKeyUp(keyCode);
                inputData.GetKey[keyCode] = Input.GetKey(keyCode);
            }
            inputData.GetAxisRaw["Horizontal"] = Input.GetAxisRaw("Horizontal");
            inputData.GetAxisRaw["Vertical"] = Input.GetAxisRaw("Vertical");
            Camera mainCamera = Camera.main;
            if (mainCamera != null) {
                Vector3 mouseScreenPosition = Input.mousePosition;
                mouseScreenPosition.z = -mainCamera.transform.position.z;
                inputData.mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            }
            return inputData;
        }
        public void ClearInputDataCache()
        {
            inputDataCache.Clear();
        }
        public InputData GetInputDataOverFrames()
        {
            InputData inputData = new InputData();
            if (inputDataCache.Count == 0) return inputData;

            foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                foreach(InputData frame in inputDataCache)
                {
                    inputData.GetKeyDown[keyCode] |= frame.GetKeyDown[keyCode];
                    inputData.GetKeyUp[keyCode] |= frame.GetKeyUp[keyCode];
                    inputData.GetKey[keyCode] |= frame.GetKey[keyCode];
                }
            }
            foreach(InputData frame in inputDataCache)
            {
                inputData.GetAxisRaw["Horizontal"] += frame.GetAxisRaw["Horizontal"];
                inputData.GetAxisRaw["Vertical"] += frame.GetAxisRaw["Vertical"];
                inputData.mouseWorldPosition += frame.mouseWorldPosition;
            }
            inputData.GetAxisRaw["Horizontal"] /= inputDataCache.Count;
            inputData.GetAxisRaw["Vertical"] /= inputDataCache.Count;
            inputData.mouseWorldPosition /= inputDataCache.Count;
            return inputData;
        }
        public InputData GetInputDataOfLatestFrame()
        {
            return inputDataCache.Peek();
        }
        public InputData ConsumeInputDataOverFrames()
        {
            InputData inputData = GetInputDataOverFrames();
            ClearInputDataCache();
            return inputData;
        }
    }
    public class InputData{
        public Dictionary<KeyCode, bool> GetKeyDown = new Dictionary<KeyCode, bool>();
        public Dictionary<KeyCode, bool> GetKeyUp = new Dictionary<KeyCode, bool>();
        public Dictionary<KeyCode, bool> GetKey = new Dictionary<KeyCode, bool>();
        public Dictionary<string, float> GetAxisRaw = new Dictionary<string, float>();
        public Vector2 mouseWorldPosition;
        public InputData()
        {
            foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                GetKeyDown[keyCode] = false;
                GetKeyUp[keyCode] = false;
                GetKey[keyCode] = false;
            }
            GetAxisRaw["Horizontal"] = 0f;
            GetAxisRaw["Vertical"] = 0f;
            mouseWorldPosition = Vector2.zero;
        }
        override public string ToString()
        {
            return "InputData";
        }
    }
}