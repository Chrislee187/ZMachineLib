using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class ReadChar : ZMachineOperation
    {
        private readonly IUserIo _io;

        public ReadChar(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.ReadChar, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var key = _io.ReadChar();

            var dest = Machine.Memory[Machine.Stack.Peek().PC++];
            byte value = (byte)key;
            VariableManager.StoreByte(dest, value);
        }
    }
}