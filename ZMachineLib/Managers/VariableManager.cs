using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Managers
{
    public interface IVariableManager
    {
        ushort GetUShort(byte variable, bool andRemove = true);
        void StoreUShort(byte dest, ushort value, bool newEntry = true);
        void StoreByte(byte dest, byte value);
    }

    public class VariableManager : IVariableManager
    {
        private readonly Stack<ZStackFrame> _stack;
        private readonly ZGlobals _globals;

        public VariableManager(Stack<ZStackFrame> stack, ZGlobals globals)
        {
            _globals = globals;
            _stack = stack;
        }
        public ushort GetUShort(byte variable, bool andRemove = true)
        {
            ushort val;

            if (DestinationIsStack(variable))
            {
                val = GetUShortFromStack(andRemove);
            }
            else if (DestinationIsVariable(variable))
            {
                val = GetUShortFromVariables(variable);
            }
            else
            {
                val = GetUShortFromGlobals(variable);
            }

            return val;
        }

        private ushort GetUShortFromGlobals(byte variable)
        {
            var globalsNumber = variable - 0x10;
            var val = _globals.Get((byte) globalsNumber); 
            return val;
        }

        private ushort GetUShortFromVariables(byte variable)
        {
            var val = _stack.Peek().Variables[variable - 1];
            Log.Write($"L{variable - 1:X2} ({val:X4}), ");
            return val;
        }

        private ushort GetUShortFromStack(bool andRemove)
        {
            ushort val;
            val = andRemove 
                ? _stack.Peek().RoutineStack.Pop() 
                : _stack.Peek().RoutineStack.Peek();

            Log.Write($"SP ({val:X4}), ");
            return val;
        }
        
        public void StoreUShort(byte dest, ushort value, bool newEntry = true)
        {
            if (DestinationIsStack(dest))
            {
                StoreUShortOnStack(value, newEntry);
            }
            else if (DestinationIsVariable(dest))
            {
                StoreUShortInVariable(dest, value);
            }
            else
            {
                StoreUShortInGlobal(dest, value);
            }
        }

        private void StoreUShortInGlobal(byte dest, ushort value)
        {
            Log.Write($"-> GLB{ZGlobals.GetGlobalsNumber(dest):X2} ({value:X4}), ");

            _globals.Set(ZGlobals.GetGlobalsNumber(dest), value);
        }

        private void StoreUShortInVariable(byte dest, ushort value)
        {
            var variablesIdx = dest - 1;
            Log.Write($"-> VAR{variablesIdx:X2} ({value:X4}), ");
            _stack.Peek().Variables[variablesIdx] = value;
        }

        private void StoreUShortOnStack(ushort value, bool newEntry)
        {
            if (!newEntry)
            {
                Log.Write($"-> STK POP BEFORE... ");
                _stack.Peek().RoutineStack.Pop();
            }
            Log.Write($"-> STK PUSH({value:X4}), ");
            _stack.Peek().RoutineStack.Push(value);
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
            var globalsIdx = ZGlobals.GetGlobalsNumber(dest);
            Log.Write($"-> GLB{globalsIdx:X2} = ({value:X2}), ");
            _globals.Set(globalsIdx, value);
        }

        private void StoreByteInVariable(byte dest, byte value)
        {
            var variableIdx = dest - 1;
            Log.Write($"-> VAR{variableIdx:X2} = ({value:X2}), ");
            _stack.Peek().Variables[variableIdx] = value;
        }

        private void StoreByteOnStack(byte value)
        {
            Log.Write($"-> STK PUSH({value:X2})");
            _stack.Peek().RoutineStack.Push(value);
        }

        private bool DestinationIsStack(byte dest) => dest == 0;

        private bool DestinationIsVariable(byte dest) => dest < 0x10;
    }

}