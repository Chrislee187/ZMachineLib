using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SetCursor : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public SetCursor(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.SetCursor, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            _io.SetCursor(operands[0], operands[1], (ushort)(operands.Count == 3 ? operands[2] : 0));

        }
    }
}