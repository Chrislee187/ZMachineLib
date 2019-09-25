using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OPVAR
{
    /// <summary>
    /// VAR:227 3 put_prop object property value
    /// Writes the given value to the given property of the given object.
    /// If the property does not exist for that object, the interpreter should
    /// halt with a suitable error message. If the property length is 1,
    /// then the interpreter should store only the least significant byte
    /// of the value.
    /// (For instance, storing -1 into a 1-byte property results in the
    /// property value 255.)
    /// As with get_prop the property length must not be more than 2:
    /// if it is, the behaviour of the opcode is undefined.
    /// </summary>
    public sealed class PutProp : ZMachineOperationBase
    {
        public PutProp(IZMemory memory)
            : base((ushort)OpCodes.PutProp, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var obj = operands[0];
            var propertyNumber = operands[1];
            var value = operands[2];

            var zObj = Contents.ObjectTree.GetOrDefault(obj);

            zObj.Properties[propertyNumber].Data = value.ToByteArray();

            return;
        }
    }
}