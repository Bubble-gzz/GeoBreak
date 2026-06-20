using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Simulation;
public class TestGameSimulator : MonoBehaviour
{
    // Start is called before the first frame update
    HostRealtimeSimulator hostRealtimeSimulator;
    PlaybackSimulator playbackSimulator;
    [SerializeField] Transform playerTransform;
    void Awake()
    {
        hostRealtimeSimulator = FindObjectOfType<HostRealtimeSimulator>();
        if (hostRealtimeSimulator == null) LogError("HostRealtimeSimulator not found");
        playbackSimulator = FindObjectOfType<PlaybackSimulator>();
        if (playbackSimulator == null) LogError("PlaybackSimulator not found");
    }
    void Start()
    {
        StartCoroutine(RunHostRealtimeSimulatorCoroutine());
    }

    IEnumerator RunHostRealtimeSimulatorCoroutine()
    {
        yield return null;
        Init();
        hostRealtimeSimulator.RunWithRandomSeed();
    }
    public void RunPlaybackSimulator()
    {
        hostRealtimeSimulator.Stop();
        StartCoroutine(RunPlaybackSimulatorCoroutine());
    }
    IEnumerator RunPlaybackSimulatorCoroutine()
    {
        yield return null;
        Init();
        GameHistory gameHistory = hostRealtimeSimulator.ExportGameHistory();
        playbackSimulator.Run(gameHistory);
    }
    void Init()
    {
        playerTransform.position = new Vector3(0, 0, 0);
    }
    void LogError(string msg)
    {
        Debug.LogError($"[TestGameSimulator] " + msg);
    }
}
