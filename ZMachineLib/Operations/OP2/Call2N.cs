﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:26 1A 5 call_2n routine arg1
    /// Executes routine(arg1) and throws away result.
    /// </summary>
    public sealed class Call2N : ZMachineOperationBase
    {
        public Call2N(ZMachine2 machine)
            : base((ushort)OpCodes.Call2N, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Call(operands, false);
        }
    }
}