using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class InsertObj : ZMachineOperation
    {
        public InsertObj(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.InsertObj, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (args[0] == 0 || args[1] == 0)
                return;

            Log.Write($"[{GetObjectName(args[0])}] [{GetObjectName(args[1])}] ");

            var obj1 = args[0];
            var obj2 = args[1];

            var obj1Addr = GetObjectAddress(args[0]);
            var obj2Addr = GetObjectAddress(args[1]);

            var parent1 = GetObjectNumber((ushort)(obj1Addr + Offsets.Parent));
            var sibling1 = GetObjectNumber((ushort)(obj1Addr + Offsets.Sibling));
            var child2 = GetObjectNumber((ushort)(obj2Addr + Offsets.Child));

            var parent1Addr = GetObjectAddress(parent1);

            var parent1Child = GetObjectNumber((ushort)(parent1Addr + Offsets.Child));
            var parent1ChildAddr = GetObjectAddress(parent1Child);
            var parent1ChildSibling = GetObjectNumber((ushort)(parent1ChildAddr + Offsets.Sibling));

            if (parent1 == obj2 && child2 == obj1)
                return;

            // if parent1's child is obj1 we need to assign the sibling
            if (parent1Child == obj1)
            {
                // set parent1's child to obj1's sibling
                SetObjectNumber((ushort)(parent1Addr + Offsets.Child), sibling1);
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
                        SetObjectNumber((ushort)(addr + Offsets.Sibling), sibling1);
                        break;
                    }

                    addr = GetObjectAddress(currentSibling);
                    currentSibling = GetObjectNumber((ushort)(addr + Offsets.Sibling));
                }
            }

            // set obj1's parent to obj2
            SetObjectNumber((ushort)(obj1Addr + Offsets.Parent), obj2);

            // set obj2's child to obj1
            SetObjectNumber((ushort)(obj2Addr + Offsets.Child), obj1);

            // set obj1's sibling to obj2's child
            SetObjectNumber((ushort)(obj1Addr + Offsets.Sibling), child2);
        }
    }
}