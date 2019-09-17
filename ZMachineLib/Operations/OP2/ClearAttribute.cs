using System.Collections.Generic;
using System.Diagnostics;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:12 C clear_attr object attribute
    /// Make object not have the attribute numbered attribute
    /// </summary>
    public sealed class ClearAttribute : ZMachineOperation
    {
        public ClearAttribute(ZMachine2 machine)
            : base((ushort)OpCodes.ClearAttribute, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var zObj = ObjectManager.GetObject(args[0]);
            Log.Write($"[{zObj.Name}] ");

            zObj.ClearAttribute(args[1]);
        }
    }
}