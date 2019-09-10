using System.Diagnostics;
using System.Text;

namespace ZMachineLib
{
	internal static class Log
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static bool Enabled = false;
		private static readonly StringBuilder Output = new StringBuilder();

		public static void Write(string s)
		{
            if(Enabled) Output.Append(s);
		}

		public static void WriteLine(string s)
		{
            if (Enabled) Output.AppendLine(s);
		}

		public static void Flush()
		{
            if (Enabled)
            {
                Print();
                Output.Clear();
            }
        }

		public static void Print()
		{
            if (Enabled) Debug.WriteLine(Output.ToString());
		}
	}
}
