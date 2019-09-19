using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:16 10 loadb array byte-index -> (result)
    /// Stores array->byte-index(i.e., the byte at address array+byte-index,
    /// which must lie in static or dynamic memory).
    /// </summary>
    public sealed class LoadB : ZMachineOperation
    {
        public LoadB(ZMachine2 machine,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.LoadB, machine, variableManager: variableManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + operands[1]);
            var dest = PeekNextByte();
            VariableManager.StoreByte(dest, MemoryManager.Get(addr));
        }
    }
}