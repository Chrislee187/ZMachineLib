﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:23 17 div a b -> (result)
    /// Signed 16-bit division.
    /// Division by zero should halt the interpreter with a suitable error message.
    /// </summary>
    public sealed class Div : ZMachineOperationBase
    {
        public Div(IZMemory contents)
            : base((ushort)OpCodes.Div, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Contents.GetCurrentByteAndInc();

            if (args[1] == 0)
            {
                // TODO: Log
                return;
            }

            ushort result = (ushort)((short)args[0] / (short)args[1]);

            OpLogging.Op2WithStore(this.GetType().Name.ToUpper(), args[0], args[1], result, dest);

            Contents.VariableManager.Store(dest, result);
        }
    }
}