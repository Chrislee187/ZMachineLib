using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Random : ZMachineOperation
    {
        private static System.Random _random = new System.Random();
        public Random(ZMachine2 machine)
            : base((ushort)OpCodes.Random, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            ushort val = 0;

            if ((short)args[0] <= 0)
                _random = new System.Random(-args[0]);
            else
                val = (ushort)(_random.Next(0, args[0]) + 1);

            var dest = Memory[Stack.Peek().PC++];
            VarHandler.StoreWord(dest, val, true);
        }
    }
}