using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CheckArgCount : ZMachineOperation
    {
        public CheckArgCount(ZMachine2 machine)
            : base((ushort)OpCodes.CheckArgCount, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Jump(args[0] <= Stack.Peek().ArgumentCount);
        }
    }
}