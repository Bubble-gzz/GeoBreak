using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Game.Simulation
{
    public static class InputManager
    {
        public static InputData GetInputData()
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
            return inputData;
        }
    }
    public class InputData{
        public Dictionary<KeyCode, bool> GetKeyDown = new Dictionary<KeyCode, bool>();
        public Dictionary<KeyCode, bool> GetKeyUp = new Dictionary<KeyCode, bool>();
        public Dictionary<KeyCode, bool> GetKey = new Dictionary<KeyCode, bool>();
        public Dictionary<string, float> GetAxisRaw = new Dictionary<string, float>();
        override public string ToString()
        {
            return "InputData";
        }
    }
}