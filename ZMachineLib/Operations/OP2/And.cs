﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:9 9 and a b -> (result)
    /// Bitwise AND.
    /// </summary>
    public sealed class And : ZMachineOperationBase
    {
        public And(IZMemory memory)
            : base((ushort)OpCodes.And, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var result = (ushort)(args[0] & args[1]);
            var dest = Memory.GetCurrentByteAndInc();

            OpLogging.Op2WithStore(GetType().Name.ToUpper(), args[0], args[1], result, dest);


            Memory.VariableManager.Store(dest, result);
        }
    }
}