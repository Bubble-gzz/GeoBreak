using System;

namespace Game.Simulation
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class TickOrder : Attribute
    {
        public const int DefaultOrder = 0;
        public const int ControlOrder = 2000;
        public const int RenderOrder = 3000;
        public int Order { get; }

        public TickOrder(int order)
        {
            Order = order;
        }
    }
}
