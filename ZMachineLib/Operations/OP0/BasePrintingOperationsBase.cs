namespace ZMachineLib.Operations.OP0
{
    public abstract class BasePrintingOperationsBase : ZMachineOperationBase
    {
        protected readonly IUserIo Io;

        protected BasePrintingOperationsBase(ushort code,
            ZMachine2 machine,
            IUserIo io) 
            : base(code, machine)
        {
            Io = io;
        }
    }
}