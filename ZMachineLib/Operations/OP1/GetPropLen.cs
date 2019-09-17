﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class GetPropLen : ZMachineOperation
    {
        public GetPropLen(ZMachine2 machine)
            : base((ushort)OpCodes.GetPropLen, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Machine.Memory[Machine.Stack.Peek().PC++];
            var propInfo = Machine.Memory[args[0] - 1];
            byte len;
            if (Machine.Header.Version > 3 && (propInfo & 0x80) == 0x80)
            {
                len = (byte)(Machine.Memory[args[0] - 1] & 0x3f);
                if (len == 0)
                    len = 64;
            }
            else
                len = (byte)((propInfo >> ((ushort) Machine.Header.Version <= 3 ? 5 : 6)) + 1);

            VariableManager.StoreByte(dest, len);
        }
    }
}