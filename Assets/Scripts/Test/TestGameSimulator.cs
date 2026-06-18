using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Simulation;
public class TestGameSimulator : MonoBehaviour
{
    // Start is called before the first frame update
    GameSimulator gameSimulator;
    void Awake()
    {
        gameSimulator = FindObjectOfType<GameSimulator>();
        if (gameSimulator == null) LogError("GameSimulator not found");
    }
    void Start()
    {
        StartCoroutine(TestGameSimulatorCoroutine());
    }

    IEnumerator TestGameSimulatorCoroutine()
    {
        yield return null;
        gameSimulator.RunSimualtion();
    }
    void LogError(string msg)
    {
        Debug.LogError($"[TestGameSimulator] " + msg);
    }
}
