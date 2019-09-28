﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using ZMachineLib.Content;
using ZMachineLib.Extensions;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP0
{
    public class Save : GameSaveBase
    {
        public Save(IZMemory memory,
            IFileIo io) 
            : base(OpCodes.Save, memory, io)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var state = CreateState();

            try
            {
                var saveSuccessful = Io.Save(state);
                state.Dispose();
                if (Contents.Header.Version < 5)
                {
                    Contents.Jump(saveSuccessful);
                }
                else
                {
                    Contents.VariableManager.Store(Contents.GetCurrentByteAndInc(), saveSuccessful.ToOneOrZero());
                }
            }
            catch
            {
                // ignored: we don't want to crash the machine so ignore any IO errors
            }
        }


        private Stream CreateState()
        {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);

            // NOTE: We used to save two extra words here, addresses for Text and Parse tables but they
            // were always zero, the values are only used whilst reading user input so have been removed from
            // the save/restore code. There is a common save format available that maybe used them, needs checking

            bw.Write(((MemoryManager)(Contents.Manager)).Buffer, 0, Contents.Header.DynamicMemorySize - 1);

            var frames = ((Contents.Stack as Stack<ZStackFrame>) ?? throw new InvalidOperationException())
                .Select(f => f).ToArray();
            var dcs = new DataContractJsonSerializer(typeof(ZStackFrame[]));
            dcs.WriteObject(ms, frames);
            return ms;
        }
    }
}