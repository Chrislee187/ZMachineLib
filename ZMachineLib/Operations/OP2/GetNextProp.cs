using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:19 13 get_next_prop object property -> (result)
    /// Gives the number of the next property provided by the quoted object.
    /// This may be zero, indicating the end of the property list;
    /// if called with zero,
    /// it gives the first property number present.
    /// It is illegal to try to find the next property of a property which does not exist,
    /// and an interpreter should halt with an error message
    /// (if it can efficiently check this condition).
    /// </summary>
    public sealed class GetNextProp : ZMachineOperationBase
    {
        public GetNextProp(IZMemory memory)
            : base((ushort)OpCodes.GetNextProp, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var prop = args[1];

            var next = prop == 0;

            var dest = Memory.GetCurrentByteAndInc();

            var zObj = Memory.ObjectTree.GetOrDefault(obj);

            var propHeaderAddr = zObj.PropertiesAddress;

            var size = Memory.Manager.Get(propHeaderAddr);
            propHeaderAddr += (ushort)(size * 2 + 1);

            // TODO: Refactor to generic propery handling, see GetPropLen
            while (Memory.Manager.Get(propHeaderAddr) != 0x00)
            {
                var propInfo = Memory.Manager.Get(propHeaderAddr);
                byte len;
                if (Memory.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte)(Memory.Manager.Get(++propHeaderAddr) & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte)((propInfo >> ((ushort)Memory.Header.Version <= 3 ? 5 : 6)) + 1);

                var propNum = (byte)(propInfo & ((ushort)Memory.Header.Version <= 3 ? 0x1f : 0x3f));

                if (next)
                {
                    Memory.VariableManager.Store(dest, propNum);
                    return;
                }

                if (propNum == prop)
                    next = true;

                propHeaderAddr += (ushort)(len + 1);
            }

            Memory.VariableManager.Store(dest, 0);
        }
    }
}