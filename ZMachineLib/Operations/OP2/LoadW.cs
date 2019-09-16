using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OP2
{
    public sealed class LoadW : ZMachineOperation
    {
        public LoadW(ZMachine2 machine)
            : base((ushort)OpCodes.LoadW, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + 2 * args[1]);
            var word = Machine.Memory.GetUshort(addr);
            var dest = Memory[Stack.Peek().PC++];
            VarHandler.StoreWord(dest, word, true);
        }
    }
}