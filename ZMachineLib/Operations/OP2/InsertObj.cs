using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:14 E insert_obj object destination
    /// Moves object O to become the first child of the destination object D.
    /// (Thus, after the operation the child of D is O, and the sibling of O
    /// is whatever was previously the child of D.) All children of O move with it.
    /// (Initially O can be at any point in the object tree; it may legally have parent zero.)
    /// </summary>
    public sealed class InsertObj : ZMachineOperation
    {
        public InsertObj(ZMachine2 machine)
            : base((ushort)OpCodes.InsertObj, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            if (operands[0] == 0 || operands[1] == 0)
                return;

            Log.Write($"[{ObjectManager.GetObjectName(operands[0])}] [{ObjectManager.GetObjectName(operands[1])}] ");

            var obj1 = operands[0];
            var obj2 = operands[1];

            var zObj1 = ObjectManager.GetObject(obj1);
            var zObj2 = ObjectManager.GetObject(obj2);


            if (zObj1.Parent == obj2 && zObj2.Child == obj1)
                return;

            var parent1ZObj = ObjectManager.GetObject(zObj1.Parent);

            // if parent1's child is obj1 we need to assign the sibling
            if (parent1ZObj.Child == obj1)
            {
                // set parent1's child to obj1's sibling
                ObjectManager.SetObjectNumber((ushort)(parent1ZObj.Address + Machine.VersionedOffsets.Child), zObj1.Sibling);
            }
            else // else if I'm not the child but there is a child, we need to link the broken sibling chain
            {
                var parent1ChildZObj = ObjectManager.GetObject(parent1ZObj.Child);
                var addr = parent1ChildZObj.Address;
                var currentSibling = parent1ChildZObj.Sibling;

                // while sibling of parent1's child has siblings
                while (currentSibling != 0)
                {
                    // if obj1 is the sibling of the current object
                    if (currentSibling == obj1)
                    {
                        // set the current object's sibling to the next sibling
                        ObjectManager.SetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling), zObj1.Sibling);
                        break;
                    }

                    addr = ObjectManager.GetObject(currentSibling).Address; // ObjectManager.GetObjectAddress(currentSibling);
                    
                    currentSibling = ObjectManager.GetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling));
                }
            }

            // set obj1's parent to obj2
            ObjectManager.SetObjectNumber((ushort)(zObj1.Address + Machine.VersionedOffsets.Parent), obj2);

            // set obj2's child to obj1
            ObjectManager.SetObjectNumber((ushort)(zObj2.Address + Machine.VersionedOffsets.Child), obj1);

            // set obj1's sibling to obj2's child
            ObjectManager.SetObjectNumber((ushort)(zObj1.Address + Machine.VersionedOffsets.Sibling), zObj2.Child);
        }
    }
}