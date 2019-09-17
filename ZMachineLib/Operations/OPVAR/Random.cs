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

        public override void Execute(List<ushort> operands)
        {
            ushort val = 0;

            if ((short)operands[0] <= 0)
                _random = new System.Random(-operands[0]);
            else
                val = (ushort)(_random.Next(0, operands[0]) + 1);

            var dest = GetNextByte();
            VariableManager.StoreWord(dest, val);
        }
    }
}