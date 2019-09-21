using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:130 2 get_child object -> (result) ?(label)
    /// Get first object contained in given object,
    /// branching if this exists,
    /// i.e. is not nothing (i.e., is not 0).
    /// </summary>
    public sealed class GetChild : ZMachineOperation
    {
        public GetChild(ZMachine2 machine, 
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.GetChild, machine, variableManager: variableManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var zObj = ObjectManager.GetObject(operands[0]);

            var dest = PeekNextByte();

            // NOTE: Do we need to store if Child == 0 ???
            if (Machine.Header.Version <= 3)
            {
                VariableManager.StoreByte(dest, (byte) zObj.Child);
            }
            else
            {
                VariableManager.StoreWord(dest, zObj.Child);
            }

            Jump(zObj.Child != 0);
        }
    }
}