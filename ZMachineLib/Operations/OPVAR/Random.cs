using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Random : ZMachineOperationBase
    {
        private static System.Random _random = new System.Random();
        public Random(IZMemory memory)
            : base((ushort)OpCodes.Random, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            ushort val = 0;

            if ((short)args[0] <= 0)
                _random = new System.Random(-args[0]);
            else
                val = (ushort)(_random.Next(0, args[0]) + 1);

            var dest = Contents.GetCurrentByteAndInc();
            Contents.VariableManager.Store(dest, val);
        }
    }
}