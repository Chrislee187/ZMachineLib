using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SetTextStyle : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public SetTextStyle(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.SetTextStyle, machine, machine.Contents)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            _io.SetTextStyle((TextStyle)operands[0]);
        }
    }
}