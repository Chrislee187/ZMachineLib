﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:20 14 add a b -> (result)
    /// Signed 16-bit addition.
    /// </summary>
    public sealed class Add : ZMachineOperationBase
    {
        public Add(IZMemory contents)
            : base((ushort)OpCodes.Add, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var result = (short)(args[0] + args[1]);
            var resultDestination = Contents.GetCurrentByteAndInc();

            OpLogging.Op2WithStore(this.GetType().Name.ToUpper(), args[0], args[1], result, resultDestination);

            Contents.VariableManager.Store(
                resultDestination, 
                (ushort) result
                );
        }
    }
}