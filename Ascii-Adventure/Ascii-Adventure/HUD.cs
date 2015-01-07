using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCIIR2
{
	public class HUD
	{
		public byte Keys { get; set; }
		public byte Bombs { get; set; }
		public byte MagicAmt { get; set; }
		public int Money { get; set; }
		public List<ConsoleColor> Spheres { get; set; }

		public HUD()
		{
			Keys = 0;
			Bombs = 0;
			MagicAmt = 0;
			Money = 0;

			Spheres = new List<ConsoleColor>();
		}

		public void AddItem(ItemType it)
		{
			switch (it)
			{
				default:
				case ItemType.Magic:
					MagicAmt+=2;
					break;
				case ItemType.Key:
					Keys++;
					break;
				case ItemType.Heart:
					break;
				case ItemType.Bomb:
					Bombs++;
					break;
				case ItemType.Crystal:
					Money += 10;
					break;
			}
		}

		public void DrawDivider()
		{
			for (int x = 0; x < Console.WindowWidth; x++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(x), (short)(21)), '-', ConsoleColor.DarkGray);
			}
		}

		public void DrawSword(SwordType st)
		{
			short start = (short)(Console.WindowWidth / 3);
			for (short x = start; x < (start + 11); x++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD(x, (short)(Console.WindowHeight - 2)), '-', ConsoleColor.White);
				if (x == start + 2 || x == start + 8)
				{
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD(x, (short)(Console.WindowHeight - 8)), ' ', ConsoleColor.White);
				}
				else if (x == start + 3)
				{
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD(x, (short)(Console.WindowHeight - 8)), "Space", ConsoleColor.White);
				}
				else if (x >= start + 4 && x <= start + 7)
				{
					continue;
				}
				else
				{
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD(x, (short)(Console.WindowHeight - 8)), '-', ConsoleColor.White);
				}
			}
			for (short y = (short)(Console.WindowHeight - 8); y <= Console.WindowHeight - 2; y++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start - 1), y), '|', ConsoleColor.White);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 11), y), '|', ConsoleColor.White);
			}

			switch (st)
			{
				case SwordType.Wood:
				default:
					for (short y = 0; y < 4; y++)
					{
						if (y == 2)
						{
							for (short x = 0; x < 3; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 4 + x), (short)(Console.WindowHeight - 6 + y)), '=', ConsoleColor.DarkYellow);
							}
						}
						else
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 5), (short)(Console.WindowHeight - 6 + y)), '|', ConsoleColor.DarkYellow);
						}
					}
					break;
				case SwordType.Short:
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 7), (short)(Console.WindowHeight - 6)), '/', ConsoleColor.Gray);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 6), (short)(Console.WindowHeight - 5)), '/', ConsoleColor.Gray);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 4), (short)(Console.WindowHeight - 3)), '/', ConsoleColor.Gray);
					for (short x = 0; x < 3; x++)
					{
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 4 + x), (short)(Console.WindowHeight - 4)), '=', ConsoleColor.Gray);
					}
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 3), (short)(Console.WindowHeight - 4)), '<', ConsoleColor.Gray);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 7), (short)(Console.WindowHeight - 4)), '>', ConsoleColor.Gray);
					break;
				case SwordType.Great:
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 6), (short)(Console.WindowHeight - 7)), '/', ConsoleColor.Cyan);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 7), (short)(Console.WindowHeight - 7)), '\\', ConsoleColor.Cyan);

					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 5), (short)(Console.WindowHeight - 6)), '/', ConsoleColor.Cyan);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 7), (short)(Console.WindowHeight - 6)), '/', ConsoleColor.Cyan);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 4), (short)(Console.WindowHeight - 5)), '/', ConsoleColor.Cyan);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 6), (short)(Console.WindowHeight - 5)), '/', ConsoleColor.Cyan);
					for (short x = 0; x < 4; x++)
					{
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 3 + x), (short)(Console.WindowHeight - 4)), '=', ConsoleColor.Cyan);
					}
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 2), (short)(Console.WindowHeight - 4)), '<', ConsoleColor.Cyan);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 7), (short)(Console.WindowHeight - 4)), '>', ConsoleColor.Cyan);

					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 3), (short)(Console.WindowHeight - 3)), '/', ConsoleColor.Cyan);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 4), (short)(Console.WindowHeight - 3)), '/', ConsoleColor.Cyan);
					break;
			}
		}

		public void DrawItem(ItemType it)
		{
			short start = (short)(Console.WindowWidth / 3 + 15);
			for (short x = start; x < (start + 11); x++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD(x, (short)(Console.WindowHeight - 2)), '-', ConsoleColor.White);
				if (x == start + 4 || x == start + 6)
				{
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD(x, (short)(Console.WindowHeight - 8)), ' ', ConsoleColor.White);
				}
				else if (x == start + 5)
				{
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD(x, (short)(Console.WindowHeight - 8)), 'e', ConsoleColor.White);
				}
				else
				{
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD(x, (short)(Console.WindowHeight - 8)), '-', ConsoleColor.White);
				}
			}
			for (short y = (short)(Console.WindowHeight - 8); y <= Console.WindowHeight - 2; y++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start - 1), y), '|', ConsoleColor.White);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 11), y), '|', ConsoleColor.White);
			}

			switch (it)
			{
				case ItemType.Bomb:
				default:
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 6), (short)(Console.WindowHeight - 7)), '*', ConsoleColor.DarkBlue);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 5), (short)(Console.WindowHeight - 6)), '/', ConsoleColor.Gray);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 4), (short)(Console.WindowHeight - 6)), '_', ConsoleColor.Blue);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 3), (short)(Console.WindowHeight - 5)), '/', ConsoleColor.Blue);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 5), (short)(Console.WindowHeight - 5)), '\\', ConsoleColor.Blue);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 3), (short)(Console.WindowHeight - 4)), '\\', ConsoleColor.Blue);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 4), (short)(Console.WindowHeight - 4)), '_', ConsoleColor.Blue);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 5), (short)(Console.WindowHeight - 4)), '/', ConsoleColor.Blue);
					break;
				case ItemType.Magic:
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 5), (short)(Console.WindowHeight - 7)), '_', ConsoleColor.DarkGray);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 4), (short)(Console.WindowHeight - 6)), '/', ConsoleColor.DarkGray);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 6), (short)(Console.WindowHeight - 6)), '\\', ConsoleColor.DarkGray);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 4), (short)(Console.WindowHeight - 5)), '|', ConsoleColor.White);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 5), (short)(Console.WindowHeight - 5)), '~', ConsoleColor.DarkCyan);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 6), (short)(Console.WindowHeight - 5)), '|', ConsoleColor.White);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 4), (short)(Console.WindowHeight - 4)), '\\', ConsoleColor.DarkGray);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 5), (short)(Console.WindowHeight - 4)), '_', ConsoleColor.DarkGray);
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(start + 6), (short)(Console.WindowHeight - 4)), '/', ConsoleColor.DarkGray);
					break;
			}
		}

		public void DrawItemAmounts()
		{
			short xStart = (short)(Console.WindowWidth / 1.25);
			short yStart = (short)(Console.WindowHeight - 7);

			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xStart, yStart), "Keys: " + Keys, ConsoleColor.White);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xStart, (short)(yStart + 1)), "Bombs: " + Bombs, ConsoleColor.DarkBlue);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xStart, (short)(yStart + 2)), "Magic: " + MagicAmt, ConsoleColor.DarkCyan);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xStart, (short)(yStart + 3)), (char)(0xA7), ConsoleColor.Yellow);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(xStart + 1), (short)(yStart + 3)), ": " + Money, ConsoleColor.Yellow);
		}

		public void DrawHealth(int hp)
		{
			short yStep = (short)(Console.WindowHeight - 7);
			short xStep = 3;
			for (int x = 0; x < hp; x++)
			{
				if (x > hp) break;
				if (x % 16 == 0)
				{
					xStep = 3;
					yStep++;
				}
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xStep, yStep), '|', ConsoleColor.Red);
				xStep++;
			}
		}

		public void ClearHealth(int hp)
		{
			int yStep = Console.WindowHeight - 7;
			int xStep = 3;
			for (int x = 0; x < (hp * 2); x++)
			{
				if (x > (hp * 2)) break;
				if (x % 16 == 0)
				{
					xStep = 3;
					yStep++;
				}
				Console.SetCursorPosition(xStep, yStep);
				Console.Write(' ');
				xStep++;
			}
		}

		public void DrawSpheres()
		{
			short xStart = (short)(Console.WindowWidth / 1.25);
			short yStart = (short)(Console.WindowHeight - 3);
			for (short x = 0; x < Spheres.Count; x++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(xStart + x), yStart), (char)0xA4, Spheres[x]);
			}
		}
	}
}
