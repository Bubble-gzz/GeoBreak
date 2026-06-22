using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Simulation
{
    public class FrameRecord
    {
        public int tick;
        public List<SimObjectState> objectStates;
        public List<string> registered;
        public List<string> unregistered;

        public FrameRecord(int tick, List<SimObjectState> objectStates, List<string> registered, List<string> unregistered)
        {
            this.tick = tick;
            this.objectStates = objectStates;
            this.registered = registered;
            this.unregistered = unregistered;
        }

        public FrameRecord Clone()
        {
            return new FrameRecord(
                tick,
                objectStates.Select(s => new SimObjectState(s.tick, s.objectId, (byte[])s.data.Clone())).ToList(),
                new List<string>(registered),
                new List<string>(unregistered)
            );
        }
    }

    public class GameHistory
    {
        public int seed { get; private set; }
        public List<TickContext> tickHistory { get; private set; }
        public List<SimObjectState>[] syncListOfEachTick { get; private set; }
        public Dictionary<string, string> objectTypeById { get; private set; }

        public GameHistory(
            int seed,
            List<TickContext> tickHistory,
            List<SimObjectState>[] syncListOfEachTick,
            Dictionary<string, string> objectTypeById)
        {
            this.seed = seed;
            this.tickHistory = tickHistory;
            this.syncListOfEachTick = syncListOfEachTick;
            this.objectTypeById = objectTypeById;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            /*
            foreach (FrameRecord frame in frameRecords)
            {
                sb.AppendLine($"[Frame {frame.tick}]");
                foreach (SimObjectState state in frame.objectStates.OrderBy(s => s.objectId, SimObjectIdComparer.Instance))
                {
                    sb.AppendLine($"    [object {state.objectId}]");
                    if (objectTypeById.TryGetValue(state.objectId, out string typeName))
                    {
                        StateDescriberRegistry.Describe(typeName, state.data, sb);
                    }
                }
                sb.AppendLine();
                sb.AppendLine($"register: [{FormatIdList(frame.registered)}]");
                sb.AppendLine($"unregister: [{FormatIdList(frame.unregistered)}]");
                sb.AppendLine();
            }
            */
            return sb.ToString();
        }

        static string FormatIdList(List<string> ids)
        {
            return string.Join(", ", ids);
        }
    }

    class SimObjectIdComparer : IComparer<string>
    {
        public static readonly SimObjectIdComparer Instance = new SimObjectIdComparer();

        public int Compare(string a, string b)
        {
            if (a == b) return 0;
            if (a == null) return -1;
            if (b == null) return 1;
            return int.Parse(a).CompareTo(int.Parse(b));
        }
    }
}
