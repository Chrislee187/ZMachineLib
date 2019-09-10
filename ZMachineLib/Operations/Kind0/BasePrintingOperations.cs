namespace ZMachineLib.Operations.Kind0
{
    public abstract class BasePrintingOperations : ZMachineOperation
    {
        protected readonly IZMachineIo Io;

        protected BasePrintingOperations(ushort code,
            ZMachine2 machine,
            IZMachineIo io) 
            : base(code, machine)
        {
            Io = io;
        }
    }
}