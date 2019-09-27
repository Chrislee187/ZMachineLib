﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Store : ZMachineOperationBase
    {
        public Store(IZMemory contents)
            : base((ushort)OpCodes.Store, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            ushort value = operands[1];
            Contents.VariableManager.StoreUShort((byte)operands[0], value, false);
        }
    }
}