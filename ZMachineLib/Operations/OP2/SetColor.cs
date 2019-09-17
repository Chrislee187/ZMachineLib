using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class SetColor : ZMachineOperation
    {
        private IUserIo _io;

        public SetColor(ZMachine2 machine,
            IUserIo io)
            : base((ushort)OpCodes.SetColor, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            _io.SetColor((ZColor)operands[0], (ZColor)operands[1]);

        }
    }
}