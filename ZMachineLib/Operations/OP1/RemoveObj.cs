using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:137 9 remove_obj object
    /// Detach the object from its parent, so that it no longer has any parent.
    /// (Its children remain in its possession.)
    /// </summary>
    /// <remarks>
    /// Drink the bottle of water in zork1 to trigger this operation
    /// </remarks>
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
            var zObj = new ZMachineObject(args[0], ObjectManager);

            var objAddr = ObjectManager.GetObjectAddress(args[0]);
            Debug.Assert(zObj.Address == objAddr);

            var parent = ObjectManager.GetObjectNumber((ushort)(objAddr + Machine.VersionedOffsets.Parent));
            Debug.Assert(zObj.Parent == parent);

            var parentAddr = ObjectManager.GetObjectAddress(parent);
            var parentZObj = ObjectManager.GetObject(zObj.Parent);
            Debug.Assert(parentZObj.Address == parentAddr);

            var parentChild = ObjectManager.GetObjectNumber((ushort)(parentAddr + Machine.VersionedOffsets.Child));
            Debug.Assert(parentZObj.Child == parentChild);

            var sibling = ObjectManager.GetObjectNumber((ushort)(objAddr + Machine.VersionedOffsets.Sibling));
            Debug.Assert(zObj.Sibling == sibling);

            // if object is the first child, set first child to the sibling
            if (parent == args[0])
                ObjectManager.SetObjectNumber((ushort)(parentAddr + Machine.VersionedOffsets.Child), sibling);
            else if (parentChild != 0)
            {
                var addr = ObjectManager.GetObjectAddress(parentChild);
                var currentSibling = ObjectManager.GetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling));
                var parentChildZObj = ObjectManager.GetObject(parentChild);

                Debug.Assert(parentChildZObj.Sibling == currentSibling);
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