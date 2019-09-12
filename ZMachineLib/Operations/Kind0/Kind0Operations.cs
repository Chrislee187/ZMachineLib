﻿using System.Collections;
using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public class Kind0Operations : IReadOnlyDictionary<Kind0OpCodes, IOperation>
    {
        private readonly IDictionary<Kind0OpCodes, IOperation> _operations = new Dictionary<Kind0OpCodes, IOperation>();

        public Kind0Operations(ZMachine2 machine,
            IZMachineIo io,
            IFileIo fileIo)
        {
            _operations.Add(Kind0OpCodes.RTrue, new RTrue(machine));
            _operations.Add(Kind0OpCodes.RFalse, new RFalse(machine));
            _operations.Add(Kind0OpCodes.Print, new Print(machine, io));
            _operations.Add(Kind0OpCodes.PrintRet, new PrintRet(machine, io, (RTrue) _operations[Kind0OpCodes.RTrue]));
            _operations.Add(Kind0OpCodes.Nop, new Nop());
            _operations.Add(Kind0OpCodes.Save, new Save(machine, fileIo));
            _operations.Add(Kind0OpCodes.Restore, new Restore(machine, fileIo));
            _operations.Add(Kind0OpCodes.Restart, new Restart(machine));
            _operations.Add(Kind0OpCodes.RetPopped, new RetPopped(machine));
            _operations.Add(Kind0OpCodes.Pop, new Pop(machine));
            _operations.Add(Kind0OpCodes.Quit, new Quit(machine, io));
            _operations.Add(Kind0OpCodes.NewLine, new Newline(machine, io));
            _operations.Add(Kind0OpCodes.ShowStatus, new ShowStatus(machine, io));
            _operations.Add(Kind0OpCodes.Verify, new Verify(machine));
            _operations.Add(Kind0OpCodes.Piracy, new Piracy(machine));
        }

        #region IReadOnlyDictionary<>

        public IEnumerator<KeyValuePair<Kind0OpCodes, IOperation>> GetEnumerator() => _operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_operations).GetEnumerator();

        public int Count => _operations.Count;

        public bool ContainsKey(Kind0OpCodes key) => _operations.ContainsKey(key);

        public bool TryGetValue(Kind0OpCodes key, out IOperation value) => _operations.TryGetValue(key, out value);

        public IOperation this[Kind0OpCodes key] => _operations[key];

        public IEnumerable<Kind0OpCodes> Keys => _operations.Keys;

        public IEnumerable<IOperation> Values => _operations.Values; 
        #endregion
    }
}