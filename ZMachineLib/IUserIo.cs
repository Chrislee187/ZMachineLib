using System;
using ZMachineLib.Content;

namespace ZMachineLib
{
	[Flags]
	public enum TextStyle
	{
		Roman = 0,
		Reverse = 1,
		Bold = 2,
		Italic = 4,
		FixedPitch = 8
	}

	public interface IUserIo
	{
		void Print(string text);
		string Read(int max, IZMemory memory);
		char ReadChar();
		void SetCursor(ushort line, ushort column, ushort window);
		void SetWindow(ushort window);
		void EraseWindow(ushort window);
		void BufferMode(bool buffer);
		void SplitWindow(ushort lines);
		void ShowStatus(IZMemory memory);
		void SetTextStyle(TextStyle textStyle);
		void SetColor(ZColor foreground, ZColor background);
		void SoundEffect(ushort number);
		void Quit();
        byte ScreenHeight { get; }
        byte ScreenWidth { get; }
    }
}
