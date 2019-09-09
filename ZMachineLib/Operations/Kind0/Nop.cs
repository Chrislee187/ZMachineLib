using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public class Nop : ZMachineOperation {
        public Kind0OpCodes Code { get; }
        public override void Execute(List<ushort> args)
        {
            // 
        }
        private static byte[] nopBytes;
        public Nop(Kind0OpCodes opCode = Kind0OpCodes.Nop) 
            : base((ushort)opCode, null)
        {
        }
    }
}