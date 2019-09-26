using ZMachineLib.Content;
using ZMachineLib.Operations.OPExtended;

namespace ZMachineLib.Operations
{
    public class ZOperations
    {
        private readonly KindExtOperations _extendedOperations;
        private readonly ZMachineLib.Operations.Operations _operations;
        private readonly IZMemory _zMemory;

        public ZOperations(IUserIo userIo, IFileIo fileIo, IZMemory zMemory)
        {
            _zMemory = zMemory;
            _operations = new ZMachineLib.Operations.Operations(userIo, fileIo, _zMemory);
            _extendedOperations = new KindExtOperations(_operations, _zMemory);
        }

        public (byte opCode, OpCodes opCodeEnum, IOperation operation) GetOperation(byte opCode)
        {
            //NOTE: http://inform-fiction.org/zmachine/standards/z1point1/sect14.html
            IOperation operation;
            OpCodes opCodeEnum;
            if (opCode == (byte)OpCodes.Extended) // 0OP:190 - special op, indicates next byte contains Extended Op
            {
                opCodeEnum = OpCodes.Extended;
                opCode = _zMemory.GetCurrentByteAndInc();
                _extendedOperations.TryGetValue((KindExtOpCodes)(opCode & 0x1f), out operation);
                // TODO: hack to make this a VAR opcode...
                opCode |= 0xc0;

                Log.Write($" Ext ");
            }
            else
            {
                opCodeEnum = opCode.ToOpCode();

                _operations.TryGetValue(opCodeEnum, out operation);
            }

            return (opCode, opCodeEnum, operation);
        }

    }
}