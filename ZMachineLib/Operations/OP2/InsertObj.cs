using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:14 E insert_obj object destination
    /// Moves object O to become the first child of the destination object D.
    /// (Thus, after the operation the child of D is O, and the sibling of O
    /// is whatever was previously the child of D.) All children of O move with it.
    /// (Initially O can be at any point in the object tree; it may legally have parent zero.)
    /// </summary>
    public sealed class InsertObj : ZMachineOperationBase
    {
        public InsertObj(IZMemory contents)
            : base((ushort)OpCodes.InsertObj, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (args[0] == 0 || args[1] == 0)
                return;


            var obj1Number = args[0];
            var obj2Number = args[1];

            var zObj1 = Contents.ObjectTree[obj1Number];
            var zObj2 = Contents.ObjectTree[obj2Number];


            if (zObj1.Parent == obj2Number && zObj2.Child == obj1Number)
                return;

            var parent1ZObj = Contents.ObjectTree[zObj1.Parent];

            // if parent1's child is obj1 we need to assign the sibling
            if (parent1ZObj.Child == obj1Number)
            {
                // set parent1's child to obj1's sibling
                parent1ZObj.Child = zObj1.Sibling;

            }
            else // else if I'm not the child but there is a child, we need to link the broken sibling chain
            {
                // Need a longer test than the drink bottle of water with more interactions
                // to get more opcodes to be hit.



                var parent1ChildZObj = Contents.ObjectTree[parent1ZObj.Child];
                var addr = parent1ChildZObj.Address;
                var currentSibling = parent1ChildZObj.Sibling;

                // while sibling of parent1's child has siblings
                while (currentSibling != 0)
                {
                    // if obj1 is the sibling of the current object
                    if (currentSibling == obj1Number)
                    {
                        // set the current object's sibling to the next sibling
                        Contents.Manager.Set(
                            (ushort)(addr + Contents.Offsets.Sibling)
                            , (byte) zObj1.Sibling);
//                        parent1ZObj.Sibling = zObj1.Sibling;
                        break;
                    }

                    addr = Contents.ObjectTree[currentSibling].Address; // ObjectManager.GetObjectAddress(currentSibling);
                    
                    currentSibling = Contents.Manager.Get((ushort)(addr + Contents.Offsets.Sibling));
                }
            }

            // set obj1's parent to obj2
            Contents.Manager.Set((ushort)(zObj1.Address + Contents.Offsets.Parent), (byte) obj2Number);

            // set obj2's child to obj1
            Contents.Manager.Set((ushort)(zObj2.Address + Contents.Offsets.Child),(byte) obj1Number);

            // set obj1's sibling to obj2's child
            Contents.Manager.Set((ushort)(zObj1.Address + Contents.Offsets.Sibling), (byte)zObj2.Child);
        }
    }
}