﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class SetFont : ZMachineOperationBase
    {
        public SetFont(IZMemory memory)
            : base((ushort)KindExtOpCodes.SetFont, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = Contents.GetCurrentByteAndInc();
            Contents.VariableManager.StoreWord(dest, 0);
        }
    }
}