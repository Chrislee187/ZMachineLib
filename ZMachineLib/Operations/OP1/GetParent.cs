using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:131 3 get_parent object -> (result)
    /// Get parent object
    /// (note that this has NO "branch if exists" clause as Get Child has).
    /// </summary>
    public sealed class GetParent : ZMachineOperationBase
    {
        public GetParent(IZMemory contents)
            : base((ushort) OpCodes.GetParent, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var zObj = Contents.ObjectTree.GetOrDefault(obj).RefreshFromMemory();
            var storageType = Contents.GetCurrentByteAndInc();

            if (Contents.Header.Version <= 3)
            {
                Contents.VariableManager.Store(storageType, (byte)zObj.Parent);
            }
            else
            {
                Contents.VariableManager.Store(storageType, zObj.Parent);
            }
        }
    }
}