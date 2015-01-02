using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCIIR2
{
	public enum ItemType { Bomb, Key, Heart, Magic, Crystal, Short, Great, HeartPiece, Sphere }
	public class Item
	{
		public EngineFunctions.COORD Location { get; set; }
		public ItemType IType { get; set; }

		public Item(ItemType type, EngineFunctions.COORD loc)
		{
			Location = loc;
			IType = type;
		}

		public void DrawItem()
		{
			switch (IType)
			{
				case ItemType.Bomb:
					EngineFunctions.DrawToConsole(Location, 'B', ConsoleColor.DarkBlue);
					break;
				case ItemType.Heart:
					EngineFunctions.DrawToConsole(Location, 'H', ConsoleColor.Magenta);
					break;
				case ItemType.Key:
					EngineFunctions.DrawToConsole(Location, 'K', ConsoleColor.White);
					break;
				case ItemType.Magic:
					EngineFunctions.DrawToConsole(Location, 'M', ConsoleColor.DarkCyan);
					break;
				case ItemType.Crystal:
					EngineFunctions.DrawToConsole(Location, (char)0xA4, ConsoleColor.Yellow);
					break;
				case ItemType.Sphere:
					EngineFunctions.DrawToConsole(Location, '0', Map.CurLevelInfo.Key);
					break;
			}
		}
	}
}
