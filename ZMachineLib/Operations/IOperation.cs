using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations
{
    public interface IOperation
    {
        ushort Code { get; }
        Action<bool> Jump { set; }
        Func<byte> GetNextByte { set; }
        void Execute(List<ushort> operands);
    }
}