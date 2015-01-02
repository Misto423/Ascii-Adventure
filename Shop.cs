using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCIIR2
{
	public class Shop
	{
		private Random rand;
		public ItemType[] itemsForSale;
		public int[] prices;
		public bool IsShopVisible { get; set; }
		public bool CanBuy { get; set; }
		public EngineFunctions.COORD location { get; private set; }

		public Shop()
		{
			rand = new Random();
			itemsForSale = new ItemType[3];
			prices = new int[3];
			CanBuy = true;
			location = new EngineFunctions.COORD(0, 0);
		}

		public void ShowShop(SwordType curSword, int maxHP)
		{
			int r = rand.Next(0, 100);
			if (r >= 85)
			{
				IsShopVisible = true;
				FindShopLocation();
				GenerateItems(curSword, maxHP);
			}
			else
			{
				IsShopVisible = false;
			}
		}

		public void GenerateItems(SwordType curSword, int maxHP)
		{
			for (int i = 0; i < 3; i++)
			{
				int h = rand.Next(0, 100);
				if (h >= 95)
					if (curSword == SwordType.Wood)
                    {
                        itemsForSale[i] = ItemType.Short;
                    }
                    else if (curSword == SwordType.Short)
                    {
                        itemsForSale[i] = ItemType.Great;
                    }
                    else
                    {
                        if (maxHP < 50)
                            itemsForSale[i] = ItemType.HeartPiece;
                        else
                            itemsForSale[i] = ItemType.Magic;
                    }
				else if (h >= 86)
                    if (maxHP < 50)
                        itemsForSale[i] = ItemType.HeartPiece;
                    else
                        itemsForSale[i] = ItemType.Magic;
				else if (h >= 50)
					itemsForSale[i] = ItemType.Heart;
				else if (h >= 25)
					itemsForSale[i] = ItemType.Bomb;
				else if (h >= 10)
					itemsForSale[i] = ItemType.Key;
				else
					itemsForSale[i] = ItemType.Magic;
			}
		}

		public void FindShopLocation()
		{
			EngineFunctions.CHAR_INFO[,] ar = EngineFunctions.screenBufferArray;
			bool locationFound = false;
			bool valid = true;
			int counter = 0;
			while (!locationFound && counter < 20)
			{
				counter++;
				short xRand = (short)rand.Next(1, 76);
				short yRand = (short)rand.Next(1, 16);

				if (ar[yRand, xRand].AsciiChar != ' ') continue;
				else
				{
					for (short x = 0; x < 5; x++)
					{
						for (short y = 0; y < 4; y++)
						{
							if (ar[yRand + y, xRand + x].AsciiChar != ' ') valid = false;
						}
					}
					if (valid)
					{
						locationFound = true;
						location = new EngineFunctions.COORD(xRand, yRand);
					}
				}
			}
			if (!locationFound)
			{
				IsShopVisible = false;
			}
		}

		public void DrawShopOnMap()
		{
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + 2), (short)(location.Y)), '_', ConsoleColor.DarkBlue);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + 1), (short)(location.Y + 1)), '/', ConsoleColor.DarkBlue);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + 3), (short)(location.Y + 1)), '\\', ConsoleColor.DarkBlue);

			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X), (short)(location.Y + 2)), '/', ConsoleColor.DarkBlue);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + 2), (short)(location.Y + 2)), 'S', ConsoleColor.DarkBlue);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + 4), (short)(location.Y + 2)), '\\', ConsoleColor.DarkBlue);

			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X), (short)(location.Y + 3)), '|', ConsoleColor.DarkBlue);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + 1), (short)(location.Y + 3)), '_', ConsoleColor.DarkBlue);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + 2), (short)(location.Y + 3)), (char)0x7F, ConsoleColor.DarkBlue);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + 3), (short)(location.Y + 3)), '_', ConsoleColor.DarkBlue);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + 4), (short)(location.Y + 3)), '|', ConsoleColor.DarkBlue);
		}

		public void DrawShop()
		{
			short yBottom = (short)(Console.WindowHeight - 10);
			short Wstop = (short)(Console.WindowWidth - 3 * (Console.WindowWidth / 4));
			short Estop = (short)(Console.WindowWidth - (Console.WindowWidth / 4));
			for (int i = Wstop; i < Console.WindowWidth / 2 - 2; i++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
			}
			for (int i = Console.WindowWidth / 2 + 2; i <= Estop; i++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
			}
			for (int i = Wstop; i <= Estop; i++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
			}
			for (int i = 0; i < Console.WindowHeight - 10; i++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD(Wstop, (short)(i)), '#', ConsoleColor.Gray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD(Estop, (short)(i)), '#', ConsoleColor.Gray);
			}

			if (CanBuy)
			{
				for (int x = Console.WindowWidth / 2 - 5; x < Console.WindowWidth / 2 + 5; x++)
				{
					EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)x, 3), '_', ConsoleColor.DarkGray);
				}

				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 - 6), 4), '/', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 + 5), 4), '\\', ConsoleColor.DarkGray);

				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 - 7), 5), '/', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 + 6), 5), '\\', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 - 3), 5), '*', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 + 2), 5), '*', ConsoleColor.DarkGray);

				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 - 8), 6), '/', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 + 7), 6), '\\', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 - 3), 6), '_', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 - 2), 6), '_', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 - 1), 6), '_', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2), 6), '_', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 + 1), 6), '_', ConsoleColor.DarkGray);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth / 2 + 2), 6), '_', ConsoleColor.DarkGray);

				DrawItemsToBuy();
			}
		}

		public void DrawItemsToBuy()
		{
			short x = 30;
			for (int index = 0; index < 3; index++)
			{
				prices[index] = GetPrice(itemsForSale[index]);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(x + index * 10), 9), prices[index].ToString(), ConsoleColor.Yellow);
				EngineFunctions.screenBufferArray[11, x + index * 10 + 1] = GetChar(itemsForSale[index]);
			}
		}

		public int GetPrice(ItemType i)
		{
			switch (i)
			{
				case ItemType.Bomb:
				default:
					return 45;
				case ItemType.Great:
					return 350;
				case ItemType.Heart:
					return 20;
				case ItemType.HeartPiece:
					return 150;
				case ItemType.Key:
					return 70;
				case ItemType.Magic:
					return 65;
				case ItemType.Short:
					return 125;
			}
		}
		public EngineFunctions.CHAR_INFO GetChar(ItemType i)
		{
			EngineFunctions.CHAR_INFO c = new EngineFunctions.CHAR_INFO();
			switch (i)
			{
				case ItemType.Bomb:
				default:
					c.AsciiChar = 'B';
					c.Attributes = 0x1;
					return c;
				case ItemType.Great:
					c.AsciiChar = 'G';
					c.Attributes = 0xB;
					return c;
				case ItemType.Heart:
					c.AsciiChar = 'H';
					c.Attributes = 0xD;
					return c;
				case ItemType.HeartPiece:
					c.AsciiChar = 'P';
					c.Attributes = 0xD;
					return c;
				case ItemType.Key:
					c.AsciiChar = 'K';
					c.Attributes = 0xF;
					return c;
				case ItemType.Magic:
					c.AsciiChar = 'M';
					c.Attributes = 0x3;
					return c;
				case ItemType.Short:
					c.AsciiChar = 'S';
					c.Attributes = 0x7;
					return c;
			}
		}
	}
}
