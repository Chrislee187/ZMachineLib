using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public class Nop : ZMachineOperation {
        public override void Execute(List<ushort> args)
        {
            // 
        }

        public Nop(Kind0OpCodes opCode = Kind0OpCodes.Nop) 
            : base((ushort)opCode, null)
        {
        }
    }

}