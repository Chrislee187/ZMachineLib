using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:12 C clear_attr object attribute
    /// Make object not have the attribute numbered attribute
    /// </summary>
    public sealed class ClearAttribute : ZMachineOperation
    {
        public ClearAttribute(ZMachine2 machine,
            IObjectManager objectManager = null)
            : base((ushort)OpCodes.ClearAttribute, machine, objectManager: objectManager)
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