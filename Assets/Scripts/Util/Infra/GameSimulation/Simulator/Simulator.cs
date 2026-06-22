using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
using Game.Util;
using System.Linq;
using System;
using System.Text;
using System.IO;
namespace Game.Simulation
{
    public abstract class Simulator : MonoBehaviour
    {
        [SerializeField] protected bool log = false;
        [SerializeField] protected string logFileName = "log.txt";
        [SerializeField] protected int logFrameInterval = 500;
        protected int seed;
        protected IRandomGenerator rng;
        protected SimRegistry simRegistry;
        protected List<ISimObject> objectsToUnregister = new List<ISimObject>();
        protected List<ISimObject> simListSnapshot;
        protected int tick;
        StreamWriter logWriter;
        protected void Init(int seed)
        {
            this.seed = seed;
            rng = new RandomGenerator(seed);
            simRegistry = new SimRegistry(this);
            var simObjects = Utils.FetchAllSimObjectsInScene();
            foreach (var simObject in simObjects)
            {
                simRegistry.RegisterSimulationObject(simObject);
            }
            ExtraInit();
            if (log) OpenLogFile();
        }
        protected virtual void ExtraInit() {}
        List<string> newlyRegisteredObjectsThisTick = new List<string>();
        List<string> newlyUnregisteredObjectsThisTick = new List<string>();
        public virtual void RegisterObject(ISimObject simObject) {
            simRegistry.RegisterSimulationObject(simObject);
            newlyRegisteredObjectsThisTick.Add(simRegistry.GetId(simObject));
        }
        public virtual void UnregisterObject(ISimObject simObject) {
            objectsToUnregister.Add(simObject);
            newlyUnregisteredObjectsThisTick.Add(simRegistry.GetId(simObject));
        }
        protected void UnregisterQueuedObjects()
        {
            foreach (var simObject in objectsToUnregister)
            {
                simRegistry.UnregisterSimulationObject(simObject);
            }
            objectsToUnregister.Clear();
        }

        protected virtual void BeforeTick()
        {
            simListSnapshot = simRegistry.GetOrderedSimulationObjects().ToList();
        }

        protected virtual void SimulateTick(TickContext tickCtx)
        {
            foreach (var simObject in simListSnapshot)
            {
                simObject.Tick(tickCtx);
            }
        }

        protected virtual void RenderTick(float deltaTime)
        {
            foreach (var simObject in simListSnapshot)
            {
                simObject.Render(deltaTime);
            }
        }

        protected virtual void EndTick()
        {
            if (log) LogFrameSnapshot();
            newlyRegisteredObjectsThisTick.Clear();
            newlyUnregisteredObjectsThisTick.Clear();
            UnregisterQueuedObjects();
        }

        protected void OpenLogFile()
        {
            CloseLogFile();
            string filePath = Path.Combine(Application.dataPath, "Logs", logFileName);
            logWriter = new StreamWriter(filePath, append: false);
        }

        protected void CloseLogFile()
        {
            if (logWriter == null) return;
            logWriter.Flush();
            logWriter.Dispose();
            logWriter = null;
        }

        void OnDestroy()
        {
            CloseLogFile();
        }

        protected virtual void LogFrameSnapshot()
        {
            string snapshot = GetFrameSnapshot(out bool shouldLog);
            if (!shouldLog || logWriter == null) return;
            logWriter.Write(snapshot);
            logWriter.Flush();
        }
        protected virtual string GetFrameSnapshot(out bool shouldLog)
        {
            shouldLog = tick % logFrameInterval == 0;
            shouldLog |= newlyRegisteredObjectsThisTick.Count > 0;
            shouldLog |= newlyUnregisteredObjectsThisTick.Count > 0;
            if (!shouldLog) return null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[Frame {tick}]");
            foreach (var simObject in simListSnapshot)
            {
                StateSnapshotFormat.AppendObjectTitle(sb, simRegistry.GetId(simObject));
                StateSnapshotFormat.AppendObjectTypeName(sb, simObject.GetType().Name);
                simObject.DescribeState(sb);
            }
            if (newlyRegisteredObjectsThisTick.Count > 0) {
                sb.AppendLine($"+ register: [{string.Join(", ", newlyRegisteredObjectsThisTick)}]");
                shouldLog = true;
            }
            if (newlyUnregisteredObjectsThisTick.Count > 0) {
                sb.AppendLine($"- unregister: [{string.Join(", ", newlyUnregisteredObjectsThisTick)}]");
                shouldLog = true;
            }
            return sb.ToString();
        }
        protected List<SimObjectState> CollectObjectStates(int tick)
        {
            var snapshots = new List<SimObjectState>();
            foreach (var simulationObject in simListSnapshot)
            {
                string objectId = simRegistry.GetId(simulationObject);
                StateWriter writer = new StateWriter();
                simulationObject.SerializeState(writer);
                snapshots.Add(new SimObjectState(tick, objectId, writer.ToArray()));
            }
            return snapshots;
        }
    }
    public interface ISimObject
    {
        Simulator simWorld { get; set; }
        int tickOrder { get; }
        void Init();
        void Tick(TickContext tickCtx);
        void SerializeState(StateWriter writer);
        void DeserializeState(StateReader reader);
        void Render(float deltaTime);
        void DescribeState(StringBuilder sb);
    }
    public class SimObjectState{
        public int tick;
        public string objectId;
        public byte[] data;
        public SimObjectState(int tick, string objectId, byte[] data)
        {
            this.tick = tick;
            this.objectId = objectId;
            this.data = data;
        }
    }
}