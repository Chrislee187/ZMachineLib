using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class PutProp : ZMachineOperation
    {
        public PutProp(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.PutProp, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            var prop = GetPropertyHeaderAddress(args[0]);
            var size = Memory[prop];
            prop += (ushort)(size * 2 + 1);

            while (Memory[prop] != 0x00)
            {
                var propInfo = Memory[prop++];
                byte len;
                if (Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte)(Memory[prop++] & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte)((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

                var propNum = (byte)(propInfo & (Version <= 3 ? 0x1f : 0x3f));
                if (propNum == args[1])
                {
                    if (len == 1)
                        Memory[prop + 1] = (byte)args[2];
                    else
                        StoreWord(prop, args[2]);

                    break;
                }

                prop += len;
            }
        }
    }
}