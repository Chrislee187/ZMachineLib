using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class GetProp : ZMachineOperation
    {
        public GetProp(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.GetProp, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            byte dest = Memory[Stack.Peek().PC++];
            ushort val = 0;

            ushort addr = GetPropertyAddress(args[0], (byte)args[1]);
            if (addr > 0)
            {
                byte propInfo = Memory[addr++];
                byte len;

                if (Version > 3 && (propInfo & 0x80) == 0x80)
                    len = (byte)(Memory[addr++] & 0x3f);
                else
                    len = (byte)((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

                for (int i = 0; i < len; i++)
                    val |= (ushort)(Memory[addr + i] << (len - 1 - i) * 8);
            }
            else
                val = GetWord((ushort)(ObjectTable + (args[1] - 1) * 2));

            StoreWordInVariable(dest, val);
        }
    }
}