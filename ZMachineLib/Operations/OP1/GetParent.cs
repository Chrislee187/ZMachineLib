using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:131 3 get_parent object -> (result)
    /// Get parent object
    /// (note that this has NO "branch if exists" clause as Get Child has).
    /// </summary>
    public sealed class GetParent : ZMachineOperationBase
    {
        public GetParent(ZMachine2 machine)
            : base((ushort) OpCodes.GetParent, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var zObj = ObjectManager.GetObject(operands[0]);

            var dest = GetNextByte();

            var variableManager = Contents.VariableManager;
            if (Machine.Contents.Header.Version <= 3)
            {
                variableManager.StoreByte(dest, (byte)zObj.Parent);
            }
            else
            {
                variableManager.StoreWord(dest, zObj.Parent);
            }
        }
    }
}