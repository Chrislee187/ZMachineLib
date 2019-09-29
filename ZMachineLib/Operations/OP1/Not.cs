using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:143 F 1/4 not value -> (result)
    /// VAR:248 18 5/6 not value -> (result)
    /// Bitwise NOT(i.e., all 16 bits reversed). Note that in Versions 3 and 4
    /// this is a 1OP instruction, reasonably since it has 1 operand, but in
    /// later Versions it was moved into the extended set to make room for call_1n.
    /// </summary>
    public sealed class Not : ZMachineOperationBase
    {
        public Not(IZMemory memory)
            : base((ushort)OpCodes.Not, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Contents.GetCurrentByteAndInc();
            ushort value = (ushort)~args[0];
            Contents.VariableManager.Store(dest, value);
        }
    }
}