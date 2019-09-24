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
        public GetNextProp(ZMachine2 machine, IZMemory contents)
            : base((ushort)OpCodes.GetNextProp, machine, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var obj = operands[0];
            var prop = operands[1];

            var next = prop == 0;

            var dest = GetNextByte();

            var zObj = Contents.ObjectTree[obj];

            var keyArray = zObj.Properties.Keys.ToArray();
            var propIdx = Array.FindIndex(keyArray, k => k == prop);

            var nextPropNum = 0;
            if (propIdx < keyArray.Length - 2)
            {
                nextPropNum = keyArray[propIdx++];
            }


            var propHeaderAddr = ObjectManager.GetPropertyHeaderAddress(obj);
            var size = MemoryManager.Get(propHeaderAddr);
            propHeaderAddr += (ushort)(size * 2 + 1);

            var variableManager = Contents.VariableManager;
            while (MemoryManager.Get(propHeaderAddr) != 0x00)
            {
                var propInfo = MemoryManager.Get(propHeaderAddr);
                byte len;
                if (Machine.Contents.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte)(MemoryManager.Get(++propHeaderAddr) & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte)((propInfo >> ((ushort) Machine.Contents.Header.Version <= 3 ? 5 : 6)) + 1);

                var propNum = (byte)(propInfo & ((ushort) Machine.Contents.Header.Version <= 3 ? 0x1f : 0x3f));

                if (next)
                {
                    Debug.Assert(propNum == nextPropNum);
                    variableManager.StoreByte(dest, propNum);
                    return;
                }

                if (propNum == prop)
                    next = true;

                propHeaderAddr += (ushort)(len + 1);
            }

            Debug.Assert(0 == nextPropNum);
            variableManager.StoreByte(dest, 0);
        }
    }
}