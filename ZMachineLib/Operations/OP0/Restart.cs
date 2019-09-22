using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public class Restart : ZMachineOperationBase
    {
        public Restart(ZMachine2 machine)
            : base((ushort)OpCodes.Restart, machine)
        {
        }
        public override void Execute(List<ushort> operands)
        {
            Machine.ReloadFile();
        }

    }
}