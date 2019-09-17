﻿using System.Collections;
using System.Collections.Generic;

namespace ZMachineLib.Operations.OPExtended
{
    public class KindExtOperations : IReadOnlyDictionary<KindExtOpCodes, IOperation>
    {
        private readonly IDictionary<KindExtOpCodes, IOperation> _operations = new Dictionary<KindExtOpCodes, IOperation>();

        public KindExtOperations(ZMachine2 machine,
            Operations operations)
        {
            _operations.Add(KindExtOpCodes.Save, operations[OpCodes.Save]);
            _operations.Add(KindExtOpCodes.Restore, operations[OpCodes.Restore]);
            _operations.Add(KindExtOpCodes.LogShift, new LogShift(machine));
            _operations.Add(KindExtOpCodes.ArtShift, new ArtShift(machine));
            _operations.Add(KindExtOpCodes.SetFont, new SetFont(machine));

        }

        #region IReadOnlyDictionary<>
        public IEnumerator<KeyValuePair<KindExtOpCodes, IOperation>> GetEnumerator() => _operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_operations).GetEnumerator();

        public int Count => _operations.Count;

        public bool ContainsKey(KindExtOpCodes key) => _operations.ContainsKey(key);

        public bool TryGetValue(KindExtOpCodes key, out IOperation value) => _operations.TryGetValue(key, out value);

        public IOperation this[KindExtOpCodes key] => _operations[key];

        public IEnumerable<KindExtOpCodes> Keys => _operations.Keys;

        public IEnumerable<IOperation> Values => _operations.Values; 
        #endregion
    }
}