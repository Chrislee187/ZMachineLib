using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Jump if object a is a direct child of b, i.e., if parent of a is b.
    /// </summary>
    public sealed class Jin : ZMachineOperation
    {
        public Jin(ZMachine2 machine,
            IObjectManager objectManager = null,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.Jin, machine, objectManager, variableManager)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"C[{ObjectManager.GetObjectName(args[0])}] P[{ObjectManager.GetObjectName(args[1])}] ");

            var addr = ObjectManager.GetObjectAddress(args[0]);
            var objectA = ObjectManager.GetObjectParent((ushort)(addr));
            var objectB = args[1];
            Jump(objectA == objectB);
        }
    }
}