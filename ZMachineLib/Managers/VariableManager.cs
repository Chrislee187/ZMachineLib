using ZMachineLib.Extensions;

namespace ZMachineLib.Managers
{
    public interface IVariableManager
    {
        ushort GetWord(byte variable, bool andRemove = true);
        void StoreWord(byte dest, ushort value, bool newEntry = true);
        void StoreByte(byte dest, byte value);
    }

    public class VariableManager : ZMachineBase, IVariableManager
    {

        public VariableManager(ZMachine2 machine,
            IMemoryManager memoryManager) : base(machine, memoryManager)
        {

        }
        public ushort GetWord(byte variable, bool andRemove = true)
        {
            ushort val;

            if (DestinationIsStack(variable))
            {
                val = GetWordFromStack(andRemove);
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
            var val = Memory.GetUShort((ushort)(GlobalsTable + 2 * (variable - 0x10)));
            Log.Write($"G{variable - 0x10:X2} ({val:X4}), ");
            return val;
        }

        private ushort GetWordFromVariables(byte variable)
        {
            var val = Stack.Peek().Variables[variable - 1];
            Log.Write($"L{variable - 1:X2} ({val:X4}), ");
            return val;
        }

        private ushort GetWordFromStack(bool andRemove)
        {
            ushort val;
            val = andRemove 
                ? Stack.Peek().RoutineStack.Pop() 
                : Stack.Peek().RoutineStack.Peek();

            Log.Write($"SP ({val:X4}), ");
            return val;
        }
        
        public void StoreWord(byte dest, ushort value, bool newEntry = true)
        {
            if (DestinationIsStack(dest))
            {
                StoreWordOnStack(value, newEntry);
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

        private void StoreWordOnStack(ushort value, bool newEntry)
        {
            if (!newEntry)
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