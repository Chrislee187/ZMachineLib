using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class Push : ZMachineOperation
    {
        public Push(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.Push, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Stack.Peek().RoutineStack.Push(args[0]);
        }
    }

    public sealed class CopyTable : ZMachineOperation
    {
        public CopyTable(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.CopyTable, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (args[1] == 0)
            {
                for (var i = 0; i < args[2]; i++)
                    Memory[args[0] + i] = 0;
            }
            else if ((short)args[1] < 0)
            {
                for (var i = 0; i < Math.Abs(args[2]); i++)
                    Memory[args[1] + i] = Memory[args[0] + i];
            }
            else
            {
                for (var i = Math.Abs(args[2]) - 1; i >= 0; i--)
                    Memory[args[1] + i] = Memory[args[0] + i];
            }
        }
    }

    public sealed class PrintTable : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public PrintTable(ZMachine2 machine, IZMachineIo io)
            : base((ushort)KindVarOpCodes.PrintTable, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            // TODO: print properly

            var s = ZsciiString.GetZsciiString(args[0]);
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }

    public sealed class CheckArgCount : ZMachineOperation
    {
        public CheckArgCount(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.CheckArgCount, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Jump(args[0] <= Stack.Peek().ArgumentCount);
        }
    }
}