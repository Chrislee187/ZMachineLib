﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:132 4 get_prop_len property-address -> (result)
    /// Get length of property data(in bytes) for the given object's property.
    /// It is illegal to try to find the property length of a property which does
    /// not exist for the given object, and an interpreter should halt with an error
    /// message (if it can efficiently check this condition).
    /// @get_prop_len 0 must return 0. This is required by some Infocom games and files
    /// generated by old versions of Inform.
    /// </summary>
    public sealed class GetPropLen : ZMachineOperationBase
    {
        public GetPropLen(IZMemory memory)
            : base((ushort)OpCodes.GetPropLen, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var propAddress = args[0] - 1;
            var propInfo = Memory.Manager.Get((ushort) propAddress);
            var dest = Memory.GetCurrentByteAndInc();
            
            var len = ZProperty.GetPropertySize(propInfo);

            Memory.VariableManager.Store(dest, len);
        }
    }
}