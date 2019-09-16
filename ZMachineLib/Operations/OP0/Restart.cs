using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public class Restart : ZMachineOperation
    {
        public Restart(ZMachine2 machine)
            : base((ushort)OpCodes.Restart, machine)
        {
        }
        public override void Execute(List<ushort> args)
        {
            Machine.ReloadFile();
        }

    }
}