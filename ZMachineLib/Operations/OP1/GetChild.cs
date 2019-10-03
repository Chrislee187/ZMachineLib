using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:130 2 get_child object -> (result) ?(label)
    /// Get first object contained in given object,
    /// branching if this exists,
    /// i.e. is not nothing (i.e., is not 0).
    /// </summary>
    public sealed class GetChild : ZMachineOperationBase
    {
        public GetChild(IZMemory memory)
            : base((ushort)OpCodes.GetChild, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var zObj = Memory.ObjectTree.GetOrDefault(obj).RefreshFromMemory();
            var storageType = Memory.GetCurrentByteAndInc();

            if (Memory.Header.Version <= 3)
            {
                Memory.VariableManager.Store(storageType, (byte) zObj.Child);
            }
            else
            {
                Memory.VariableManager.Store(storageType, zObj.Child);
            }

            Memory.Jump(zObj.Child != 0);
        }
    }
}