using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace ASCIIR2
{
	public class EngineFunctions
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct COORD 
		{
			public short X;
			public short Y;

			public COORD(short X, short Y)
			{
				this.X = X;
				this.Y = Y;
			}
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct SMALL_RECT
		{

			public short Left;
			public short Top;
			public short Right;
			public short Bottom;

		};

		[StructLayout(LayoutKind.Sequential)]
		public struct CONSOLE_SCREEN_BUFFER_INFO_EX
		{
			public uint cbSize;
			public COORD dwSize;
			public COORD dwCursorPosition;
			public short wAttributes;
			public SMALL_RECT srWindow;
			public COORD dwMaximumWindowSize;

			public ushort wPopupAttributes;
			public bool bFullscreenSupported;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public COLORREF[] ColorTable;

			public static CONSOLE_SCREEN_BUFFER_INFO_EX Create()
			{
				return new CONSOLE_SCREEN_BUFFER_INFO_EX { cbSize = 96 };
			}
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct COLORREF
		{
			public uint ColorDWORD;

			public COLORREF(System.Drawing.Color color)
			{
				ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
			}

			public COLORREF(uint r, uint g, uint b)
			{
				ColorDWORD = r + (g << 8) + (b << 16);
			}

			public System.Drawing.Color GetColor()
			{
				return System.Drawing.Color.FromArgb((int)(0x000000FFU & ColorDWORD),
				   (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
			}

			public void SetColor(System.Drawing.Color color)
			{
				ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
			}
		};

		[StructLayout(LayoutKind.Explicit)]
		public struct CHAR_INFO
		{
			[FieldOffset(0)]
			internal char UnicodeChar;
			[FieldOffset(0)]
			internal char AsciiChar;
			[FieldOffset(2)]
			internal UInt16 Attributes;
		}

		//used for collision
		[DllImport("kernel32.dll", SetLastError=true)]
		private static extern bool ReadConsoleOutputCharacter(IntPtr hConsoleOutput,
			[Out] StringBuilder lpCharacter, uint nLenght, COORD dwReadCoord, out uint lpNumberOfCharactersRead);

		//Flush Console Input
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FlushConsoleInputBuffer(IntPtr hConsoleInput);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint="CreateFileW")]
		public static extern IntPtr GetInputBuffer([MarshalAs(UnmanagedType.LPWStr)] string filename,
			 [MarshalAs(UnmanagedType.U4)] UInt32 access, [MarshalAs(UnmanagedType.U4)] UInt32 share,
			 IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] UInt32 creationDisposition,
			 [MarshalAs(UnmanagedType.U4)] UInt32 flagsAndAttributes, IntPtr templateFile);

		//Used to change color palette
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbie);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbie);

		//get the console window handle
		[DllImport("kernel32.dll", SetLastError=true)]
		private static extern IntPtr GetStdHandle(int nStdHandle);

		//double buffering
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleActiveScreenBuffer(IntPtr hConsoleOutput);

		[DllImport("Kernel32.dll", SetLastError=true)]
		private static extern IntPtr CreateConsoleScreenBuffer(UInt32 dwDesiredAccess, UInt32 dwShareMode, IntPtr securityAttributes,
			 UInt32 flags, [MarshalAs(UnmanagedType.U4)] UInt32 screenBufferData);

		[DllImport("kernel32.dll", EntryPoint = "WriteConsoleOutputW", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool WriteConsoleOutput(IntPtr hConsoleBuffer, [MarshalAs(UnmanagedType.LPArray), In] CHAR_INFO[,] lpBuffer,
			COORD BufferSize, COORD BufferCoord, ref SMALL_RECT lpReadRegion);

		[DllImport("kernel32.dll", EntryPoint = "ReadConsoleOutputW", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool ReadConsoleOutput(IntPtr hConsoleBuffer, [MarshalAs(UnmanagedType.LPArray), Out] CHAR_INFO[,] lpBuffer,
			COORD BufferSize, COORD BufferCoord, ref SMALL_RECT lpReadRegion);

		//Variables
		public const int STD_INPUT_HANDLE = -10;
		public const int STD_OUTPUT_HANDLE = -11;

		public static readonly string COLLISION_CHARS = @"#X_\/SE-~|°¥";
		//Current Screen Array
		public static CHAR_INFO[,] screenBufferArray = new CHAR_INFO[30,80];

		public static void AdjustColorPalette()
		{
			CONSOLE_SCREEN_BUFFER_INFO_EX cinfo = new CONSOLE_SCREEN_BUFFER_INFO_EX();
			
			cinfo.cbSize = (uint)Marshal.SizeOf(cinfo);
			IntPtr ConsoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
			GetConsoleScreenBufferInfoEx(ConsoleHandle, ref cinfo);

			cinfo.ColorTable[1] = new COLORREF(255, 128, 0);
			cinfo.ColorTable[4] = new COLORREF(198, 98, 251);
			cinfo.ColorTable[6] = new COLORREF(128, 64, 0);
			++cinfo.srWindow.Bottom;
			++cinfo.srWindow.Right;
			SetConsoleScreenBufferInfoEx(ConsoleHandle, ref cinfo);
		}

		public static void SetBackgroundColor(ConsoleColor color)
		{
			for (int x = 0; x < Console.WindowWidth; x++)
			{
				for (int y = 0; y < Console.WindowHeight - 10; y++)
				{
					DrawToConsole(new COORD((short)x, (short)y), ' ', ConsoleColor.Black, color);
				}
			}
		}

		public static void GetScreenBuffer()
		{
			SMALL_RECT readArea;

			readArea.Top = 0;
			readArea.Left = 0;
			readArea.Right = 79;
			readArea.Bottom = 29;
			bool success = ReadConsoleOutput(GetStdHandle(STD_OUTPUT_HANDLE), screenBufferArray,
				new COORD(80, 30), new COORD(0, 0), ref readArea);
			if (!success)
			{
				int g = Marshal.GetLastWin32Error();
			}
		}

		public static void ClearBuffer()
		{
			for (int y = 0; y < screenBufferArray.GetLength(0); y++)
			{
				for (int x = 0; x < screenBufferArray.GetLength(1); x++)
				{
					screenBufferArray[y, x].AsciiChar = ' ';
				}
			}
		}

		public static void DrawToBuffer()
		{
			IntPtr buffer = CreateConsoleScreenBuffer(0x40000000 | 0x80000000, 0x00000001 | 0x00000002, IntPtr.Zero, 1, 0);

			//Test Code
			Int32 error = Marshal.GetLastWin32Error();
			if (error != 0) throw new System.ComponentModel.Win32Exception(error);

			SMALL_RECT readArea;

			readArea.Top = 0;
			readArea.Left = 0;
			readArea.Right = 79;
			readArea.Bottom = 29;
			
			WriteConsoleOutput(buffer, screenBufferArray, new COORD(80, 30), new COORD(0, 0), ref readArea);
			//ClearBuffer();
			//Test Code
			error = Marshal.GetLastWin32Error();
			if (error != 0) throw new System.ComponentModel.Win32Exception(error);
			
			SetConsoleActiveScreenBuffer(buffer);
			//Test Code
			error = Marshal.GetLastWin32Error();
			if (error != 0) throw new System.ComponentModel.Win32Exception(error);

		}

		public static void Flush()
		{
			IntPtr inBuffer = GetInputBuffer("CONIN$", 0x40000000 | 0x80000000, 
				0x00000001, IntPtr.Zero, 3, 0x80, IntPtr.Zero);

			//Test Code
			Int32 error = Marshal.GetLastWin32Error();
			if (error != 0) throw new System.ComponentModel.Win32Exception(error);

			FlushConsoleInputBuffer(inBuffer);
			
			//Test Code
			error = Marshal.GetLastWin32Error();
			if (error != 0) throw new System.ComponentModel.Win32Exception(error);
		}

		public static char GetCharacterAtPosition(EngineFunctions.COORD	pos)
		{
			IntPtr handler = GetStdHandle(STD_OUTPUT_HANDLE);
			uint width = 1;
			StringBuilder sb = new StringBuilder((int)width);
			COORD readCoord = new COORD(pos.X, pos.Y);
			uint numCharsRead = 0;
			if (!ReadConsoleOutputCharacter(handler, sb, width, readCoord, out numCharsRead))
				return (char)0;
			else return sb.ToString()[0];
		}

		#region Console Drawing Functions
		public static void SetForegroundColor(COORD p, ConsoleColor color)
		{
			switch (color)
			{
				case ConsoleColor.White:
				default:
					screenBufferArray[p.Y, p.X].Attributes = 0xF;
					break;
				case ConsoleColor.Yellow:
					screenBufferArray[p.Y, p.X].Attributes = 0xE;
					break;
				case ConsoleColor.Magenta:
					screenBufferArray[p.Y, p.X].Attributes = 0xD;
					break;
				case ConsoleColor.Red:
					screenBufferArray[p.Y, p.X].Attributes = 0xC;
					break;
				case ConsoleColor.Cyan:
					screenBufferArray[p.Y, p.X].Attributes = 0xB;
					break;
				case ConsoleColor.Green:
					screenBufferArray[p.Y, p.X].Attributes = 0xA;
					break;
				case ConsoleColor.Blue:
					screenBufferArray[p.Y, p.X].Attributes = 0x9;
					break;
				case ConsoleColor.DarkGray:
					screenBufferArray[p.Y, p.X].Attributes = 0x8;
					break;
				case ConsoleColor.Gray:
					screenBufferArray[p.Y, p.X].Attributes = 0x7;
					break;
				case ConsoleColor.DarkYellow:
					screenBufferArray[p.Y, p.X].Attributes = 0x6;
					break;
				case ConsoleColor.DarkMagenta:
					screenBufferArray[p.Y, p.X].Attributes = 0x5;
					break;
				case ConsoleColor.DarkRed:
					screenBufferArray[p.Y, p.X].Attributes = 0x4;
					break;
				case ConsoleColor.DarkCyan:
					screenBufferArray[p.Y, p.X].Attributes = 0x3;
					break;
				case ConsoleColor.DarkGreen:
					screenBufferArray[p.Y, p.X].Attributes = 0x2;
					break;
				case ConsoleColor.DarkBlue:
					screenBufferArray[p.Y, p.X].Attributes = 0x1;
					break;
				case ConsoleColor.Black:
					screenBufferArray[p.Y, p.X].Attributes = 0x0;
					break;
			}
		}
		public static void SetBackgroundColor(COORD p, ConsoleColor color)
		{
			switch (color)
			{
				case ConsoleColor.White:
				default:
					screenBufferArray[p.Y, p.X].Attributes |= 0xF0;
					break;
				case ConsoleColor.Yellow:
					screenBufferArray[p.Y, p.X].Attributes |= 0xE0;
					break;
				case ConsoleColor.Magenta:
					screenBufferArray[p.Y, p.X].Attributes |= 0xD0;
					break;
				case ConsoleColor.Red:
					screenBufferArray[p.Y, p.X].Attributes |= 0xC0;
					break;
				case ConsoleColor.Cyan:
					screenBufferArray[p.Y, p.X].Attributes |= 0xB0;
					break;
				case ConsoleColor.Green:
					screenBufferArray[p.Y, p.X].Attributes |= 0xA0;
					break;
				case ConsoleColor.Blue:
					screenBufferArray[p.Y, p.X].Attributes |= 0x90;
					break;
				case ConsoleColor.DarkGray:
					screenBufferArray[p.Y, p.X].Attributes |= 0x80;
					break;
				case ConsoleColor.Gray:
					screenBufferArray[p.Y, p.X].Attributes |= 0x70;
					break;
				case ConsoleColor.DarkYellow:
					screenBufferArray[p.Y, p.X].Attributes |= 0x60;
					break;
				case ConsoleColor.DarkMagenta:
					screenBufferArray[p.Y, p.X].Attributes |= 0x50;
					break;
				case ConsoleColor.DarkRed:
					screenBufferArray[p.Y, p.X].Attributes |= 0x40;
					break;
				case ConsoleColor.DarkCyan:
					screenBufferArray[p.Y, p.X].Attributes |= 0x30;
					break;
				case ConsoleColor.DarkGreen:
					screenBufferArray[p.Y, p.X].Attributes |= 0x20;
					break;
				case ConsoleColor.DarkBlue:
					screenBufferArray[p.Y, p.X].Attributes |= 0x10;
					break;
				case ConsoleColor.Black:
					screenBufferArray[p.Y, p.X].Attributes |= 0x00;
					break;
			}
		}

		public static void DrawToConsole(COORD p, char c)
		{
			screenBufferArray[p.Y, p.X].AsciiChar = c;
		}
		public static void DrawToConsole(COORD p, COORD erase, char c)
		{
			screenBufferArray[p.Y, p.X].AsciiChar = c;
			screenBufferArray[erase.Y, erase.X].AsciiChar = ' ';
		}
		public static void DrawToConsole(COORD p, char c, ConsoleColor color)
		{
			SetForegroundColor(p, color);
			screenBufferArray[p.Y, p.X].AsciiChar = c;
		}
		public static void DrawToConsole(COORD p, COORD erase, char c, ConsoleColor color)
		{
			SetForegroundColor(p, color);
			screenBufferArray[p.Y, p.X].AsciiChar = c;
			screenBufferArray[erase.Y, erase.X].AsciiChar = ' ';
		}
		public static void DrawToConsole(COORD p, string c, ConsoleColor color)
		{
			int stringPos = 0;
			for (short x = p.X; x < p.X + c.Length; x++)
			{
				SetForegroundColor(new COORD(x, p.Y), color);
				screenBufferArray[p.Y, x].AsciiChar = c[stringPos];
				stringPos++;
			}
		}
		public static void DrawToConsole(COORD p, char c, ConsoleColor fg, ConsoleColor bg)
		{
			SetForegroundColor(p, fg);
			SetBackgroundColor(p, bg);
			screenBufferArray[p.Y, p.X].AsciiChar = c;
		}
		#endregion
	}
}
