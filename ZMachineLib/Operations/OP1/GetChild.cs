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
        public GetChild(IZMemory contents)
            : base((ushort)OpCodes.GetChild, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var zObj = Contents.ObjectTree.GetOrDefault(obj).RefreshFromMemory();
            var storageType = Contents.GetCurrentByteAndInc();

            if (Contents.Header.Version <= 3)
            {
                Contents.VariableManager.Store(storageType, (byte) zObj.Child);
            }
            else
            {
                Contents.VariableManager.Store(storageType, zObj.Child);
            }

            Contents.Jump(zObj.Child != 0);
        }
    }
}