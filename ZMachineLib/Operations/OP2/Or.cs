﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:8 8 or a b -> (result)
    /// Bitwise OR.
    /// </summary>
    public sealed class Or : ZMachineOperationBase
    {
        public Or(IZMemory contents)
            : base((ushort)OpCodes.Or, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetCurrentByteAndInc();
            Contents.VariableManager.StoreUShort(dest, (ushort)(operands[0] | operands[1]));
        }
    }
}