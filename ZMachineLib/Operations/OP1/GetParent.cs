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
            : base((ushort) OpCodes.GetParent, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var zObj = ObjectManager.GetObject(operands[0]);

            var dest = PeekNextByte();

            if (Machine.Header.Version <= 3)
            {
                VariableManager.StoreByte(dest, (byte)zObj.Parent);
            }
            else
            {
                VariableManager.StoreWord(dest, zObj.Parent);
            }
        }
    }
}