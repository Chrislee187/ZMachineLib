using System.Diagnostics;
using System.Text;

namespace ZMachineLib
{
	internal static class Log
    {
        public static bool Enabled = false;
		private static readonly StringBuilder _output = new StringBuilder();

		public static void Write(string s)
		{
            if(Enabled) _output.Append(s);
		}

		public static void WriteLine(string s)
		{
            if (Enabled) _output.AppendLine(s);
		}

		public static void Flush()
		{
            if (Enabled)
            {
                Print();
                _output.Clear();
            }
        }

		public static void Print()
		{
            if (Enabled) Debug.WriteLine(_output.ToString());
		}
	}
}
