using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class EraseWindow : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public EraseWindow(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.EraseWindow, machine, machine.Contents)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            _io.EraseWindow(operands[0]);
        }
    }
}