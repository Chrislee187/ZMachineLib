namespace ZMachineLib.Operations.Kind0
{
    public abstract class BasePrintingOperations : ZMachineOperation
    {
        protected readonly IZMachineIO Io;

        protected readonly ZsciiString ZsciiString;
        protected BasePrintingOperations(ushort code,
            ZMachine2 machine,
            IZMachineIO io) 
            : base((ushort) code, machine)
        {
            Io = io;
            ZsciiString = new ZsciiString(machine);
        }
    }
}