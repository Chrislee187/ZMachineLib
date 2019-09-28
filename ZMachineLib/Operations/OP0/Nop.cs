using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public class Nop : ZMachineOperationBase {
        public override void Execute(List<ushort> args)
        {
            // 
        }

        public Nop(OpCodes opCode = OpCodes.Nop) 
            : base((ushort)opCode, null)
        {
        }
    }

}