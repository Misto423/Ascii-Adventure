using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ASCIIR2
{
	public class Menu
	{
		private readonly string line1 = @"   ___|   _ \    \  |   ___|    _ \   |      ____|";
		private readonly string line2 = @"  |      |   |    \ | \___ \   |   |  |      __|  ";
		private readonly string line3 = @"  |      |   |  |\  |       |  |   |  |      |    ";
		private readonly string line4 = @" \____| \___/  _| \_| _____/  \___/  _____| _____|";

		private readonly string line5 = @"      |      ____|   ___|  ____|   \  |  __ \  ";
		private readonly string line6 = @"      |      __|    |      __|      \ |  |   | ";
		private readonly string line7 = @"      |      |      |   |  |      |\  |  |   | ";
		private readonly string line8 = @"     _____| _____| \____| _____| _| \_| ____/ ";

		private int selection = 0;
		private Thread inputThread;
		private TimeSpan timeSinceUpdate = TimeSpan.Zero;
		private Input input;
		private char c;

		public Menu()
		{
			input = new Input();
			inputThread = new Thread(new ThreadStart(GetInput));
			inputThread.Start();
		}

		public void GetInput()
		{
			while (true)
			{
				EngineFunctions.Flush();
				if (DateTime.Now.TimeOfDay.Subtract(timeSinceUpdate) > TimeSpan.FromMilliseconds(50))
				{
					c = input.GetInput();
					timeSinceUpdate = DateTime.Now.TimeOfDay;
				}
			}
		}

		public int UpdateMenu()
		{
			if (c == 'w')
			{
				if (selection == 0)
				{
					selection = 2;
				}
				else
				{
					selection--;
				}
			}
			else if (c == 's')
			{
				if (selection == 2)
				{
					selection = 0;
				}
				else
				{
					selection++;
				}
			}
			else if (c == ' ')
			{
				inputThread.Abort();
				inputThread.Join();
				switch (selection)
				{
					default:
					case 0:
						return 0;
					case 1:
						return 1;
					case 2:
						return 2;
				}
			}
			c = 'n';
			return -1;
		}

		public void DrawMenu()
		{
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(10, 5), line1, ConsoleColor.White);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(10, 6), line2, ConsoleColor.White);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(10, 7), line3, ConsoleColor.White);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(10, 8), line4, ConsoleColor.White);

			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(20, 12), line5, ConsoleColor.White);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(20, 13), line6, ConsoleColor.White);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(20, 14), line7, ConsoleColor.White);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(20, 15), line8, ConsoleColor.White);

			for (int x = 0; x < Console.WindowWidth; x++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)x, 17), '-', ConsoleColor.White);
			}

			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(35, 20), "New Game", ConsoleColor.White);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(35, 22), "Load Game", ConsoleColor.White);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(35, 24), "Exit Game", ConsoleColor.White);

			switch (selection)
			{
				case 0:
				default:
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD(31, 20), "->", ConsoleColor.White);
					break;
				case 1:
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD(31, 22), "->", ConsoleColor.White);
					break;
				case 2:
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD(31, 24), "->", ConsoleColor.White);
					break;
			}
		}
	}
}
