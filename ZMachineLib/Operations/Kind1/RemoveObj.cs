using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class RemoveObj : ZMachineOperation
    {
        private readonly IZMachineIO _io;

        public RemoveObj(ZMachine2 machine)
            : base((ushort)Kind1OpCodes.RemoveObj, machine)
        {

        }

        public override void Execute(List<ushort> args)
        {
            if (args[0] == 0)
                return;

            Log.Write($"[{GetObjectName(args[0])}] ");
            ushort objAddr = GetObjectAddress(args[0]);
            ushort parent = GetObjectNumber((ushort)(objAddr + Machine.Offsets.Parent));
            ushort parentAddr = GetObjectAddress(parent);
            ushort parentChild = GetObjectNumber((ushort)(parentAddr + Machine.Offsets.Child));
            ushort sibling = GetObjectNumber((ushort)(objAddr + Machine.Offsets.Sibling));

            // if object is the first child, set first child to the sibling
            if (parent == args[0])
                SetObjectNumber((ushort)(parentAddr + Machine.Offsets.Child), sibling);
            else if (parentChild != 0)
            {
                ushort addr = GetObjectAddress(parentChild);
                ushort currentSibling = GetObjectNumber((ushort)(addr + Machine.Offsets.Sibling));

                // while sibling of parent1's child has siblings
                while (currentSibling != 0)
                {
                    // if obj1 is the sibling of the current object
                    if (currentSibling == args[0])
                    {
                        // set the current object's sibling to the next sibling
                        SetObjectNumber((ushort)(addr + Machine.Offsets.Sibling), sibling);
                        break;
                    }

                    addr = GetObjectAddress(currentSibling);
                    currentSibling = GetObjectNumber((ushort)(addr + Machine.Offsets.Sibling));
                }
            }

            // set the object's parent to nothing
            SetObjectNumber((ushort)(objAddr + Machine.Offsets.Parent), 0);
        }
    }
}