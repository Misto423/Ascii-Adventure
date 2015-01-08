using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ASCIIR2
{
	public enum FaceDirection { Up, Down, Left, Right, noDir }
	public enum SwordType { Wood, Short, Great }
	
	public class Player
	{
		public FaceDirection curFacing { get; private set; }
		public EngineFunctions.COORD location { get;  set; }
		private Input playerInput;
		private List<Enemy> enemiesInRoom;
		private bool transition = true;
		public bool TookDamage { get; set; }
		public bool SwingSword { get; set; }
		public bool IsBombPlaced { get; set; }
		public bool IsMagicUsed { get; set; }
        public bool UseKey { get; set; }
        public bool ShouldSave { get; set; }
        public byte SaveSlot { get; set; }
		public EngineFunctions.COORD bombLoc { get; private set; }
		//--------------------
		public int HP = 6;
		public int CurHP { get; set; }
		public SwordType Sword { get; set; }
		public ItemType CurItem { get; set; }
		//---------------------
		Thread inputThread;
		TimeSpan timeSinceUpdate = TimeSpan.Zero;
		char c;
		private bool SwordHit = false;

		public Player(EngineFunctions.COORD p)
		{
			playerInput = new Input();
			location = p;
			CurHP = HP;
			curFacing = FaceDirection.Down;
			Sword = SwordType.Wood;
			SwingSword = false;
			CurItem = ItemType.Magic;
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
					c = playerInput.GetInput();
					timeSinceUpdate = DateTime.Now.TimeOfDay;
				}
			}
		}

		public void UpdatePlayer(ref List<Enemy> e, out FaceDirection roomExit, List<FaceDirection> doors)
		{
			roomExit = FaceDirection.noDir;
			enemiesInRoom = e;
			if (c == 'w')
			{
				curFacing = FaceDirection.Up;
                if (location.Y > 0)
                {
                    if (!CheckCollision())
                    {
                        location = new EngineFunctions.COORD(location.X, (short)(location.Y - 1));
                        if (location.Y == 0 && transition)
                        {
                            roomExit = FaceDirection.Up;
                            transition = false;
                        }
                        else
                        {
                            transition = true;
                        }
                    }
                }
			}
			if (c == 's')
			{
				curFacing = FaceDirection.Down;
                if (location.Y < Console.WindowHeight - 10)
                {
                    if (!CheckCollision())
                    {
                        location = new EngineFunctions.COORD(location.X, (short)(location.Y + 1));
                        if (location.Y == Console.WindowHeight - 10 && transition)
                        {
                            roomExit = FaceDirection.Down;
                            transition = false;
                        }
                        else
                        {
                            transition = true;
                        }
                    }
                }
			}
			if (c == 'a')
			{
				curFacing = FaceDirection.Left;
                if (location.X > 0)
                {
                    if (!CheckCollision())
                    {
                        location = new EngineFunctions.COORD((short)(location.X - 1), location.Y);
                        if (location.X == 0 && transition)
                        {
                            roomExit = FaceDirection.Left;
                            transition = false;
                        }
                        else
                        {
                            transition = true;
                        }
                    }
                }
			}
			if (c == 'd')
			{
				curFacing = FaceDirection.Right;
                if (location.X < Console.WindowWidth - 1)
                {
                    if (!CheckCollision())
                    {
                        location = new EngineFunctions.COORD((short)(location.X + 1), location.Y);
                        if (location.X == Console.WindowWidth - 1 && transition)
                        {
                            roomExit = FaceDirection.Right;
                            transition = false;
                        }
                        else
                        {
                            transition = true;
                        }
                    }
                }
			}
			if (c == 'q')
			{
				SwitchItem();
			}
			if (c == ' ')
			{
				if (Game.gameOver)
				{
					Game.ResetGame = true;
					Game.gameOver = false;
					inputThread.Abort();
					inputThread.Join();
				}
				else
				{
					SwingSword = true;
				}
			}
            if ((int)(c) >= 49 && (int)(c) <= 52)
            {
                ShouldSave = true;
                SaveSlot = (byte)(c - 49);
            }
			if (c == 'e')
			{
                if (location.X > Console.WindowWidth / 2 - 2 && location.X <= Console.WindowWidth / 2 + 2 &&
                    location.Y == 1 && curFacing == FaceDirection.Up)
                {
                    UseKey = true;
                }
                else if (location.X > Console.WindowWidth / 2 - 2 && location.X <= Console.WindowWidth / 2 + 2 &&
                    location.Y == (Console.WindowHeight - 11) && curFacing == FaceDirection.Down)
                {
                    UseKey = true;
                }
                else if (location.Y > (Console.WindowHeight - 10) / 2 - 2 && location.Y <= (Console.WindowHeight - 10) / 2 + 2 &&
                    location.X == Console.WindowWidth - 1 && curFacing == FaceDirection.Right)
                {
                    UseKey = true;
                }
                else if (location.Y > (Console.WindowHeight - 10) / 2 - 2 && location.Y <= (Console.WindowHeight - 10) / 2 + 2 &&
                    location.X == 1 && curFacing == FaceDirection.Left)
                {
                    UseKey = true;
                }
                else
                {
                    UseItem();
                }
			}
			e = enemiesInRoom;
			c = 'n';
		}

		public void PlaceInNextRoom(FaceDirection exitDirection)
		{
			switch (exitDirection)
			{
				default:
				case FaceDirection.Up:
					location = new EngineFunctions.COORD(location.X, (short)(Console.WindowHeight - 10));
					break;
				case FaceDirection.Down:
					location = new EngineFunctions.COORD(location.X, (short)(0));
					break;
				case FaceDirection.Left:
					location = new EngineFunctions.COORD((short)(Console.WindowWidth - 1), location.Y);
					break;
				case FaceDirection.Right:
					location = new EngineFunctions.COORD(0, location.Y);
					break;
			}
			PlaceCharacterNewScreen();
		}
		public void PlaceInMapAfterShop(EngineFunctions.COORD loc)
		{
			location = loc;
		}
		public bool CheckCollision()
		{
			if (curFacing == FaceDirection.Up)
			{
				if (EngineFunctions.GetCharacterAtPosition(new EngineFunctions.COORD(location.X, (short)(location.Y - 1))) == '#')
				{
					return true;
				}
				else if (EngineFunctions.COLLISION_CHARS.Contains(EngineFunctions.screenBufferArray[location.Y - 1, location.X].AsciiChar))
				{
					return true;
				}
			}
			else if (curFacing == FaceDirection.Down)
			{
				if (EngineFunctions.GetCharacterAtPosition(new EngineFunctions.COORD(location.X, (short)(location.Y + 1))) == '#')
				{
					return true;
				}
				else if (EngineFunctions.COLLISION_CHARS.Contains(EngineFunctions.screenBufferArray[location.Y + 1, location.X].AsciiChar))
				{
					return true;
				}
			}
			else if (curFacing == FaceDirection.Left)
			{
				if (EngineFunctions.GetCharacterAtPosition(new EngineFunctions.COORD((short)(location.X - 1), location.Y)) == '#')
				{
					return true;
				}
				else if (EngineFunctions.COLLISION_CHARS.Contains(EngineFunctions.screenBufferArray[location.Y, location.X - 1].AsciiChar))
				{
					return true;
				}
			}
			else if (curFacing == FaceDirection.Right)
			{
				if (EngineFunctions.GetCharacterAtPosition(new EngineFunctions.COORD((short)(location.X + 1), location.Y)) == '#')
				{
					return true;
				}
				else if (EngineFunctions.COLLISION_CHARS.Contains(EngineFunctions.screenBufferArray[location.Y, location.X + 1].AsciiChar))
				{
					return true;
				}
			}
			return false;
			
		}
		public void PlaceCharacter()
		{
			switch (curFacing)
			{
				case FaceDirection.Up:
				default:
					EngineFunctions.DrawToConsole(location, '^', ConsoleColor.Green);
					break;
				case FaceDirection.Down:
					EngineFunctions.DrawToConsole(location, 'V', ConsoleColor.Green);
					break;
				case FaceDirection.Left:
					EngineFunctions.DrawToConsole(location, '<', ConsoleColor.Green);
					break;
				case FaceDirection.Right:
					EngineFunctions.DrawToConsole(location, '>', ConsoleColor.Green);
					break;
			}
		}
		public void PlaceCharacterNewScreen()
		{
			Console.SetCursorPosition(location.X, location.Y);
			switch (curFacing)
			{
				case FaceDirection.Up:
				default:
					EngineFunctions.DrawToConsole(location, '^', ConsoleColor.Green);
					break;
				case FaceDirection.Down:
					EngineFunctions.DrawToConsole(location, 'V', ConsoleColor.Green);
					break;
				case FaceDirection.Left:
					EngineFunctions.DrawToConsole(location, '<', ConsoleColor.Green);
					break;
				case FaceDirection.Right:
					EngineFunctions.DrawToConsole(location, '>', ConsoleColor.Green);
					break;
			}
		}
		public void DrawSword()
		{
			switch (Sword)
			{
				default:
				case SwordType.Wood:
					#region Starting Sword
					switch (curFacing)
					{
						case FaceDirection.Up:
						default:
							for (int y = 1; y <= 4; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y - y)), '|', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Down:
							for (int y = 1; y <= 4; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y + y)), '|', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Left:
							for (int x = 1; x <= 4; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X - x), location.Y), '-', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Right:
							for (int x = 1; x <= 4; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + x), location.Y), '-', ConsoleColor.DarkYellow);
							}
							break;
					}
					if (!SwordHit)
						CheckEnemySwordCollision();
					#endregion
					break;
				case SwordType.Short:
					#region Short Sword
					switch (curFacing)
					{
						case FaceDirection.Up:
						default:
							for (int y = 1; y <= 4; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y - y)), '|', ConsoleColor.Gray);
							}
							break;
						case FaceDirection.Down:
							for (int y = 1; y <= 4; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y + y)), '|', ConsoleColor.Gray);
							}
							break;
						case FaceDirection.Left:
							for (int x = 1; x <= 4; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X - x), location.Y), '-', ConsoleColor.Gray);
							}
							break;
						case FaceDirection.Right:
							for (int x = 1; x <= 4; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + x), location.Y), '-', ConsoleColor.Gray);
							}
							break;
					}
					if (!SwordHit)
						CheckEnemySwordCollision();
					#endregion
					break;
				case SwordType.Great:
					#region Great Sword
					switch (curFacing)
					{
						case FaceDirection.Up:
						default:
							for (int y = 1; y <= 5; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y - y)), '|', ConsoleColor.Cyan);
							}
							break;
						case FaceDirection.Down:
							for (int y = 1; y <= 5; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y + y)), '|', ConsoleColor.Cyan);
							}
							break;
						case FaceDirection.Left:
							for (int x = 1; x <= 5; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X - x), location.Y), '-', ConsoleColor.Cyan);
							}
							break;
						case FaceDirection.Right:
							for (int x = 1; x <= 5; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + x), location.Y), '-', ConsoleColor.Cyan);
							}
							break;
					}
					if (!SwordHit)
						CheckEnemySwordCollision();
					#endregion
					break;
			}
		}
		public void EraseSword()
		{
			switch (Sword)
			{
				default:
				case SwordType.Wood:
					#region Starting Sword
					switch (curFacing)
					{
						case FaceDirection.Up:
						default:
							for (int y = 1; y <= 4; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y - y)), '|', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Down:
							for (int y = 1; y <= 4; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y + y)), '|', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Left:
							for (int x = 1; x <= 4; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X - x), location.Y), '-', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Right:
							for (int x = 1; x <= 4; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + x), location.Y), '-', ConsoleColor.DarkYellow);
							}
							break;
					}
					#endregion
					break;
				case SwordType.Short:
					#region Short Sword
					switch (curFacing)
					{
						case FaceDirection.Up:
						default:
							for (int y = 1; y <= 4; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y - y)), '|', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Down:
							for (int y = 1; y <= 4; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y + y)), '|', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Left:
							for (int x = 1; x <= 4; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X - x), location.Y), '-', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Right:
							for (int x = 1; x <= 4; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + x), location.Y), '-', ConsoleColor.DarkYellow);
							}
							break;
					}
					#endregion
					break;
				case SwordType.Great:
					#region Great Sword
					switch (curFacing)
					{
						case FaceDirection.Up:
						default:
							for (int y = 1; y <= 5; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y - y)), ' ', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Down:
							for (int y = 1; y <= 5; y++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD(location.X, (short)(location.Y + y)), ' ', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Left:
							for (int x = 1; x <= 5; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X - x), location.Y), ' ', ConsoleColor.DarkYellow);
							}
							break;
						case FaceDirection.Right:
							for (int x = 1; x <= 5; x++)
							{
								EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(location.X + x), location.Y), ' ', ConsoleColor.DarkYellow);
							}
							break;
					}
					#endregion
					break;
			}
			SwordHit = false;
		}
		public void CheckEnemySwordCollision()
		{
			int length = 0;
			int dmg = 0;
			switch (Sword)
			{
				case SwordType.Short:
					length = 4;
					dmg = 4;
					break;
				default:
				case SwordType.Wood:
					length = 4;
					dmg = 2;
					break;
				case SwordType.Great:
					length = 5;
					dmg = 6;
					break;
			}
			foreach (Enemy e in enemiesInRoom)
			{
				//can't hit flying enemies with sword
				//if (e.enemytype == EType.Dragon || e.enemytype == EType.Glider) continue;
				switch (curFacing)
				{
					case FaceDirection.Up:
					default:
						if (e.loc.X == location.X && (e.loc.Y >= location.Y - length && e.loc.Y <= location.Y))
						{
							e.HP -= dmg;
							SwordHit = true;
						}
						break;
					case FaceDirection.Down:
						if (e.loc.X == location.X && (e.loc.Y >= location.Y && e.loc.Y <= location.Y + length))
						{
							e.HP -= dmg;
							SwordHit = true;
						}
						break;
					case FaceDirection.Left:
						if (e.loc.Y == location.Y && (e.loc.X <= location.X && e.loc.X >= location.X - length))
						{
							e.HP -= dmg;
							SwordHit = true;
						}
						break;
					case FaceDirection.Right:
						if (e.loc.Y == location.Y && (e.loc.X >= location.X && e.loc.X <= location.X + length))
						{
							e.HP -= dmg;
							SwordHit = true;
						}
						break;
				}
			}
		}
		private void SwitchItem()
		{
			if (CurItem == ItemType.Magic)
			{
				CurItem = ItemType.Bomb;
			}
			else if (CurItem == ItemType.Bomb)
			{
				CurItem = ItemType.Magic;
			}
		}
		private void UseItem()
		{
			switch (CurItem)
			{
				case ItemType.Bomb:
					PlaceBomb();
					break;
				case ItemType.Magic:
					UseMagic();
					break;
			}
		}
		private void PlaceBomb()
		{
			if (CurItem == ItemType.Bomb && !IsBombPlaced)
			{
				IsBombPlaced = true;
				bombLoc = location;
			}
		}
		private void UseMagic()
		{
			if (CurItem == ItemType.Magic && !IsMagicUsed)
			{
				IsMagicUsed = true;
			}
		}
	}
}
