using System.Collections.Generic;
using ZMachineLib.Operations.Kind0;

namespace ZMachineLib.Operations.Kind1
{
    public class Kind1Operations : Dictionary<Kind1OpCodes, IOperation>
    {
        public Kind1Operations(ZMachine2 machine,
            IZMachineIo io)
        {
            Add(Kind1OpCodes.Jz, new Jz(machine));
            Add(Kind1OpCodes.GetSibling, new GetSibling(machine));
            Add(Kind1OpCodes.GetChild, new GetChild(machine));
            Add(Kind1OpCodes.GetParent, new GetParent(machine));
            Add(Kind1OpCodes.GetPropLen, new GetPropLen(machine));
            Add(Kind1OpCodes.Inc, new Inc(machine));
            Add(Kind1OpCodes.Dec, new Dec(machine));
            Add(Kind1OpCodes.PrintAddr, new PrintAddr(machine, io));
            Add(Kind1OpCodes.Call1S, new Call1S(machine));
            Add(Kind1OpCodes.RemoveObj, new RemoveObj(machine));
            Add(Kind1OpCodes.PrintObj, new PrintObj(machine, io));
            Add(Kind1OpCodes.Ret, new Ret(machine));
            Add(Kind1OpCodes.Jump, new Jump(machine));
            Add(Kind1OpCodes.PrintPAddr, new PrintPAddr(machine, io));
            Add(Kind1OpCodes.Load, new Load(machine));

            if (machine.Version <= 4)
            {
                Add(Kind1OpCodes.Not, new Not(machine));
            }
            else
            {
                Add(Kind1OpCodes.Call1N, new Call1N(machine));
            }
        }
    }
}