using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class SetColor : ZMachineOperationBase
    {
        private IUserIo _io;

        public SetColor(IZMemory contents,
            IUserIo io)
            : base((ushort)OpCodes.SetColor, contents)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            _io.SetColor((ZColor)operands[0], (ZColor)operands[1]);

        }
    }
}