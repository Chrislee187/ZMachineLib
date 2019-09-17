using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class InsertObj : ZMachineOperation
    {
        public InsertObj(ZMachine2 machine)
            : base((ushort)OpCodes.InsertObj, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (args[0] == 0 || args[1] == 0)
                return;

            Log.Write($"[{ObjectManager.GetObjectName(args[0])}] [{ObjectManager.GetObjectName(args[1])}] ");

            var obj1 = args[0];
            var obj2 = args[1];

            var obj1Addr = ObjectManager.GetObjectAddress(args[0]);
            var obj2Addr = ObjectManager.GetObjectAddress(args[1]);

            var parent1 = ObjectManager.GetObjectNumber((ushort)(obj1Addr + Machine.VersionedOffsets.Parent));
            var sibling1 = ObjectManager.GetObjectNumber((ushort)(obj1Addr + Machine.VersionedOffsets.Sibling));
            var child2 = ObjectManager.GetObjectNumber((ushort)(obj2Addr + Machine.VersionedOffsets.Child));

            var parent1Addr = ObjectManager.GetObjectAddress(parent1);

            var parent1Child = ObjectManager.GetObjectNumber((ushort)(parent1Addr + Machine.VersionedOffsets.Child));
            var parent1ChildAddr = ObjectManager.GetObjectAddress(parent1Child);
            var parent1ChildSibling = ObjectManager.GetObjectNumber((ushort)(parent1ChildAddr + Machine.VersionedOffsets.Sibling));

            if (parent1 == obj2 && child2 == obj1)
                return;

            // if parent1's child is obj1 we need to assign the sibling
            if (parent1Child == obj1)
            {
                // set parent1's child to obj1's sibling
                ObjectManager.SetObjectNumber((ushort)(parent1Addr + Machine.VersionedOffsets.Child), sibling1);
            }
            else // else if I'm not the child but there is a child, we need to link the broken sibling chain
            {
                var addr = parent1ChildAddr;
                var currentSibling = parent1ChildSibling;

                // while sibling of parent1's child has siblings
                while (currentSibling != 0)
                {
                    // if obj1 is the sibling of the current object
                    if (currentSibling == obj1)
                    {
                        // set the current object's sibling to the next sibling
                        ObjectManager.SetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling), sibling1);
                        break;
                    }

                    addr = ObjectManager.GetObjectAddress(currentSibling);
                    currentSibling = ObjectManager.GetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling));
                }
            }

            // set obj1's parent to obj2
            ObjectManager.SetObjectNumber((ushort)(obj1Addr + Machine.VersionedOffsets.Parent), obj2);

            // set obj2's child to obj1
            ObjectManager.SetObjectNumber((ushort)(obj2Addr + Machine.VersionedOffsets.Child), obj1);

            // set obj1's sibling to obj2's child
            ObjectManager.SetObjectNumber((ushort)(obj1Addr + Machine.VersionedOffsets.Sibling), child2);
        }
    }
}