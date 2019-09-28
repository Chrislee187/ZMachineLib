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
        public GetPropLen(IZMemory contents)
            : base((ushort)OpCodes.GetPropLen, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Contents.GetCurrentByteAndInc();
            var propInfo = Contents.Manager.Get(args[0] - 1);
            byte len;
            if (Contents.Header.Version > 3 && (propInfo & 0x80) == 0x80)
            {
                len = (byte) (Contents.Manager.Get(args[0] - 1) & 0x3f);
                if (len == 0)
                    len = 64;
            }
            else
                len = (byte)((propInfo >> ((ushort) Contents.Header.Version <= 3 ? 5 : 6)) + 1);

            Contents.VariableManager.Store(dest, len);
        }
    }
}