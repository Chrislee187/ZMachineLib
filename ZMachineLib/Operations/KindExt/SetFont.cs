using System.Collections.Generic;

namespace ZMachineLib.Operations.KindExt
{
    public sealed class SetFont : ZMachineOperation
    {
        public SetFont(ZMachine2 machine)
            : base((ushort)KindExtOpCodes.SetFont, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            // TODO

            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, 0);
        }
    }
}