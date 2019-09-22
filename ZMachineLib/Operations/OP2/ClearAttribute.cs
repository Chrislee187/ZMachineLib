using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:12 C clear_attr object attribute
    /// Make object not have the attribute numbered attribute
    /// </summary>
    public sealed class ClearAttribute : ZMachineOperationBase
    {
        public ClearAttribute(ZMachine2 machine,
            IZMemory contents,
            IObjectManager objectManager = null)
            : base((ushort)OpCodes.ClearAttribute, machine, contents, objectManager: objectManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var zObj = ObjectManager.GetObject(operands[0]);
            Log.Write($"[{zObj.Name}] ");

            zObj.ClearAttribute(operands[1]);
        }
    }
}