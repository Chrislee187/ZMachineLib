#define SIMPLE_IO
using System;
using System.Diagnostics;
using ZMachineLib;
using ZMachineLib.Operations.KindExt;

namespace ZPlay
{
    public class UserIo : IUserIo
	{
		private int _lines;
		private readonly ConsoleColor _defaultFore;
		private readonly ConsoleColor _defaultBack;

        public UserIo()
		{
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
			Console.SetCursorPosition(0, Console.WindowHeight-1);
            _defaultFore = ConsoleColor.White; //Console.ForegroundColor;
            _defaultBack = ConsoleColor.Black; //Console.BackgroundColor;
		}

		public void Print(string s)
		{
			for(var i = 0; i < s.Length; i++)
			{
				if(s[i] == ' ')
				{
					var next = s.IndexOf(' ', i+1);
					if(next == -1)
						next = s.Length;
					if(next >= 0)
					{
						if(Console.CursorLeft + (next - i) >= Console.WindowWidth)
						{
							Console.MoveBufferArea(0, 0, Console.WindowWidth, _lines, 0, 1);
							Console.WriteLine("");

							i++;
						}
					}
				}

				if(i < s.Length && s[i] == Environment.NewLine[0])
					Console.MoveBufferArea(0, 0, Console.WindowWidth, _lines, 0, 1);

				if(i < s.Length)
					Console.Write(s[i]);
			}
		}

		public string Read(int max)
		{
#if SIMPLE_IO
			var s = Console.ReadLine();
			Console.MoveBufferArea(0, 0, Console.WindowWidth, _lines, 0, 1);
			return s?.Substring(0, Math.Min(s.Length, max));
#else
			string s = string.Empty;
			ConsoleKeyInfo key = new ConsoleKeyInfo();

			do
			{
				if(Console.KeyAvailable)
				{
					key = Console.ReadKey(true);
					switch(key.Key)
					{
						case ConsoleKey.Backspace:
							if(s.Length > 0)
							{
								s = s.Remove(s.Length-1, 1);
								Console.Write(key.KeyChar);
							}
							break;
						case ConsoleKey.Enter:
							break;
						default:
							s += key.KeyChar;
							Console.Write(key.KeyChar);
							break;
					}
				}
			}
			while(key.Key != ConsoleKey.Enter);

			Console.MoveBufferArea(0, 0, Console.WindowWidth, _lines, 0, 1);
			Console.WriteLine(string.Empty);
			return s;
#endif
		}

		public char ReadChar()
		{
			return Console.ReadKey(true).KeyChar;
		}

		public void SetCursor(ushort line, ushort column, ushort window)
		{
			Console.SetCursorPosition(column-1, line-1);
		}

		public void SetWindow(ushort window)
		{
			if(window == 0)
				Console.SetCursorPosition(0, Console.WindowHeight-1);
		}

		public void EraseWindow(ushort window)
		{
			var c = Console.BackgroundColor;
			Console.BackgroundColor = _defaultBack;
			Console.Clear();
			Console.BackgroundColor = c;
			Console.ForegroundColor = _defaultFore;
		}

		public void BufferMode(bool buffer)
		{
            Log("BufferMode not implemented");
		}

		public void SplitWindow(ushort lines)
		{
			_lines = lines;
		}

		public void ShowStatus()
		{
            Log("ShowStatus not implemented");
		}

        public void SetTextStyle(TextStyle textStyle)
		{
			switch(textStyle)
			{
				case TextStyle.Roman:
					Console.ResetColor();
					break;
				case TextStyle.Reverse:
					var temp = Console.BackgroundColor;
					Console.BackgroundColor = Console.ForegroundColor;
					Console.ForegroundColor = temp;
					break;
				case TextStyle.Bold:
                    Console.ForegroundColor = ConsoleColor.Blue;
					break;
				case TextStyle.Italic:
                    Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case TextStyle.FixedPitch:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(textStyle), textStyle, null);
			}
		}

		public void SetColor(ZColor foreground, ZColor background)
		{
			Console.ForegroundColor = ZColorToConsoleColor(foreground, true);
			Console.BackgroundColor = ZColorToConsoleColor(background, false);
		}

		public void SoundEffect(ushort number)
		{
			if(number == 1)
				Console.Beep(2000, 300);
			else if(number == 2)
				Console.Beep(250, 300);
			else
                Log("SoundEffect number > 2");
        }

		public void Quit()
		{}

		private ConsoleColor ZColorToConsoleColor(ZColor c, bool fore)
		{
			switch(c)
			{
				case ZColor.PixelUnderCursor:
				case ZColor.Current:
					return fore ? Console.ForegroundColor : Console.BackgroundColor;
				case ZColor.Default:
					return fore ? _defaultFore : _defaultBack;
				case ZColor.Black:
					return ConsoleColor.Black;
				case ZColor.Red:
					return ConsoleColor.Red;
				case ZColor.Green:
					return ConsoleColor.Green;
				case ZColor.Yellow:
					return ConsoleColor.Yellow;
				case ZColor.Blue:
					return ConsoleColor.Blue;
				case ZColor.Magenta:
					return ConsoleColor.Magenta;
				case ZColor.Cyan:
					return ConsoleColor.Cyan;
				case ZColor.White:
					return ConsoleColor.White;
                case ZColor.LightGrey:
                    return ConsoleColor.Gray;
                case ZColor.MediumGrey:
                    return ConsoleColor.Gray;
                case ZColor.DarkishGrey:
					return ConsoleColor.DarkGray;
                case ZColor.DarkGrey:
					return ConsoleColor.DarkGray;
			}
			return fore ? _defaultFore : _defaultBack;
		}

        public void Log(string text)
        {
            // TODO: Hook up Core logging
            Debug.Print(text);
        }
    }
}
