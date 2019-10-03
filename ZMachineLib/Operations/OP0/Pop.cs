using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Pop : ZMachineOperationBase
    {
        public Pop(IZMemory memory)
            : base((ushort)OpCodes.Pop, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (Memory.Stack.CurrentRoutingAvailable())
            {
                Memory.Stack.PopCurrentRoutine();
            }
            else
            {
                Memory.Stack.Pop();
            }
        }
    }
}