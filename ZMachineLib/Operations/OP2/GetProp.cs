using System.Collections.Generic;
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
        public GetProp(IZMemory contents)
            : base((ushort)OpCodes.GetProp, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetCurrentByteAndInc();
            var obj = operands[0];
            byte prop = (byte)operands[1];
            var zObj = Contents.ObjectTree[obj];

            ushort valNew = 0;
            var propValues = zObj.GetProperty(prop);
            for (var i = 0; i < propValues.Data.Length; i++)
                valNew |= (ushort)(propValues.Data[i] << (propValues.Data.Length - 1 - i) * 8);

            Contents.VariableManager.StoreWord(dest, valNew);
        }
    }
}