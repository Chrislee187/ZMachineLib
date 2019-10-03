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
        public GetParent(IZMemory memory)
            : base((ushort) OpCodes.GetParent, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var zObj = Memory.ObjectTree.GetOrDefault(obj).RefreshFromMemory();
            var storageType = Memory.GetCurrentByteAndInc();

            if (Memory.Header.Version <= 3)
            {
                Memory.VariableManager.Store(storageType, (byte)zObj.Parent);
            }
            else
            {
                Memory.VariableManager.Store(storageType, zObj.Parent);
            }
        }
    }
}