using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    /// <summary>
    /// VAR:231 7 random range -> (result)<br/>
    /// If range is positive, returns a uniformly random number between 1 and range.If range is negative,
    /// the random number generator is seeded to that value and the return value is 0.
    /// Most interpreters consider giving 0 as range illegal (because they attempt a division with
    /// remainder by the range), but correct behaviour is to reseed the generator in as random a way
    /// as the interpreter can(e.g.by using the time in milliseconds).
    ///  </summary>
    /// <remarks>
    /// (Some version 3 games, such as 'Enchanter' release 29, had a debugging verb #random such
    /// that typing, say, #random 14 caused a call of random with -14.)
    /// </remarks>
    /// <remarks>
    /// ZORK I (Release 119 / Serial number 880429) has a '#rand' command that can be used with numbers less than or equal to 0 which will
    /// reseed the Random number generator to the absolute value (i.e. -seed).
    /// NB. The parser doesn't accept negative numbers so negative numbers
    /// must be supplied as there unsigned value (i.e. cast to 'short') for example; 65535 == (short) -1, 65534 == (short) -2
    /// </remarks>
    public sealed class Random : ZMachineOperationBase
    {
        private static System.Random _random = new System.Random();
        public Random(IZMemory memory)
            : base((ushort)OpCodes.Random, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            ushort val = 0;

            short seed = (short)args[0];
            if (seed <= 0)
                _random = new System.Random(seed); // Random automatically performs ABS() on the seed
            else
                val = (ushort)(_random.Next(0, args[0]) + 1);

            var dest = Memory.GetCurrentByteAndInc();
            Memory.VariableManager.Store(dest, val);
        }
    }
}