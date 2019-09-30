using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public class Nop : ZMachineOperationBase {
        public override void Execute(List<ushort> args)
        {
            // 
        }

        public Nop(IZMemory memory) 
            : base((ushort) OpCodes.Nop, memory)
        {
        }
    }

}