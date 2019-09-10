using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class GetPropLen : ZMachineOperation
    {
        public GetPropLen(ZMachine2 machine)
            : base((ushort)Kind1OpCodes.GetPropLen, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory[Stack.Peek().PC++];
            var propInfo = Memory[args[0] - 1];
            byte len;
            if (Version > 3 && (propInfo & 0x80) == 0x80)
            {
                len = (byte)(Memory[args[0] - 1] & 0x3f);
                if (len == 0)
                    len = 64;
            }
            else
                len = (byte)((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

            StoreByteInVariable(dest, len);
        }
    }
}