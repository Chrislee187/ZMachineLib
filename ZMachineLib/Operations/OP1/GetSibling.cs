using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:129 1 get_sibling object -> (result) ?(label)
    /// Get next object in tree, branching if this exists, i.e. is not 0.
    /// </summary>
    public sealed class GetSibling : ZMachineOperationBase
    {
        public GetSibling(IZMemory memory)
            : base((ushort)OpCodes.GetSibling, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var zObj = Memory.ObjectTree.GetOrDefault(obj).RefreshFromMemory();
            var storageType = Memory.GetCurrentByteAndInc();

            if (Memory.Header.Version <= 3)
            {
                Memory.VariableManager.Store(storageType, (byte)zObj.Sibling);
            }
            else
                Memory.VariableManager.Store(storageType, zObj.Sibling);

            Memory.Jump(zObj.Sibling != 0);
        }
    }
}