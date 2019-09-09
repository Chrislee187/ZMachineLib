using System.Collections.Generic;
using ZMachineLib.Operations.Kind1;

namespace ZMachineLib.Operations.Kind0
{
    public class Kind0Operations : Dictionary<Kind0OpCodes, IOperation>
    {
        public RTrue RTrue { get; }
        public RFalse RFalse { get; }
        public Save Save { get; }
        public Restore Restore { get; }
        public Kind0Operations(ZMachine2 machine,
            IZMachineIO io)
        {
            RTrue = new RTrue(machine);
            RFalse = new RFalse(machine);
            Save = new Save(machine, io, RTrue, RFalse);
            Restore = new Restore(machine, io);

            Add(Kind0OpCodes.RTrue, RTrue);
            Add(Kind0OpCodes.RFalse, RFalse);
            Add(Kind0OpCodes.Print, new Print(machine, io));
            Add(Kind0OpCodes.PrintRet, new PrintRet(machine, io, RTrue));
            Add(Kind0OpCodes.Nop, new Nop());
            Add(Kind0OpCodes.Save, Save);
            Add(Kind0OpCodes.Restore, Restore);
            Add(Kind0OpCodes.Restart, new Nop(Kind0OpCodes.Restart));
            Add(Kind0OpCodes.RetPopped, new RetPopped(machine));
            Add(Kind0OpCodes.Pop, new Pop(machine));
            Add(Kind0OpCodes.Quit, new Nop(Kind0OpCodes.Quit));
            Add(Kind0OpCodes.NewLine, new Newline(machine, io));;
            Add(Kind0OpCodes.ShowStatus, new ShowStatus(machine, io));;
            Add(Kind0OpCodes.Verify, new Verify(machine));;
            Add(Kind0OpCodes.Piracy, new Piracy(machine));;
        }
    }

    public class Kind1Operations : Dictionary<Kind1OpCodes, IOperation>
    {
//        public RTrue RTrue { get; }
//        public RFalse RFalse { get; }
//        public Save Save { get; }
//        public Restore Restore { get; }
        public Kind1Operations(ZMachine2 machine,
            IZMachineIO io)
        {
//            RTrue = new RTrue(machine);
//            RFalse = new RFalse(machine);
//            Save = new Save(machine, io, RTrue, RFalse);
//            Restore = new Restore(machine, io);

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