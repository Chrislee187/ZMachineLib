using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class ReadChar : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public ReadChar(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.ReadChar, machine, machine.Contents)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            var key = _io.ReadChar();

            var dest = GetNextByte();
            byte value = (byte)key;
            Contents.VariableManager.StoreByte(dest, value);
        }
    }
}