using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public abstract class BasePrintingOperationsBase : ZMachineOperationBase
    {
        protected readonly IUserIo Io;

        protected BasePrintingOperationsBase(ushort code,
            ZMachine2 machine,
            IZMemory memory,
            IUserIo io) 
            : base(code, memory)
        {
            Io = io;
        }
    }
}