using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class GetParent : ZMachineOperation
    {
        public GetParent(ZMachine2 machine)
            : base((ushort)OpCodes.GetParent, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{ObjectManager.GetObjectName(args[0])}] ");

            var addr = ObjectManager.GetObjectAddress(args[0]);
            var parent = ObjectManager.GetObjectNumber((ushort)(addr + Offsets.Parent));

            Log.Write($"[{ObjectManager.GetObjectName(parent)}] ");

            var dest = Memory[Stack.Peek().PC++];

            if (Version <= 3)
            {
                byte value = (byte)parent;
                VariableManager.StoreByte(dest, value);
            }
            else
                VariableManager.StoreWord(dest, parent);
        }
    }
}