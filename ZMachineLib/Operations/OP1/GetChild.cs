using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class GetChild : ZMachineOperation
    {
        public GetChild(ZMachine2 machine)
            : base((ushort)OpCodes.GetChild, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{ObjectManager.GetObjectName(args[0])}] ");

            var addr = ObjectManager.GetObjectAddress(args[0]);
            var child = ObjectManager.GetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Child));

            Log.Write($"[{ObjectManager.GetObjectName(child)}] ");

            var dest = Machine.Memory[Machine.Stack.Peek().PC++];

            if (Machine.Header.Version <= 3)
            {
                byte value = (byte)child;
                VariableManager.StoreByte(dest, value);
            }
            else
                VariableManager.StoreWord(dest, child);

            Jump(child != 0);
        }
    }
}