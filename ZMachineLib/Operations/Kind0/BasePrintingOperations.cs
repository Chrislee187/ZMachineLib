namespace ZMachineLib.Operations.Kind0
{
    public abstract class BasePrintingOperations : ZMachineOperation
    {
        protected readonly IZMachineIO Io;

        protected readonly ZsciiString ZsciiString;
        protected BasePrintingOperations(Kind0OpCodes code,
            ZMachine2 machine,
            IZMachineIO io) 
            : base(code, machine)
        {
            Io = io;
            ZsciiString = new ZsciiString(machine);
        }
    }
}