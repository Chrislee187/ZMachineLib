﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:129 1 get_sibling object -> (result) ?(label)
    /// Get next object in tree, branching if this exists, i.e. is not 0.
    /// </summary>
    public sealed class GetSibling : ZMachineOperationBase
    {
        public GetSibling(IZMemory contents)
            : base((ushort)OpCodes.GetSibling, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var zObj = Contents.ObjectTree.GetOrDefault(obj).RefreshFromMemory();
            var storageType = Contents.GetCurrentByteAndInc();

            if (Contents.Header.Version <= 3)
            {
                Contents.VariableManager.Store(storageType, (byte)zObj.Sibling);
            }
            else
                Contents.VariableManager.Store(storageType, zObj.Sibling);

            Contents.Jump(zObj.Sibling != 0);
        }
    }
}