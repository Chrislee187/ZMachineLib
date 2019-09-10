using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class RemoveObj : ZMachineOperation
    {
        public RemoveObj(ZMachine2 machine)
            : base((ushort)Kind1OpCodes.RemoveObj, machine)
        {

        }

        public override void Execute(List<ushort> args)
        {
            if (args[0] == 0)
                return;

            Log.Write($"[{GetObjectName(args[0])}] ");
            var objAddr = GetObjectAddress(args[0]);
            var parent = GetObjectNumber((ushort)(objAddr + Offsets.Parent));
            var parentAddr = GetObjectAddress(parent);
            var parentChild = GetObjectNumber((ushort)(parentAddr + Offsets.Child));
            var sibling = GetObjectNumber((ushort)(objAddr + Offsets.Sibling));

            // if object is the first child, set first child to the sibling
            if (parent == args[0])
                SetObjectNumber((ushort)(parentAddr + Offsets.Child), sibling);
            else if (parentChild != 0)
            {
                var addr = GetObjectAddress(parentChild);
                var currentSibling = GetObjectNumber((ushort)(addr + Offsets.Sibling));

                // while sibling of parent1's child has siblings
                while (currentSibling != 0)
                {
                    // if obj1 is the sibling of the current object
                    if (currentSibling == args[0])
                    {
                        // set the current object's sibling to the next sibling
                        SetObjectNumber((ushort)(addr + Offsets.Sibling), sibling);
                        break;
                    }

                    addr = GetObjectAddress(currentSibling);
                    currentSibling = GetObjectNumber((ushort)(addr + Offsets.Sibling));
                }
            }

            // set the object's parent to nothing
            SetObjectNumber((ushort)(objAddr + Offsets.Parent), 0);
        }
    }
}