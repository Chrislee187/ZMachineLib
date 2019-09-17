﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class OutputStream : ZMachineOperation
    {
        public OutputStream(ZMachine2 machine)
            : base((ushort)OpCodes.OutputStream, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            // TODO
            Log.WriteLine("VarOp.OutputSteam To Be Implemented");
        }
    }
}