﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Je : ZMachineOperation
    {
        public Je(ZMachine2 machine)
            : base((ushort)OpCodes.Je, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var equal = false;
            for (var i = 1; i < args.Count; i++)
            {
                if (args[0] == args[i])
                {
                    equal = true;
                    break;
                }
            }

            Jump(equal);
        }
    }
}