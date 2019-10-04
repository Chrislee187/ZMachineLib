using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class GetPropAddr : ZMachineOperationBase
    {
        public GetPropAddr(IZMemory memory)
            : base((ushort)OpCodes.GetPropAddr, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory.GetCurrentByteAndInc();

            var obj = args[0];
            var prop = (byte)args[1];
            var zObj = Memory.ObjectTree[obj];
            var addr = zObj.GetPropertyOrDefault(prop).DataAddress;

            Memory.VariableManager.Store(dest, addr);
        }
    }
}