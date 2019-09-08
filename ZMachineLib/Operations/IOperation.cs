using System.Collections.Generic;

namespace ZMachineLib.Operations
{
    public interface IOperation
    {
        Kind0OpCodes Code { get; }
        void Execute(List<ushort> args);
    }
}