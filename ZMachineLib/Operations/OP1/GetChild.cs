using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:130 2 get_child object -> (result) ?(label)
    /// Get first object contained in given object,
    /// branching if this exists,
    /// i.e. is not nothing (i.e., is not 0).
    /// </summary>
    public sealed class GetChild : ZMachineOperationBase
    {
        public GetChild(ZMachine2 machine)
            : base((ushort)OpCodes.GetChild, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var zObj = ObjectManager.GetObject(operands[0]);

            var dest = GetNextByte();

            // NOTE: Do we need to store if Child == 0 ???
            var variableManager = Contents.VariableManager;
            if (Machine.Contents.Header.Version <= 3)
            {
                variableManager.StoreByte(dest, (byte) zObj.Child);
            }
            else
            {
                variableManager.StoreWord(dest, zObj.Child);
            }

            Jump(zObj.Child != 0);
        }
    }
}