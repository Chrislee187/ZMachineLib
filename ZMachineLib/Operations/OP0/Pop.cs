﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Pop : ZMachineOperationBase
    {
        public Pop(IZMemory contents)
            : base((ushort)OpCodes.Pop, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (Contents.Stack.CurrentRoutingAvailable())
            {
                Contents.Stack.PopCurrentRoutine();
            }
            else
            {
                Contents.Stack.Pop();
            }
        }
    }
}