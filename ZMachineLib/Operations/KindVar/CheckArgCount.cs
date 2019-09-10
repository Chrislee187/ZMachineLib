using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class CheckArgCount : ZMachineOperation
    {
        public CheckArgCount(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.CheckArgCount, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Jump(args[0] <= Stack.Peek().ArgumentCount);
        }
    }
}