using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib
{
    public class VarHandler
    {
        private byte[] Memory => _machine.Memory;
        private ushort GlobalsTable => _machine.Header.Globals;
        protected Stack<ZStackFrame> Stack => _machine.Stack;
        private readonly ZMachine2 _machine;

        public VarHandler(ZMachine2 machine)
        {
            _machine = machine;
        }
        public ushort GetWord(byte variable, bool pop = true)
        {
            ushort val;

            if (DestinationIsStack(variable))
            {
                val = GetWordFromStack(pop);
            }
            else if (DestinationIsVariable(variable))
            {
                val = GetWordFromVariables(variable);
            }
            else
            {
                val = GetWordFromGlobals(variable);
            }

            return val;
        }

        private ushort GetWordFromGlobals(byte variable)
        {
            ushort val;
            val = Memory.GetUshort((ushort)(GlobalsTable + 2 * (variable - 0x10)));
            Log.Write($"G{variable - 0x10:X2} ({val:X4}), ");
            return val;
        }

        private ushort GetWordFromVariables(byte variable)
        {
            ushort val;
            val = Stack.Peek().Variables[variable - 1];
            Log.Write($"L{variable - 1:X2} ({val:X4}), ");
            return val;
        }

        private ushort GetWordFromStack(bool pop)
        {
            ushort val;
            if (pop)
                val = Stack.Peek().RoutineStack.Pop();
            else
                val = Stack.Peek().RoutineStack.Peek();
            Log.Write($"SP ({val:X4}), ");
            return val;
        }


        public void StoreWord(byte dest, ushort value, bool push = true)
        {
            if (DestinationIsStack(dest))
            {
                StoreWordOnStack(value, push);
            }
            else if (DestinationIsVariable(dest))
            {
                StoreWordInVariable(dest, value);
            }
            else
            {
                StoreWordInGlobal(dest, value);
            }
        }

        private void StoreWordInGlobal(byte dest, ushort value)
        {
            var globalsIdx = dest - 0x10;
            Log.Write($"-> GLB{globalsIdx:X2} ({value:X4}), ");
            Memory.StoreAt((ushort)(GlobalsTable + 2 * globalsIdx), value);
        }

        private void StoreWordInVariable(byte dest, ushort value)
        {
            var variablesIdx = dest - 1;
            Log.Write($"-> VAR{variablesIdx:X2} ({value:X4}), ");
            Stack.Peek().Variables[variablesIdx] = value;
        }

        private void StoreWordOnStack(ushort value, bool replaceLastEntry)
        {
            if (!replaceLastEntry)
            {
                Log.Write($"-> STK POP BEFORE... ");
                Stack.Peek().RoutineStack.Pop();
            }
            Log.Write($"-> STK PUSH({value:X4}), ");
            Stack.Peek().RoutineStack.Push(value);
        }

        public void StoreByte(byte dest, byte value)
        {
            if (DestinationIsStack(dest))
            {
                StoreByteOnStack(value);
            }
            else if (DestinationIsVariable(dest))
            {
                StoreByteInVariable(dest, value);
            }
            else
            {
                StoreByteInGlobals(dest, value);
            }
        }

        private void StoreByteInGlobals(byte dest, byte value)
        {
            var globalsIdx = dest - 0x10;
            Log.Write($"-> GLB{globalsIdx:X2} = ({value:X2}), ");
            var addr = GlobalsTable + 2 * globalsIdx;
            // this still gets written as a word...write the byte to addr+1
            Memory.StoreAt(addr, 0, value);
        }

        private void StoreByteInVariable(byte dest, byte value)
        {
            var variableIdx = dest - 1;
            Log.Write($"-> VAR{variableIdx:X2} = ({value:X2}), ");
            Stack.Peek().Variables[variableIdx] = value;
        }

        private void StoreByteOnStack(byte value)
        {
            Log.Write($"-> STK PUSH({value:X2})");
            Stack.Peek().RoutineStack.Push(value);
        }

        private bool DestinationIsStack(byte dest) => dest == 0;

        private bool DestinationIsVariable(byte dest) => dest < 0x10;
    }
}