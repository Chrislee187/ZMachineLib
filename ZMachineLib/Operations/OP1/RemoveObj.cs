using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:137 9 remove_obj object
    /// Detach the object from its parent, so that it no longer has any parent.
    /// (Its children remain in its possession.)
    /// </summary>
    public sealed class RemoveObj : ZMachineOperationBase
    {
        public RemoveObj(IZMemory memory)
            : base((ushort)OpCodes.RemoveObj, memory)
        {

        }

        public override void Execute(List<ushort> args)
        {
            var rootObjectNumber = args[0];
            if (rootObjectNumber == 0)
                return;

            var zObj = Memory.ObjectTree.GetOrDefault(rootObjectNumber);
            var parentZObj = Memory.ObjectTree.GetOrDefault(zObj.Parent);

            // if object is the first child, simply set first child to the sibling to skip the the object being de-etached
            if (parentZObj.Child == rootObjectNumber)
            {
                parentZObj.Child = zObj.Sibling;
            }
            else if (parentZObj.Child != 0)
            {
                RemoveObjectFromParent(zObj, Memory.ObjectTree.GetOrDefault(parentZObj.Child));
            }

            // set the object's parent to nothing
            zObj.Parent = 0;
        }

        private void RemoveObjectFromParent(IZMachineObject zObj, IZMachineObject firstChild)
        {
            var currentZObjToSet = firstChild;
            var nextSibling = Memory.ObjectTree.GetOrDefault(firstChild.Sibling);
            // while sibling of parent1's child has siblings
            while (nextSibling.ObjectNumber != 0)
            {
                // if obj1 is the sibling of the current object
                if (nextSibling.ObjectNumber == zObj.ObjectNumber)
                {
                    // set the current object's sibling to the next sibling
                    currentZObjToSet.Sibling = zObj.Sibling;
                    break;
                }

                currentZObjToSet = nextSibling;
                nextSibling = Memory.ObjectTree.GetOrDefault(nextSibling.Sibling);
            }
        }
    }
}