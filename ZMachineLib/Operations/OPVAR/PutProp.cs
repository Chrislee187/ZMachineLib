using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class PutProp : ZMachineOperation
    {
        public PutProp(ZMachine2 machine)
            : base((ushort)OpCodes.PutProp, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{ObjectManager.GetObjectName(args[0])}] ");

            var prop = ObjectManager.GetPropertyHeaderAddress(args[0]);
            var size = Machine.Memory[prop];
            prop += (ushort)(size * 2 + 1);

            while (Machine.Memory[prop] != 0x00)
            {
                var propInfo = Machine.Memory[prop++];
                byte len;
                if (Machine.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte)(Machine.Memory[prop++] & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte)((propInfo >> ((ushort) Machine.Header.Version <= 3 ? 5 : 6)) + 1);

                var propNum = (byte)(propInfo & ((ushort) Machine.Header.Version <= 3 ? 0x1f : 0x3f));
                if (propNum == args[1])
                {
                    if (len == 1)
                        Machine.Memory[prop + 1] = (byte)args[2];
                    else
                    {
                        ushort value = args[2];
                        Machine.Memory.StoreAt(prop, value);
                    }

                    break;
                }

                prop += len;
            }
        }
    }
}