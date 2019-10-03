using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:23 17 div a b -> (result)
    /// Signed 16-bit division.
    /// Division by zero should halt the interpreter with a suitable error message.
    /// </summary>
    public sealed class Div : ZMachineOperationBase
    {
        public Div(IZMemory memory)
            : base((ushort)OpCodes.Div, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory.GetCurrentByteAndInc();

            if (args[1] == 0)
            {
                // TODO: Log
                return;
            }

            ushort result = (ushort)((short)args[0] / (short)args[1]);

            OpLogging.Op2WithStore(GetType().Name.ToUpper(), args[0], args[1], result, dest);

            Memory.VariableManager.Store(dest, result);
        }
    }
}