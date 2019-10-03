using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// jump
    /// 1OP:140 C jump ? (label)
    ///
    /// Jump(unconditionally) to the given label.
    /// (This is not a branch instruction and the operand is a 2-byte
    /// signed offset to apply to the program counter.)
    /// It is legal for this to jump into a different routine
    /// (which should not change the routine call state),
    /// although it is considered bad practice to do so
    /// and the Txd disassembler is confused by it.
    ///
    /// The destination of the jump opcode is:
    ///
    /// Address after instruction + Offset - 2
    ///
    /// This is analogous to the calculation for branch offsets.
    /// </summary>
    public sealed class Jump : ZMachineOperationBase
    {
        public Jump(IZMemory memory)
            : base((ushort)OpCodes.Jump, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var offset = (short)(args[0] - 2);
            Memory.Stack.IncrementPC(offset);
            Log.Write($"-> {Memory.Stack.GetPC():X5}");
        }
    }
}