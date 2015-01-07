using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCIIR2
{
	class Program
	{
        //public static SaveHandler saver;
		static void Main(string[] args)
		{
            //saver = new SaveHandler();
			Game g = new Game();
			Console.CursorVisible = false;
			Console.Title = "ASCII-R 2";
			g.Start();
		}

		public static void CloseGame()
		{
			Environment.Exit(0);
		}
	}
}
