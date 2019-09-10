using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class SetColor : ZMachineOperation
    {
        private IZMachineIO _io;

        public SetColor(ZMachine2 machine,
            IZMachineIO io)
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