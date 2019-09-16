using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public class Nop : ZMachineOperation {
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