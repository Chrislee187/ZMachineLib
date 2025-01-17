﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:17 11 get_prop object property -> (result)
    /// Read property from object (resulting in the default value if it had no such
    /// declared property).
    /// If the property has length 1, the value is only that byte.
    /// If it has length 2, the first two bytes of the property are taken as a word value.
    /// It is illegal for the opcode to be used if the property has length greater than 2,
    /// and the result is unspecified.
    /// </summary>
    public sealed class GetProp : ZMachineOperationBase
    {
        public GetProp(IZMemory memory)
            : base((ushort)OpCodes.GetProp, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory.GetCurrentByteAndInc();
            var obj = args[0];
            byte prop = (byte)args[1];
            var zObj = Memory.ObjectTree[obj];

            ushort valNew = 0;
            var propValues = zObj.GetPropertyOrDefault(prop);
            for (var i = 0; i < propValues.Data.Length; i++)
                valNew |= (ushort)(propValues.Data[i] << (propValues.Data.Length - 1 - i) * 8);

            Memory.VariableManager.Store(dest, valNew);
        }
    }
}