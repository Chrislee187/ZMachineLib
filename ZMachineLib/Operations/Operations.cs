using System.Collections;
using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Operations.OP0;
using ZMachineLib.Operations.OP1;
using ZMachineLib.Operations.OP2;
using ZMachineLib.Operations.OPVAR;
using Not = ZMachineLib.Operations.OP1.Not;

namespace ZMachineLib.Operations
{
    public class Operations : IReadOnlyDictionary<OpCodes, IOperation>
    {
        private readonly IDictionary<OpCodes, IOperation> _operations = new Dictionary<OpCodes, IOperation>();

        public Operations(IUserIo io, IFileIo fileIo, IZMemory memory)
        {
            // OP2
            _operations.Add(OpCodes.Je, new Je(memory));
            _operations.Add(OpCodes.Jl, new Jl(memory));
            _operations.Add(OpCodes.Jg, new Jg(memory));
            _operations.Add(OpCodes.DecCheck, new DecCheck(memory));
            _operations.Add(OpCodes.IncCheck, new IncCheck(memory));
            _operations.Add(OpCodes.Jin, new Jin(memory));
            _operations.Add(OpCodes.Test, new Test(memory));
            _operations.Add(OpCodes.Or, new Or(memory));
            _operations.Add(OpCodes.And, new And(memory));
            _operations.Add(OpCodes.TestAttribute, new TestAttribute(memory));
            _operations.Add(OpCodes.SetAttribute, new SetAttribute(memory));
            _operations.Add(OpCodes.ClearAttribute, new ClearAttribute(memory));
            _operations.Add(OpCodes.Store, new Store(memory));
            _operations.Add(OpCodes.InsertObj, new InsertObj(memory));
            _operations.Add(OpCodes.LoadW, new LoadW(memory));
            _operations.Add(OpCodes.LoadB, new LoadB(memory));
            _operations.Add(OpCodes.GetProp, new GetProp(memory));
            _operations.Add(OpCodes.GetPropAddr, new GetPropAddr(memory));
            _operations.Add(OpCodes.GetNextProp, new GetNextProp(memory));
            _operations.Add(OpCodes.Add, new Add(memory));
            _operations.Add(OpCodes.Sub, new Sub(memory));
            _operations.Add(OpCodes.Mul, new Mul(memory));
            _operations.Add(OpCodes.Div, new Div(memory));
            _operations.Add(OpCodes.Mod, new Mod(memory));
            _operations.Add(OpCodes.Call2S, new Call2S(memory));
            _operations.Add(OpCodes.Call2N, new Call2N(memory));
            _operations.Add(OpCodes.SetColor, new SetColor(memory, io));

            // OP1
            _operations.Add(OpCodes.Jz, new Jz(memory));
            _operations.Add(OpCodes.GetSibling, new GetSibling(memory));
            _operations.Add(OpCodes.GetChild, new GetChild(memory));
            _operations.Add(OpCodes.GetParent, new GetParent(memory));
            _operations.Add(OpCodes.GetPropLen, new GetPropLen(memory));
            _operations.Add(OpCodes.Inc, new Inc(memory));
            _operations.Add(OpCodes.Dec, new Dec(memory));
            _operations.Add(OpCodes.PrintAddr, new PrintAddr(memory, io));
            _operations.Add(OpCodes.Call1S, new Call1S(memory));
            _operations.Add(OpCodes.RemoveObj, new RemoveObj(memory));
            _operations.Add(OpCodes.PrintObj, new PrintObj(memory, io));
            _operations.Add(OpCodes.Ret, new Ret(memory));
            _operations.Add(OpCodes.Jump, new Jump(memory));
            _operations.Add(OpCodes.PrintPAddr, new PrintPAddr(memory, io));
            _operations.Add(OpCodes.Load, new Load(memory));

            if (memory.Header.Version <= 4)
            {
                _operations.Add(OpCodes.Not, new Not(memory));
            }
            else
            {
                _operations.Add(OpCodes.Call1N, new Call1N(memory));
            }

            // OP0
            _operations.Add(OpCodes.RTrue, new RTrue(memory));
            _operations.Add(OpCodes.RFalse, new RFalse(memory));
            _operations.Add(OpCodes.Print, new Print(memory, io));
            _operations.Add(OpCodes.PrintRet, new PrintRet(memory, io, (RTrue)_operations[OpCodes.RTrue]));
            _operations.Add(OpCodes.Nop, new Nop(memory));
            _operations.Add(OpCodes.Save, new Save(memory, fileIo)); 
            _operations.Add(OpCodes.Restore, new Restore(memory, fileIo));
            _operations.Add(OpCodes.Restart, new Restart(memory));
            _operations.Add(OpCodes.RetPopped, new RetPopped(memory));
            _operations.Add(OpCodes.Pop, new Pop(memory));
            _operations.Add(OpCodes.Quit, new Quit(memory, io));
            _operations.Add(OpCodes.NewLine, new Newline(memory, io));
            _operations.Add(OpCodes.ShowStatus, new ShowStatus(memory, io));
            _operations.Add(OpCodes.Verify, new Verify(memory));
            _operations.Add(OpCodes.Piracy, new Piracy(memory));

            // OPVAR
            _operations.Add(OpCodes.Call, new Call(memory));
            _operations.Add(OpCodes.StoreB, new StoreB(memory));
            _operations.Add(OpCodes.StoreW, new StoreW(memory));
            _operations.Add(OpCodes.PutProp, new PutProp(memory)); 
            _operations.Add(OpCodes.Read, new Read(memory, io));
            _operations.Add(OpCodes.PrintChar, new PrintChar(memory, io));
            _operations.Add(OpCodes.PrintNum, new PrintNum(memory, io));
            _operations.Add(OpCodes.Random, new Random(memory));
            _operations.Add(OpCodes.Push, new Push(memory));
            _operations.Add(OpCodes.Pull, new Pull(memory));
            _operations.Add(OpCodes.SplitWindow, new SplitWindow(memory, io));
            _operations.Add(OpCodes.SetWindow, new SetWindow(memory, io));
            _operations.Add(OpCodes.CallVs2, new CallVs2(memory));
            _operations.Add(OpCodes.EraseWindow, new EraseWindow(memory, io));
            _operations.Add(OpCodes.SetCursor, new SetCursor(memory, io));
            _operations.Add(OpCodes.SetTextStyle, new SetTextStyle(memory, io));
            _operations.Add(OpCodes.BufferMode, new BufferMode(memory, io));
            _operations.Add(OpCodes.OutputStream, new OutputStream(memory));
            _operations.Add(OpCodes.SoundEffect, new SoundEffect(memory, io));
            _operations.Add(OpCodes.ReadChar, new ReadChar(memory, io));
            _operations.Add(OpCodes.ScanTable, new ScanTable(memory));
            _operations.Add(OpCodes.NotVar, new Not(memory));
            _operations.Add(OpCodes.CallVn, new CallVn(memory));
            _operations.Add(OpCodes.CallVn2, new CallVn2(memory));
            _operations.Add(OpCodes.CopyTable, new CopyTable(memory));
            _operations.Add(OpCodes.PrintTable, new PrintTable(memory, io));
            _operations.Add(OpCodes.CheckArgCount, new CheckArgCount(memory));
        }

        #region IReadOnlyDictionary<>
        public IEnumerator<KeyValuePair<OpCodes, IOperation>> GetEnumerator() => _operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_operations).GetEnumerator();

        public int Count => _operations.Count;

        public bool ContainsKey(OpCodes key) => _operations.ContainsKey(key);

        public bool TryGetValue(OpCodes key, out IOperation value) => _operations.TryGetValue(key, out value);

        public IOperation this[OpCodes key] => _operations[key];

        public IEnumerable<OpCodes> Keys => _operations.Keys;

        public IEnumerable<IOperation> Values => _operations.Values;
        #endregion
    }
}