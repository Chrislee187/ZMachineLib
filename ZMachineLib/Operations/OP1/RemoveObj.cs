using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class RemoveObj : ZMachineOperation
    {
        public RemoveObj(ZMachine2 machine)
            : base((ushort)OpCodes.RemoveObj, machine)
        {

        }

        public override void Execute(List<ushort> args)
        {
            if (args[0] == 0)
                return;

            Log.Write($"[{ObjectManager.GetObjectName(args[0])}] ");
            var objAddr = ObjectManager.GetObjectAddress(args[0]);
            var parent = ObjectManager.GetObjectNumber((ushort)(objAddr + Machine.VersionedOffsets.Parent));
            var parentAddr = ObjectManager.GetObjectAddress(parent);
            var parentChild = ObjectManager.GetObjectNumber((ushort)(parentAddr + Machine.VersionedOffsets.Child));
            var sibling = ObjectManager.GetObjectNumber((ushort)(objAddr + Machine.VersionedOffsets.Sibling));

            // if object is the first child, set first child to the sibling
            if (parent == args[0])
                ObjectManager.SetObjectNumber((ushort)(parentAddr + Machine.VersionedOffsets.Child), sibling);
            else if (parentChild != 0)
            {
                var addr = ObjectManager.GetObjectAddress(parentChild);
                var currentSibling = ObjectManager.GetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling));

                // while sibling of parent1's child has siblings
                while (currentSibling != 0)
                {
                    // if obj1 is the sibling of the current object
                    if (currentSibling == args[0])
                    {
                        // set the current object's sibling to the next sibling
                        ObjectManager.SetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling), sibling);
                        break;
                    }

                    addr = ObjectManager.GetObjectAddress(currentSibling);
                    currentSibling = ObjectManager.GetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling));
                }
            }

            // set the object's parent to nothing
            ObjectManager.SetObjectNumber((ushort)(objAddr + Machine.VersionedOffsets.Parent), 0);
        }
    }
}