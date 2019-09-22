﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Jump : ZMachineOperationBase
    {
        public Jump(ZMachine2 machine)
            : base((ushort)OpCodes.Jump, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Machine.Stack.Peek().PC = (uint)(Machine.Stack.Peek().PC + (short)(operands[0] - 2));
            Log.Write($"-> {Machine.Stack.Peek().PC:X5}");
        }
    }
}