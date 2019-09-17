﻿using System;
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

            var zObj = ObjectManager.GetObject(args[0]);
            var parentZObj = ObjectManager.GetObject(zObj.Parent);
            Log.Write($"[{zObj.Name}] parent [{parentZObj.Name}]");

            // if object is the first child, set first child to the sibling
            if (zObj.Parent == args[0])
            {
                ObjectManager.SetObjectNumber((ushort)(parentZObj.Address + Machine.VersionedOffsets.Child), zObj.Sibling);
            }
            else if (parentZObj.Child != 0)
            {
                var parentChildZObj = ObjectManager.GetObject(parentZObj.Child);

                var addr = ObjectManager.GetObjectAddress(parentZObj.Child);
                var current = ObjectManager.GetObject(parentZObj.Child);
                var currentSibling = ObjectManager.GetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling));
                Debug.Assert(currentSibling == current.Sibling);
                Debug.Assert(parentChildZObj.Sibling == current.Sibling);

                // while sibling of parent1's child has siblings
                while (current.Sibling != 0)
                {
                    //
                    // NOTE: Not found anything that hits this yet, do not refactor until we do
                    //
                    // if obj1 is the sibling of the current object
                    if (currentSibling == args[0])
                    {
                        // set the current object's sibling to the next sibling
                        Debug.Assert(current.Sibling == (ushort)(addr + Machine.VersionedOffsets.Sibling));
                        ObjectManager.SetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling), zObj.Sibling);
                        break;
                    }

                    addr = ObjectManager.GetObjectAddress(currentSibling);
                    current = ObjectManager.GetObject(currentSibling);
                    currentSibling = ObjectManager.GetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling));
                    Debug.Assert(currentSibling == current.Sibling);
                    throw new Exception("Been waiting to find something that hits this");
                }
            }

            // set the object's parent to nothing
            ObjectManager.SetObjectNumber((ushort)(zObj.Address + Machine.VersionedOffsets.Parent), 0);
        }
    }
}