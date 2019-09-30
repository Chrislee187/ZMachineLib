using System.Collections.Generic;

namespace ZMachineLib.Managers
{
    public interface IOperandManager
    {
        List<ushort> GetOperands(byte opcode);
    }
}