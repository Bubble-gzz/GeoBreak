using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindSystem<T>
{
    private Dictionary<T, KeyCode> keyBinds = new Dictionary<T, KeyCode>();
    public void Clear() => keyBinds.Clear();
    public void AddKeyBind(T key, KeyCode code) => keyBinds.Add(key, code);
    public void RemoveKeyBind(T key) => keyBinds.Remove(key);
    public void OverrideKeyBind(T key, KeyCode code) => keyBinds[key] = code;
    public KeyCode GetKeyCode(T key) => keyBinds[key];
}
