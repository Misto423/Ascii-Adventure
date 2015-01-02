using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCIIR2
{
	public class Room
	{
		public byte RoomType { get; private set; }
		public List<Enemy> enemies { get; set; }
		private List<EngineFunctions.SMALL_RECT> blocks;
		public List<EngineFunctions.COORD> destroyed;
		public bool IsExplored { get; set; }

		public Room(byte type, bool GenEnemies)
		{
			this.RoomType = type;
			enemies = new List<Enemy>();
			blocks = new List<EngineFunctions.SMALL_RECT>();
			destroyed = new List<EngineFunctions.COORD>();
			IsExplored = false;
			GenerateObstacles();
			DrawRoom();
			if (GenEnemies)
			{
				GenerateEnemies();
			}
		}

		public void GenerateEnemies()
		{
			int numEnemies = Game.rand.Next(0, 9);
			for (int n = 0; n < numEnemies; n++)
			{
                EType et;
                int chance = Game.rand.Next(0, 100);
                if (chance >= 60)
                    et = EType.Grunt;
                else if (chance >= 40)
                    et = EType.Glider;
                else if (chance >= 20)
                    et = EType.Beast;
                else if (chance >= 10)
                    et = EType.Fighter;
                else
                    et = EType.Warrior;
				Enemy e = new Enemy(et);
                if (RoomType == 9)
                {
                    int left = Console.WindowWidth - 3 * (Console.WindowWidth / 4);
                    if (e.FindPosition(left, 78))
                    {
                        enemies.Add(e);
                    }
                }
                else if (RoomType == 10)
                {
                    int right = Console.WindowWidth - (Console.WindowWidth / 4);
                    if (e.FindPosition(5, right))
                    {
                        enemies.Add(e);
                    }
                }
                else if (RoomType == 11 || RoomType == 12)
                {
                    int left = Console.WindowWidth - 3 * (Console.WindowWidth / 4);
                    int right = Console.WindowWidth - (Console.WindowWidth / 4);
                    if (e.FindPosition(left, right))
                    {
                        enemies.Add(e);
                    }
                }
                else
                {
                    if (e.FindPosition(5, 78))
                    {
                        enemies.Add(e);
                    }
                }
			}
		}

		public void GenerateObstacles()
		{
			int num = Game.rand.Next(0, 5);
			for (int i = 0; i < num; i++)
			{
				EngineFunctions.SMALL_RECT obst = new EngineFunctions.SMALL_RECT();
				obst.Left = (short)(Game.rand.Next(10, 66));
				obst.Right = (short)(Game.rand.Next(obst.Left, obst.Left + (Console.WindowWidth - obst.Left - 10)));
				obst.Top = (short)(Game.rand.Next(3, 9));
				obst.Bottom = (short)(Game.rand.Next(obst.Top, obst.Top + (Console.WindowHeight - obst.Top - 13)));
				blocks.Add(obst);
			}
		}

		public void DrawRoom()
		{
			short yBottom = (short)(Console.WindowHeight - 10);
			short xFar = (short)(Console.WindowWidth - 1);
			#region Room Borders
			switch (RoomType)
			{
				case 0:
					{
						#region 4-way intersection
						for (short i = 0; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(i, (short)(0)), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(i, yBottom), '#', ConsoleColor.Gray);
						}
						for (short i = (short)(Console.WindowWidth / 2 + 2); i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(i, (short)(0)), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(i, yBottom), '#', ConsoleColor.Gray);
						}
						for (short i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(0), i), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, i), '#', ConsoleColor.Gray);
						}
						for (short i = (short)((Console.WindowHeight - 10) / 2 + 2); i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(0), i), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, i), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 1:
					{
						#region T-up Intersection
						for (int i = 0; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = Console.WindowWidth / 2 + 2; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 2:
					{
						#region T-down Intersection
						for (int i = 0; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = Console.WindowWidth / 2 + 2; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 3:
					{
						#region T-Left intersection
						for (int i = 0; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = Console.WindowWidth / 2 + 2; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 4:
					{
						#region T-Right intersection
						for (int i = 0; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = Console.WindowWidth / 2 + 2; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 5:
					{
						#region NW-corner
						for (int i = 0; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
						}
						for (int i = Console.WindowWidth / 2 + 2; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray); ;
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 6:
					{
						#region NE-corner
						for (int i = 0; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
						}
						for (int i = Console.WindowWidth / 2 + 2; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 7:
					{
						#region SW-corner
						for (int i = 0; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = Console.WindowWidth / 2 + 2; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 8:
					{
						#region SE-corner
						for (int i = 0; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = Console.WindowWidth / 2 + 2; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 9:
					{
						#region E-dead End
						short stop = (short)(Console.WindowWidth - 3 * (Console.WindowWidth / 4));
						for (int i = stop; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i <= Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(stop, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 10:
					{
						#region W-dead End
						short stop = (short)(Console.WindowWidth - (Console.WindowWidth / 4));
						for (int i = 0; i < stop; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i <= Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(stop, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 11:
					{
						#region N-dead End
						short Wstop = (short)(Console.WindowWidth - 3 * (Console.WindowWidth / 4));
						short Estop = (short)(Console.WindowWidth - (Console.WindowWidth / 4));
						for (int i = Wstop; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
						}
						for (int i = Console.WindowWidth / 2 + 2; i < Estop; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
						}
						for (int i = Wstop; i <= Estop; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(Wstop, (short)(i)), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(Estop, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 12:
					{
						#region S-dead End
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
						#endregion
						break;
					}
				case 13:
					{
						#region H-Hall
						for (int i = 0; i < Console.WindowWidth; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < (Console.WindowHeight - 10) / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						for (int i = (Console.WindowHeight - 10) / 2 + 2; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)(i)), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(xFar, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
				case 14:
					{
						#region V-Hall
						short Wstop = (short)(Console.WindowWidth - 3 * (Console.WindowWidth / 4));
						short Estop = (short)(Console.WindowWidth - (Console.WindowWidth / 4));
						for (int i = Wstop; i < Console.WindowWidth / 2 - 2; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = Console.WindowWidth / 2 + 2; i <= Estop; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), 0), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(i), yBottom), '#', ConsoleColor.Gray);
						}
						for (int i = 0; i < Console.WindowHeight - 10; i++)
						{
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(Wstop, (short)(i)), '#', ConsoleColor.Gray);
							EngineFunctions.DrawToConsole(new EngineFunctions.COORD(Estop, (short)(i)), '#', ConsoleColor.Gray);
						}
						#endregion
						break;
					}
			}
			#endregion

			#region Draw Obstacles
			for (int index = 0; index < blocks.Count; index++)
			{
				for (short x = blocks[index].Left; x <= blocks[index].Right; x++)
				{
					for (short y = blocks[index].Top; y <= blocks[index].Bottom; y++)
					{
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD(x, y), Map.CurLevelInfo.Value, Map.CurLevelInfo.Key);
					}
				}
			}
			#endregion

			#region Erase Sections
			for (int index = 0; index < destroyed.Count; index++)
			{
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD(destroyed[index].X, destroyed[index].Y), ' ');
			}
			#endregion
		}
	}
}
