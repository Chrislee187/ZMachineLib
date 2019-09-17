using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations
{
    public interface IOperation
    {
        ushort Code { get; }
        Action<bool> Jump { set; }
        Func<byte> PeekNextByte { set; }
        void Execute(List<ushort> operands);
    }
}