using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class SetColor : ZMachineOperation
    {
        private IUserIo _io;

        public SetColor(ZMachine2 machine,
            IUserIo io)
            : base((ushort)Kind2OpCodes.SetColor, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.SetColor((ZColor)args[0], (ZColor)args[1]);

        }
    }
}