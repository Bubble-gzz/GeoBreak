using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Simulation
{
    public class FrameRecord
    {
        public int tick;
        public List<SimObjectState> objectStates;
        public List<int> registered;
        public List<int> unregistered;

        public FrameRecord(int tick, List<SimObjectState> objectStates, List<int> registered, List<int> unregistered)
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
                new List<int>(registered),
                new List<int>(unregistered)
            );
        }
    }

    public class GameHistory
    {
        public int seed { get; private set; }
        public List<TickContext> tickHistory { get; private set; }
        public List<SimObjectState> stateHistory { get; private set; }
        public List<FrameRecord> frameRecords { get; private set; }
        public Dictionary<int, string> objectTypeById { get; private set; }

        public GameHistory(
            int seed,
            List<TickContext> tickHistory,
            List<SimObjectState> stateHistory,
            List<FrameRecord> frameRecords,
            Dictionary<int, string> objectTypeById)
        {
            this.seed = seed;
            this.tickHistory = tickHistory;
            this.stateHistory = stateHistory;
            this.frameRecords = frameRecords;
            this.objectTypeById = objectTypeById;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (FrameRecord frame in frameRecords)
            {
                sb.AppendLine($"[Frame {frame.tick}]");
                foreach (SimObjectState state in frame.objectStates.OrderBy(s => s.objectId))
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
            return sb.ToString();
        }

        static string FormatIdList(List<int> ids)
        {
            return string.Join(", ", ids);
        }
    }
}
