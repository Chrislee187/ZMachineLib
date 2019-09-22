﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Bitwise OR
    /// </summary>
    public sealed class Or : ZMachineOperationBase
    {
        public Or(ZMachine2 machine,
            IZMemory contents)
            : base((ushort)OpCodes.Or, machine, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetNextByte();
            Contents.VariableManager.StoreWord(dest, (ushort)(operands[0] | operands[1]));
        }
    }
}