using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public class Kind0Operations : Dictionary<Kind0OpCodes, IOperation>
    {
        public RTrue RTrue { get; }
        public RFalse RFalse { get; }
        public Save Save { get; }
        public Restore Restore { get; }
        public Kind0Operations(ZMachine2 machine,
            IZMachineIo io)
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
            Add(Kind0OpCodes.Restart, new Restart(machine));
            Add(Kind0OpCodes.RetPopped, new RetPopped(machine));
            Add(Kind0OpCodes.Pop, new Pop(machine));
            Add(Kind0OpCodes.Quit, new Nop(Kind0OpCodes.Quit));
            Add(Kind0OpCodes.NewLine, new Newline(machine, io));
            Add(Kind0OpCodes.ShowStatus, new ShowStatus(machine, io));
            Add(Kind0OpCodes.Verify, new Verify(machine));
            Add(Kind0OpCodes.Piracy, new Piracy(machine));
        }
    }
}