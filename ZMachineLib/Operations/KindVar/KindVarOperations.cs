using System.Collections;
using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public class KindVarOperations : IReadOnlyDictionary<KindVarOpCodes, IOperation>
    {
        private readonly IDictionary<KindVarOpCodes, IOperation> _operations = new Dictionary<KindVarOpCodes, IOperation>();

        public KindVarOperations(ZMachine2 machine,
            IUserIo io)
        {
            _operations.Add(KindVarOpCodes.Call, new Call(machine));
            _operations.Add(KindVarOpCodes.StoreB, new StoreB(machine));
            _operations.Add(KindVarOpCodes.StoreW, new StoreW(machine));
            _operations.Add(KindVarOpCodes.PutProp, new PutProp(machine));
            _operations.Add(KindVarOpCodes.Read, new Read(machine, io));
            _operations.Add(KindVarOpCodes.PrintChar, new PrintChar(machine, io));
            _operations.Add(KindVarOpCodes.PrintNum, new PrintNum(machine, io));
            _operations.Add(KindVarOpCodes.Random, new Random(machine));
            _operations.Add(KindVarOpCodes.Push, new Push(machine));
            _operations.Add(KindVarOpCodes.Pull, new Pull(machine));
            _operations.Add(KindVarOpCodes.SplitWindow, new SplitWindow(machine, io));
            _operations.Add(KindVarOpCodes.SetWindow, new SetWindow(machine, io));
            _operations.Add(KindVarOpCodes.CallVs2, new CallVs2(machine));
            _operations.Add(KindVarOpCodes.EraseWindow, new EraseWindow(machine, io));
            _operations.Add(KindVarOpCodes.SetCursor, new SetCursor(machine, io));
            _operations.Add(KindVarOpCodes.SetTextStyle, new SetTextStyle(machine, io));
            _operations.Add(KindVarOpCodes.BufferMode, new BufferMode(machine, io));
            _operations.Add(KindVarOpCodes.OutputStream, new OutputStream(machine));
            _operations.Add(KindVarOpCodes.SoundEffect, new SoundEffect(machine, io));
            _operations.Add(KindVarOpCodes.ReadChar, new ReadChar(machine, io));
            _operations.Add(KindVarOpCodes.ScanTable, new ScanTable(machine));
            _operations.Add(KindVarOpCodes.Not, new Not(machine));
            _operations.Add(KindVarOpCodes.CallVn, new CallVn(machine));
            _operations.Add(KindVarOpCodes.CallVn2, new CallVn2(machine));
            _operations.Add(KindVarOpCodes.CopyTable, new CopyTable(machine));
            _operations.Add(KindVarOpCodes.PrintTable, new PrintTable(machine, io));
            _operations.Add(KindVarOpCodes.CheckArgCount, new CheckArgCount(machine));
        }

        #region IReadOnlyDictionary<>
        public IEnumerator<KeyValuePair<KindVarOpCodes, IOperation>> GetEnumerator() => _operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_operations).GetEnumerator();

        public int Count => _operations.Count;

        public bool ContainsKey(KindVarOpCodes key) => _operations.ContainsKey(key);

        public bool TryGetValue(KindVarOpCodes key, out IOperation value) => _operations.TryGetValue(key, out value);

        public IOperation this[KindVarOpCodes key] => _operations[key];

        public IEnumerable<KindVarOpCodes> Keys => _operations.Keys;

        public IEnumerable<IOperation> Values => _operations.Values; 
        #endregion
    }
}