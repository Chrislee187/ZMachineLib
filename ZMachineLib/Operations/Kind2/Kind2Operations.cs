using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public class Kind2Operations : Dictionary<Kind2OpCodes, IOperation>
    {
        public Kind2Operations(ZMachine2 machine,
            IZMachineIO io)
        {
            Add(Kind2OpCodes.Je, new Je(machine));
            Add(Kind2OpCodes.Jl, new Jl(machine));
            Add(Kind2OpCodes.Jg, new Jg(machine));
            Add(Kind2OpCodes.DecCheck, new DecCheck(machine));
            Add(Kind2OpCodes.IncCheck, new IncCheck(machine));
            Add(Kind2OpCodes.Jin, new Jin(machine));
            Add(Kind2OpCodes.Test, new Test(machine));
            Add(Kind2OpCodes.Or, new Or(machine));
            Add(Kind2OpCodes.And, new And(machine));
            Add(Kind2OpCodes.TestAttribute, new TestAttribute(machine));
            Add(Kind2OpCodes.SetAttribute, new SetAttribute(machine));
            Add(Kind2OpCodes.ClearAttribute, new ClearAttribute(machine));
            Add(Kind2OpCodes.Store, new Store(machine));
            Add(Kind2OpCodes.InsertObj, new InsertObj(machine));
            Add(Kind2OpCodes.LoadW, new LoadW(machine));
            Add(Kind2OpCodes.LoadB, new LoadB(machine));
            Add(Kind2OpCodes.GetProp, new GetProp(machine));
            Add(Kind2OpCodes.GetPropAddr, new GetPropAddr(machine));
            Add(Kind2OpCodes.GetNextProp, new GetNextProp(machine));
            Add(Kind2OpCodes.Add, new Add(machine));
            Add(Kind2OpCodes.Sub, new Sub(machine));
            Add(Kind2OpCodes.Mul, new Mul(machine));
            Add(Kind2OpCodes.Div, new Div(machine));
            Add(Kind2OpCodes.Mod, new Mod(machine));
            Add(Kind2OpCodes.Call2S, new Call2S(machine));
            Add(Kind2OpCodes.Call2N, new Call2N(machine));
            Add(Kind2OpCodes.SetColor, new SetColor(machine, io));
        }
    }
}