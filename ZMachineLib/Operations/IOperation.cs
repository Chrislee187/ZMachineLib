using System.Collections.Generic;

namespace ZMachineLib.Operations
{
    public interface IOperation
    {
        ushort Code { get; }
        void Execute(List<ushort> args);
    }
}