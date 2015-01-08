using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ASCIIR2
{
	public enum EType { Grunt, Beast, Fighter, Glider, Warrior, Dragon }
	public class Enemy
	{
		public EngineFunctions.COORD loc { get; private set; }
		public EType enemytype {get; private set;}
		public int HP { get; set; }
		public int Damage { get; set; }
		private TimeSpan timeSinceUpdate = TimeSpan.Zero;
		
		public Enemy(EType type)
		{
			enemytype = type;
			switch (enemytype)
			{
				default:
				case EType.Grunt: //move randomly
					HP = 2;
					Damage = 2;
					break;
				case EType.Beast: //follow
					HP = 8;
					Damage = 4;
					break;
				case EType.Fighter: // smart follow
					HP = 12;
					Damage = 6;
					break;
				case EType.Warrior: // smart follow
					HP = 20;
					Damage = 6;
					break;
				case EType.Glider: // follow
					HP = 6;
					Damage = 2;
					break;
				case EType.Dragon: //follow 
					HP = 16;
					Damage = 6;
					break;
			}				
		}

		public bool FindPosition(int Left, int Right)
		{
			bool posFound = false;
			byte tries = 0;
			while (!posFound && tries < 20)
			{
				short xPos = (short)(Game.rand.Next(Left + 1, Right - 1));
				short yPos = (short)(Game.rand.Next(5, 19));
				if (EngineFunctions.screenBufferArray[yPos, xPos].AsciiChar == ' ')
				{
					posFound = true;
					loc = new EngineFunctions.COORD(xPos, yPos);
					break;
				}
				tries++;
			}

			if (!posFound) return false;
			else return true;
		}

		public void Killed()
		{
			EngineFunctions.DrawToConsole(loc, ' ');
		}

		public void MoveEnemy()
		{
			if (DateTime.Now.TimeOfDay.Subtract(timeSinceUpdate) > TimeSpan.FromSeconds(1))
			{
				timeSinceUpdate = DateTime.Now.TimeOfDay;
				EngineFunctions.COORD curPlayerPos = Game.player.location;

				// modify x position
				if (loc.X < curPlayerPos.X && loc.X < Console.WindowWidth - 3)
				{
					if (!CheckCollision(FaceDirection.Right))
					{
						EngineFunctions.DrawToConsole(loc, ' ');
						loc = new EngineFunctions.COORD((short)(loc.X + 1), loc.Y);
					}
				}
				else if (loc.X > curPlayerPos.X && loc.X > 2)
				{
					if (!CheckCollision(FaceDirection.Left))
					{
						EngineFunctions.DrawToConsole(loc, ' ');
						loc = new EngineFunctions.COORD((short)(loc.X - 1), loc.Y);
					}
				}

				// modify y position
				if (loc.Y < curPlayerPos.Y && loc.Y < Console.WindowHeight - 13)
				{
					if (!CheckCollision(FaceDirection.Down))
					{
						EngineFunctions.DrawToConsole(loc, ' ');
						loc = new EngineFunctions.COORD(loc.X, (short)(loc.Y + 1));
					}
				}
				else if (loc.Y > curPlayerPos.Y && loc.Y > 2)
				{
					if (!CheckCollision(FaceDirection.Up))
					{
						EngineFunctions.DrawToConsole(loc, ' ');
						loc = new EngineFunctions.COORD(loc.X, (short)(loc.Y - 1));
					}
				}
			}
			DrawEnemy();
		}

		public void DrawEnemy()
		{
			switch (enemytype)
			{
				default:
				case EType.Grunt:
					EngineFunctions.DrawToConsole(loc, 'E', ConsoleColor.DarkRed);
					break;
				case EType.Beast:
					EngineFunctions.DrawToConsole(loc, 'B', ConsoleColor.DarkMagenta);
					break;
				case EType.Fighter:
					EngineFunctions.DrawToConsole(loc, 'F', ConsoleColor.Blue);
					break;
				case EType.Warrior:
					EngineFunctions.DrawToConsole(loc, 'W', ConsoleColor.Red);
					break;
				case EType.Glider:
					EngineFunctions.DrawToConsole(loc, 'G', ConsoleColor.DarkCyan);
					break;
				case EType.Dragon:
					EngineFunctions.DrawToConsole(loc, 'D', Map.CurLevelInfo.Key);
					break;
			}			
		}
		public bool CheckCollision(FaceDirection dir)
		{
			if (enemytype == EType.Glider || enemytype == EType.Dragon) return false;
			switch (dir)
			{
				case FaceDirection.Up:
					if (EngineFunctions.COLLISION_CHARS.Contains(EngineFunctions.screenBufferArray[loc.Y - 1, loc.X].AsciiChar))
					{
						return true;
					}
					break;
				case FaceDirection.Down:
					if (EngineFunctions.COLLISION_CHARS.Contains(EngineFunctions.screenBufferArray[loc.Y + 1, loc.X].AsciiChar))
					{
						return true;
					}
					break;
				case FaceDirection.Left:
					if (EngineFunctions.COLLISION_CHARS.Contains(EngineFunctions.screenBufferArray[loc.Y, loc.X - 1].AsciiChar))
					{
						return true;
					}
					break;
				case FaceDirection.Right:
					if (EngineFunctions.COLLISION_CHARS.Contains(EngineFunctions.screenBufferArray[loc.Y, loc.X + 1].AsciiChar))
					{
						return true;
					}
					break;
			}
			return false;
		}
	}
}
