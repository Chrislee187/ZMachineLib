using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Managers
{
    public class OperandManager : IOperandManager
    {
        /// <summary>
        /// http://inform-fiction.org/zmachine/standards/z1point1/sect04.html
        /// <see>Section 4.3</see>
        /// </summary>
        private enum OperationForm : byte
        {
            Variable = Bits.Bit6 | Bits.Bit7, // Section 4.3.3
            Short = Bits.Bit7 // Section 4.3.1
        }

        private readonly IZStack _stack;
        private readonly IVariableManager _variableManager;
        private readonly IMemoryManager _memoryManager;

        public OperandManager(IMemoryManager memoryManager,
            IZStack stack,
            IVariableManager variableManager)
        {
            _memoryManager = memoryManager;
            _variableManager = variableManager;
            _stack = stack;

        }

        /// <summary>
        /// http://inform-fiction.org/zmachine/standards/z1point1/sect04.html
        /// <see>Section 4.3, 4.4, 4.5</see>
        /// </summary>
        private enum OperandTypeMasks : byte
        {
            ShortFormShiftMask = Bits.Bit0 | Bits.Bit1,
            LongFormType1 = Bits.Bit6,
            LongFormType2 = Bits.Bit5,
        }

        public List<ushort> GetOperands(byte opcode)
        {
            var args = new List<ushort>();

            if (Bits.BitsSet(opcode, (byte) OperationForm.Variable))
            {
                // Section 4.4.3
                var types = _memoryManager.Get(_stack.GetPCAndInc());
                byte types2 = 0;

                if (opcode == 0xec || opcode == 0xfa)
                    types2 = _memoryManager.Get(_stack.GetPCAndInc());

                GetVariableOperands(types, args);
                if (opcode == 0xec || opcode == 0xfa)
                    GetVariableOperands(types2, args);
            }
            else if (Bits.BitsSet(opcode, (byte) OperationForm.Short))
            {
                // Section 4.4.1
                args.Add(GetOperand(ShortFormOperandType(opcode)));
            }
            else // Long Form
            {
                // Section 4.4.2
                args.Add(GetOperand(LongFormOperandType1(opcode)));
                args.Add(GetOperand(LongFormOperandType2(opcode)));
            }

            return args;
        }

        private void GetVariableOperands(byte types, List<ushort> args)
        {
            for (var i = 6; i >= 0; i -= 2)
            {
                var type = (byte) ((types >> i) & 0x03);

                // omitted
                if (type == 0x03)
                    break;

                var arg = GetOperand((OperandType) type);
                args.Add(arg);
            }
        }

        private ushort GetOperand(OperandType type)
        {
            ushort arg = 0;

            switch (type)
            {
                case OperandType.LargeConstant:
                    arg = _memoryManager.GetUShort((int) _stack.GetPC());
                    _stack.IncrementPC(2);
                    Log.Write($"#{arg:X4}, ");
                    break;
                case OperandType.SmallConstant:
                    arg = _memoryManager.Get(_stack.GetPCAndInc());
                    Log.Write($"#{arg:X2}, ");
                    break;
                case OperandType.Variable:
                    var b = _memoryManager.Get(_stack.GetPCAndInc());
                    arg = _variableManager.GetUShort(b);
                    break;
            }

            return arg;
        }

        private static OperandType ShortFormOperandType(byte opcode) // Section 4.4.1
            => (OperandType) (opcode >> 4 & (byte) OperandTypeMasks.ShortFormShiftMask);

        private static OperandType LongFormOperandType1(byte opcode) // Section 4.4.2
            => Bits.BitsSet(opcode, (byte) OperandTypeMasks.LongFormType1)
                ? OperandType.Variable
                : OperandType.SmallConstant;

        private static OperandType LongFormOperandType2(byte opcode) // Section 4.4.2
            => Bits.BitsSet(opcode, (byte) OperandTypeMasks.LongFormType2)
                ? OperandType.Variable
                : OperandType.SmallConstant;
    }

    /// <summary>
    /// http://inform-fiction.org/zmachine/standards/z1point1/sect04.html
    /// <see>Section 4.3</see>
    /// </summary>
    public enum OperationForm : byte
    {
        Variable = Bits.Bit6 | Bits.Bit7,
        Short = Bits.Bit7
    }

    /// <summary>
    /// http://inform-fiction.org/zmachine/standards/z1point1/sect04.html
    /// <see>Section 4.3</see>
    /// </summary>
    public enum OperandTypeMasks : byte
    {
        ShortFormShiftMask = Bits.Bit0 | Bits.Bit1,
        LongFormType1 = Bits.Bit6,
        LongFormType2 = Bits.Bit5,
    }
}
