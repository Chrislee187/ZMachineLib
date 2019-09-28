using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class GetPropAddr : ZMachineOperationBase
    {
        public GetPropAddr(IZMemory contents)
            : base((ushort)OpCodes.GetPropAddr, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Contents.GetCurrentByteAndInc();

            var obj = args[0];
            var prop = (byte)args[1];
            var zObj = Contents.ObjectTree[obj];
            var addr = zObj.GetProperty(prop).DataAddress;

            Contents.VariableManager.Store(dest, addr);
        }
    }
}