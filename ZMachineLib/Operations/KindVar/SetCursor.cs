using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class SetCursor : ZMachineOperation
    {
        private readonly IUserIo _io;

        public SetCursor(ZMachine2 machine, IUserIo io)
            : base((ushort)KindVarOpCodes.SetCursor, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.SetCursor(args[0], args[1], (ushort)(args.Count == 3 ? args[2] : 0));

        }
    }
}