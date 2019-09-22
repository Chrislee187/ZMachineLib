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
            : base((ushort)OpCodes.GetSibling, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var zObj = ObjectManager.GetObject(operands[0]);

            var dest = PeekNextByte();

            if (Machine.Header.Version <= 3)
            {
                VariableManager.StoreByte(dest, (byte)zObj.Sibling);
            }
            else
                VariableManager.StoreWord(dest, zObj.Sibling);

            Jump(zObj.Sibling != 0);
        }
    }
}