using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:129 1 get_sibling object -> (result) ?(label)
    /// Get next object in tree, branching if this exists, i.e. is not 0.
    /// </summary>
    public sealed class GetSibling : ZMachineOperationBase
    {
        public GetSibling(ZMachine2 machine)
            : base((ushort)OpCodes.GetSibling, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var zObj = ObjectManager.GetObject(operands[0]);

            var dest = GetNextByte();

            var variableManager = Contents.VariableManager;
            if (Machine.Contents.Header.Version <= 3)
            {
                variableManager.StoreByte(dest, (byte)zObj.Sibling);
            }
            else
                variableManager.StoreWord(dest, zObj.Sibling);

            Jump(zObj.Sibling != 0);
        }
    }
}