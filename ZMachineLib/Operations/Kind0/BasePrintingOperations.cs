namespace ZMachineLib.Operations.Kind0
{
    public abstract class BasePrintingOperations : ZMachineOperation
    {
        protected readonly IUserIo Io;

        protected BasePrintingOperations(ushort code,
            ZMachine2 machine,
            IUserIo io) 
            : base(code, machine)
        {
            Io = io;
        }
    }
}