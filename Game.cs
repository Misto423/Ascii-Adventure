using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ASCIIR2
{
	public enum Tone
	{
		GbelowC = 196,
		A = 220,
		Asharp = 233,
		B = 247,
		C = 262,
		Csharp = 277,
		D = 294,
		Dsharp = 311,
		E = 330,
		F = 349,
		Fsharp = 370,
		G = 392,
		Gsharp = 415,
	}

	// Define the duration of a note in units of milliseconds. 
	public enum Duration
	{
		WHOLE = 1600,
		HALF = WHOLE / 2,
		QUARTER = HALF / 2,
		EIGHTH = QUARTER / 2,
		SIXTEENTH = EIGHTH / 2,
	}

	public class Game
	{
		#region Vars
		public static Random rand = new Random();
        public static Player player { get; set; }
        public Map m { get; set; }
		private HUD hud;

		private Menu startMenu;
		public static bool gameOver = false;
		public static bool ResetGame = false;

		private Shop shop;
		public bool inShop = false;

		private List<Enemy> enemies;
		private List<Item> itemsOnMap;
		private FaceDirection mapRoomTransition = FaceDirection.noDir;
		private int RoomsExplored = 0;

		private bool levelTransition;
		private bool setBg = true;
		private ParticleHandler endLevelParticles;

		private Stopwatch swordTimer = new Stopwatch();
		private Stopwatch bombTimer = new Stopwatch();
		private Stopwatch magicTimer = new Stopwatch();
		private bool explosion = false;
		private bool flashing = false;
		private TimeSpan bombFlashTimer = DateTime.Now.TimeOfDay;
		private Thread musicThread;
		#endregion

		public Game()
        {
			Console.SetWindowSize(80, 30);
			EngineFunctions.AdjustColorPalette();

			startMenu = new Menu();

			musicThread = new Thread(new ThreadStart(PlayMusic));
			musicThread.IsBackground = true;
			//musicThread.Start();
			//while (!musicThread.IsAlive) ;
        }

		private void LoadGame()
		{
			player = new Player(new EngineFunctions.COORD(1,1));
			m = new Map(new EngineFunctions.COORD(0,0));
			RoomsExplored = 1;
            hud = new HUD();
			shop = new Shop();
            enemies = new List<Enemy>();
            itemsOnMap = new List<Item>();
		}

		private void PlayMusic()
		{
			while (true)
			{
				if (startMenu == null)
				{
					if (player.CurHP / (float)player.HP <= .4)
					{
						Console.Beep((int)Tone.C, (int)Duration.QUARTER);

						Console.Beep((int)Tone.Gsharp, (int)Duration.EIGHTH);
						Console.Beep((int)Tone.Gsharp, (int)Duration.EIGHTH);

						Console.Beep((int)Tone.C, (int)Duration.QUARTER);

						Console.Beep((int)Tone.Gsharp, (int)Duration.EIGHTH);
						Console.Beep((int)Tone.Gsharp, (int)Duration.EIGHTH);
					}
					else
					{
						//Console.Beep((int)Tone.Fsharp, (int)Duration.EIGHTH);
						//Console.Beep((int)Tone.Fsharp, (int)Duration.EIGHTH);
						//Console.Beep((int)Tone.A, (int)Duration.EIGHTH);
						//Console.Beep((int)Tone.B, (int)Duration.EIGHTH);
						//Console.Beep((int)Tone.A, (int)Duration.EIGHTH);
						//Console.Beep((int)Tone.D, (int)Duration.EIGHTH);
						//Console.Beep((int)Tone.Fsharp, (int)Duration.EIGHTH);
						//Console.Beep((int)Tone.D, (int)Duration.EIGHTH);

						Console.Beep((int)Tone.C, (int)Duration.HALF);
						Console.Beep((int)Tone.Asharp, (int)Duration.QUARTER);
						Console.Beep((int)Tone.F, (int)Duration.HALF);
						Console.Beep((int)Tone.G, (int)Duration.QUARTER);

						Console.Beep((int)Tone.G, (int)Duration.HALF);
						Console.Beep((int)Tone.E, (int)Duration.QUARTER);
						Console.Beep((int)Tone.C, (int)Duration.HALF);
						Console.Beep((int)Tone.D, (int)Duration.HALF);

						Console.Beep((int)Tone.E, (int)Duration.HALF);
						Console.Beep((int)Tone.D, (int)Duration.QUARTER);
						Console.Beep((int)Tone.G, (int)Duration.QUARTER);
						Console.Beep((int)Tone.D, (int)Duration.HALF);
					}
				}
				else //menu music
				{

				}
			}
		}

		public void Start()
		{
			while (true)
			{
				if (ResetGame)
				{
					ResetGame = false;
					startMenu = new Menu();
				}

				EngineFunctions.GetScreenBuffer();
				DrawScreen();
				EngineFunctions.DrawToBuffer();

				if (startMenu != null)
				{
					int sel = startMenu.UpdateMenu();
					switch (sel)
					{
						case 0:
							startMenu = null;
							LoadGame();
							break;
						case 1:

							break;
						case 2:
							Program.CloseGame();
							break;
					}
				}
				else
				{
					if (gameOver)
					{

					}
					else
					{
                        if (player.ShouldSave)
                        {
                            SaveGame(player.SaveSlot);
                            player.ShouldSave = false;
                        }

						if (inShop)
						{
							#region Shop Stuff
							if (mapRoomTransition != FaceDirection.noDir)
							{
								player.PlaceInMapAfterShop(new EngineFunctions.COORD((short)(shop.location.X + 2), (short)(shop.location.Y + 4)));
								inShop = false;
								shop.IsShopVisible = false;
								shop.CanBuy = true;
							}
							if (shop.CanBuy)
							{
								if (player.location.X == 31 && player.location.Y == 11)
								{
                                    if (hud.Money >= shop.GetPrice(shop.itemsForSale[0]))
                                    {
                                        BuyItem(0);
                                        shop.CanBuy = false;
                                    }
								}
								else if (player.location.X == 41 && player.location.Y == 11)
								{
                                    if (hud.Money >= shop.GetPrice(shop.itemsForSale[1]))
                                    {
                                        BuyItem(1);
                                        shop.CanBuy = false;
                                    }
								}
								else if (player.location.X == 51 && player.location.Y == 11)
								{
                                    if (hud.Money >= shop.GetPrice(shop.itemsForSale[2]))
                                    {
                                        BuyItem(2);
                                        shop.CanBuy = false;
                                    }
								}
							}
							#endregion
						}
						else
						{
							#region Items
							int rIndex = -1;
							for (int item = 0; item < itemsOnMap.Count; item++)
							{
								if (player.location.Equals(itemsOnMap[item].Location))
								{
									if (itemsOnMap[item].IType == ItemType.Sphere)
									{
										hud.Spheres.Add(Map.CurLevelInfo.Key);
										player.CurHP = player.HP;
										//new level
										levelTransition = true;
										endLevelParticles = new ParticleHandler(Map.CurLevelInfo.Value);
										endLevelParticles.SpawnParticles();
									}
									else
									{
										hud.AddItem(itemsOnMap[item].IType);
										if (itemsOnMap[item].IType == ItemType.Heart)
										{
											if (player.CurHP + 4 > player.HP)
											{
												player.CurHP = player.HP;
											}
											else
											{
												player.CurHP += 4;
											}
										}
									}
									rIndex = item;
									break;
								}
							}
							if (rIndex >= 0)
							{
								itemsOnMap.RemoveAt(rIndex);
								rIndex = -1;
							}
							#endregion

							#region Enemies
							enemies = m.getRoom().enemies;
							foreach (Enemy enemy in enemies)
							{
								if (enemy.HP <= 0)
								{
									if (enemy.enemytype == EType.Dragon)
									{
										itemsOnMap.Add(new Item(ItemType.Sphere, enemy.loc));
									}
									else if (enemies.Count == 1)
									{
										itemsOnMap.Add(new Item(ItemType.Key, enemy.loc));
									}
									else
									{
										int i = rand.Next(0, 100);
										if (i >= 20)
										{
											i = rand.Next(0, 100);
											if (i > 80)
											{
												itemsOnMap.Add(new Item(ItemType.Heart, enemy.loc));
											}
											else if (i > 35)
											{
												itemsOnMap.Add(new Item(ItemType.Crystal, enemy.loc));
											}
											else if (i > 30)
											{
												itemsOnMap.Add(new Item(ItemType.Key, enemy.loc));
											}
											else if (i > 10)
											{
												itemsOnMap.Add(new Item(ItemType.Magic, enemy.loc));
											}
											else
											{
												itemsOnMap.Add(new Item(ItemType.Bomb, enemy.loc));
											}
										}
									}
									enemy.Killed();
									enemies.Remove(enemy);
									break;
								}
							}
							m.getRoom().enemies = enemies;
							#endregion

							#region Collision Checking
							CheckEnemyCollision();
							if (player.TookDamage)
							{
								player.TookDamage = false;
								hud.ClearHealth(player.HP);
								hud.DrawHealth(player.CurHP);
							}
							#endregion

							#region Change Rooms
							if (mapRoomTransition != FaceDirection.noDir)
							{
								m.MoveToNextRoom(mapRoomTransition);
								player.PlaceInNextRoom(mapRoomTransition);
								shop.ShowShop(player.Sword, player.HP);
								itemsOnMap.Clear();

								Room r = m.getRoom();
								if (!r.IsExplored)
								{
									r.IsExplored = true;
									RoomsExplored++;
								}
								if (RoomsExplored == m.GetRoomCount())
								{
									r.enemies.Add(new Enemy(EType.Dragon));
								}
								m.SetRoom(r);
							}
							#endregion

							#region Enter Shop
							if (shop.IsShopVisible &&
								player.location.Equals(new EngineFunctions.COORD((short)(shop.location.X + 2), (short)(shop.location.Y + 3))))
							{
								inShop = true;
								player.PlaceInMapAfterShop(new EngineFunctions.COORD((short)(Console.WindowWidth / 2), 20));
							}
							#endregion

                            #region Unlock Door
                            if (player.UseKey)
                            {
                                if (hud.Keys > 0)
                                {
                                    hud.Keys--;
                                    player.UseKey = false;
                                    if (player.location.X == 1)
                                    {
                                        m.UnlockDoor(new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(
                                            m.roomLocation, 
                                            new EngineFunctions.COORD((short)(m.roomLocation.X - 1), m.roomLocation.Y)));
                                    }
                                    else if (player.location.X == Console.WindowWidth - 2)
                                    {
                                        m.UnlockDoor(new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(
                                            m.roomLocation,
                                            new EngineFunctions.COORD((short)(m.roomLocation.X + 1), m.roomLocation.Y)));
                                    }
                                    else if (player.location.Y == 1)
                                    {
                                        m.UnlockDoor(new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(
                                            m.roomLocation,
                                            new EngineFunctions.COORD(m.roomLocation.X, (short)(m.roomLocation.Y - 1))));
                                    }
                                    else if (player.location.Y == Console.WindowHeight - 11)
                                    {
                                        m.UnlockDoor(new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(
                                            m.roomLocation,
                                            new EngineFunctions.COORD(m.roomLocation.X, (short)(m.roomLocation.Y + 1))));
                                    }
                                }
                            }
                            #endregion
                        }
					}                 
					player.UpdatePlayer(ref enemies, out mapRoomTransition, m.GetLockedDoors());
				}
			}
		}

		public void CheckEnemyCollision()
		{
			foreach (Enemy e in enemies)
			{
				//push player away from enemy
				if (e.loc.Equals(new EngineFunctions.COORD((short)(player.location.X - 1), player.location.Y)))
				{
					EngineFunctions.DrawToConsole(player.location, ' ');
					if (player.location.X + 4 >= Console.WindowWidth - 1)
					{
						player.location = new EngineFunctions.COORD((short)(Console.WindowWidth - 2), player.location.Y);
					}
					else
					{
						player.location = new EngineFunctions.COORD((short)(player.location.X + 4), player.location.Y);
					} 
                    player.TookDamage = true;
					player.CurHP -= e.Damage;
				}
				else if (e.loc.Equals(new EngineFunctions.COORD((short)(player.location.X + 1), player.location.Y)))
				{
					EngineFunctions.DrawToConsole(player.location, ' ');
					if (player.location.X - 4 <= 0)
					{
						player.location = new EngineFunctions.COORD((short)(1), player.location.Y);
					}
					else
					{
						player.location = new EngineFunctions.COORD((short)(player.location.X - 4), player.location.Y);
					}
					player.TookDamage = true;
					player.CurHP -= e.Damage;
				}
				else if (e.loc.Equals(new EngineFunctions.COORD(player.location.X, (short)(player.location.Y - 1))))
				{
					EngineFunctions.DrawToConsole(player.location, ' ');
					if (player.location.Y + 4 >= Console.WindowHeight - 11)
					{
						player.location = new EngineFunctions.COORD(player.location.X, (short)(Console.WindowHeight - 12));
					}
					else
					{
						player.location = new EngineFunctions.COORD(player.location.X, (short)(player.location.Y + 4));
					} player.TookDamage = true;
					player.CurHP -= e.Damage;
				}
				else if (e.loc.Equals(new EngineFunctions.COORD(player.location.X, (short)(player.location.Y + 1))))
				{
					EngineFunctions.DrawToConsole(player.location, ' ');
					if (player.location.Y - 4 <= 0)
					{
						player.location = new EngineFunctions.COORD(player.location.X, (short)(1));
					}
					else
					{
						player.location = new EngineFunctions.COORD(player.location.X, (short)(player.location.Y - 4));
					}
					player.TookDamage = true;
					player.CurHP -= e.Damage;
				}
			}
			if (player.CurHP <= 0)
			{
				gameOver = true;
			}
			player.PlaceCharacter();
		}

		public void DrawScreen()
		{
			if (startMenu != null)
			{
				startMenu.DrawMenu();
			}
			else
			{
				if (gameOver)
				{
					DrawGameOver();
				}
				else
				{
					if (levelTransition)
					{
						DrawMapTransition();
					}
					else
					{
						if (inShop)
						{
							shop.DrawShop();
						}
						else
						{
							m.DrawRoom();

							foreach (Enemy e in enemies)
							{
								e.MoveEnemy();
							}

							foreach (Item item in itemsOnMap)
							{
								item.DrawItem();
							}

							if (shop.IsShopVisible)
							{
								shop.DrawShopOnMap();
							}

							DrawSword();
							if (player.IsBombPlaced || explosion)
							{
								if (hud.Bombs > 0)
									DrawBomb();
								else player.IsBombPlaced = false;
							}
							if (player.IsMagicUsed)
							{
								if (hud.MagicAmt > 0)
									DrawMagic();
								else player.IsMagicUsed = false;
							}
						}
					}
					hud.DrawDivider();
					hud.DrawSword(player.Sword);
					hud.DrawItem(player.CurItem);
					hud.DrawHealth(player.CurHP);
					hud.DrawItemAmounts();
					hud.DrawSpheres();
					player.PlaceCharacter();
				}
			}
		}

		private void DrawSword()
		{
			if (player.SwingSword && swordTimer.ElapsedMilliseconds >= 250)
			{
				player.EraseSword();
				player.SwingSword = false;
				swordTimer.Stop();
				swordTimer.Reset();
			}
			if (player.SwingSword)
			{
				player.DrawSword();
				swordTimer.Start();
			}
		}
		private void DrawBomb()
		{
			if (player.IsBombPlaced && bombTimer.ElapsedMilliseconds >= 2500)
			{
				bombTimer.Stop();
				bombTimer.Reset();
				EngineFunctions.DrawToConsole(player.bombLoc, ' ', ConsoleColor.DarkBlue);
				player.IsBombPlaced = false;
				explosion = true;
				bombTimer.Start();
			}
			if (player.IsBombPlaced && bombTimer.ElapsedMilliseconds <= 1500)
			{
				bombTimer.Start();
				EngineFunctions.DrawToConsole(player.bombLoc, '@', ConsoleColor.DarkBlue);
			}
			else if (player.IsBombPlaced && bombTimer.ElapsedMilliseconds > 1500)
			{
				EngineFunctions.DrawToConsole(player.bombLoc, '@', flashing ? ConsoleColor.Red : ConsoleColor.DarkBlue);
				if (DateTime.Now.TimeOfDay.Subtract(bombFlashTimer) > TimeSpan.FromMilliseconds(100))
				{
					flashing = !flashing;
					bombFlashTimer = DateTime.Now.TimeOfDay;
				}
			}

			if (explosion && bombTimer.ElapsedMilliseconds <= 150)
			{
				if (player.bombLoc.X - 3 > 0)
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.bombLoc.X - 3), (short)(player.bombLoc.Y)), '*', ConsoleColor.DarkBlue);
				if (player.bombLoc.X - 2 > 0 && player.bombLoc.Y + 1 < Console.WindowHeight - 10)
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.bombLoc.X - 2), (short)(player.bombLoc.Y + 1)), '*', ConsoleColor.DarkBlue);
				if (player.bombLoc.Y + 3 < Console.WindowHeight - 10)
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.bombLoc.X), (short)(player.bombLoc.Y + 3)), '*', ConsoleColor.DarkBlue);
				if (player.bombLoc.X + 2 < Console.WindowWidth - 1 && player.bombLoc.Y + 1 < Console.WindowHeight - 10)
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.bombLoc.X + 2), (short)(player.bombLoc.Y + 1)), '*', ConsoleColor.DarkBlue);
				if (player.bombLoc.X + 3 < Console.WindowWidth - 1)
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.bombLoc.X + 3), (short)(player.bombLoc.Y)), '*', ConsoleColor.DarkBlue);
				if (player.bombLoc.X + 2 < Console.WindowWidth - 1 && player.bombLoc.Y - 1 > 0)
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.bombLoc.X + 2), (short)(player.bombLoc.Y - 1)), '*', ConsoleColor.DarkBlue);
				if (player.bombLoc.Y - 3 > 0)
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.bombLoc.X), (short)(player.bombLoc.Y - 3)), '*', ConsoleColor.DarkBlue);
				if (player.bombLoc.X - 2 > 0 && player.bombLoc.Y - 1 > 0)
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.bombLoc.X - 2), (short)(player.bombLoc.Y - 1)), '*', ConsoleColor.DarkBlue);
				EngineFunctions.DrawToConsole(new EngineFunctions.COORD(player.bombLoc.X, player.bombLoc.Y), '*', ConsoleColor.DarkBlue);
			}
			else if (explosion && bombTimer.ElapsedMilliseconds >= 150)
			{
				bombTimer.Stop();
				bombTimer.Reset();
				explosion = false;
				hud.Bombs--;
				BombDamage();
			}
		}
		private void DrawMagic()
		{
			if (player.IsMagicUsed && magicTimer.ElapsedMilliseconds >= 250)
			{
				//eraseSword
				DrawMagicToScreen(true);
				player.IsMagicUsed = false;
				magicTimer.Stop();
				magicTimer.Reset();
				MagicDamage();
				hud.MagicAmt--;
			}
			if (player.IsMagicUsed)
			{
				//draw Magic
				DrawMagicToScreen(false);
				magicTimer.Start();
			}
		}
		private void DrawMagicToScreen(bool erase)
		{
				switch (player.curFacing)
				{
					case FaceDirection.Up:
					default:
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X - 2), (short)(player.location.Y - 4)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X), (short)(player.location.Y - 4)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X + 2), (short)(player.location.Y - 4)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X - 1), (short)(player.location.Y - 2)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X), (short)(player.location.Y - 2)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X + 1), (short)(player.location.Y - 2)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						break;
					case FaceDirection.Down:
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X - 2), (short)(player.location.Y + 4)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X), (short)(player.location.Y + 4)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X + 2), (short)(player.location.Y + 4)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X - 1), (short)(player.location.Y + 2)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X), (short)(player.location.Y + 2)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X + 1), (short)(player.location.Y + 2)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						break;
					case FaceDirection.Left:
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X - 4), (short)(player.location.Y - 2)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X - 4), (short)(player.location.Y)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X - 4), (short)(player.location.Y + 2)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X - 2), (short)(player.location.Y - 1)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X - 2), (short)(player.location.Y)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X - 2), (short)(player.location.Y + 1)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						break;
					case FaceDirection.Right:
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X + 4), (short)(player.location.Y - 2)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X + 4), (short)(player.location.Y)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X + 4), (short)(player.location.Y + 2)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X + 2), (short)(player.location.Y - 1)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X + 2), (short)(player.location.Y)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(player.location.X + 2), (short)(player.location.Y + 1)),
							erase ? ' ' : '*', ConsoleColor.DarkCyan);
						break;
				}
		}
		public void MagicDamage()
		{
			switch (player.curFacing)
			{
				case FaceDirection.Up:
					for (int x = player.location.X - 2; x <= player.location.X + 2; x++)
					{
						for (int y = player.location.Y - 4; y < player.location.Y; y++)
						{
							ItemDamage((short)x, (short)y);
						}
					}
					break;
				case FaceDirection.Down:
					for (int x = player.location.X - 2; x <= player.location.X + 2; x++)
					{
						for (int y = player.location.Y + 4; y > player.location.Y; y--)
						{
							ItemDamage((short)x, (short)y);
						}
					}
					break;
				case FaceDirection.Left:
					for (int x = player.location.X - 4; x <= player.location.X; x++)
					{
						for (int y = player.location.Y - 2; y < player.location.Y + 2; y++)
						{
							ItemDamage((short)x, (short)y);
						}
					}
					break;
				case FaceDirection.Right:
					for (int x = player.location.X + 4; x >= player.location.X; x--)
					{
						for (int y = player.location.Y - 2; y < player.location.Y + 2; y++)
						{
							ItemDamage((short)x, (short)y);
						}
					}
					break;
			}
		}
		public void BombDamage()
		{
			for (int x = player.bombLoc.X - 3; x <= player.bombLoc.X + 3; x++)
			{
				for (int y = player.bombLoc.Y - 3; y <= player.bombLoc.Y + 3; y++)
				{
					if (x > 0 && x < Console.WindowWidth - 1 && y > 0 && y < Console.WindowHeight - 10)
					{
						if (EngineFunctions.screenBufferArray[y, x].AsciiChar == '#')
						{
							Room r = m.getRoom();
							r.destroyed.Add(new EngineFunctions.COORD((short)x, (short)y));
							m.SetRoom(r);
						}
						else if (player.location.Equals(new EngineFunctions.COORD((short)x, (short)y)))
						{
							player.CurHP -= 4;
						}

						ItemDamage((short)x, (short)y);
					}
				}
			}
		}
		private void ItemDamage(short x, short y)
		{
			foreach (Enemy e in enemies)
			{
				if (e.loc.Equals(new EngineFunctions.COORD(x, y)))
				{
					e.HP -= 4;
				}
			}
		}
		public void BuyItem(int index)
		{
			if (hud.Money >= shop.prices[index])
			{
				hud.Money -= shop.prices[index];
				switch (shop.itemsForSale[index])
				{
					case ItemType.Bomb:
					default:
						hud.Bombs += 4;
						break;
					case ItemType.Heart:
                        if (player.CurHP < player.HP - 4)
                        {
                            player.CurHP += 4;
                        }
                        else
                        {
                            player.CurHP = player.HP;
                        }
						break;
					case ItemType.HeartPiece:
                        if (player.HP <= 28)
                        {
                            player.HP += 2;
                        }
						player.CurHP = player.HP;
						break;
					case ItemType.Key:
						hud.Keys++;
						break;
					case ItemType.Magic:
						hud.MagicAmt += 2;
						break;
					case ItemType.Short:
						player.Sword = SwordType.Short;
						break;
					case ItemType.Great:
						player.Sword = SwordType.Great;
						break;
				}
			}
		}
		private void DrawMapTransition()
		{
			if (setBg)
			{
				EngineFunctions.SetBackgroundColor(Map.CurLevelInfo.Key);
				setBg = false;
			}
			endLevelParticles.UpdateParticles();
			if (endLevelParticles.NoParticles)
			{
				setBg = true;
				m.ChangeMap();
				levelTransition = false;
				RoomsExplored = 1;
				player.location = new EngineFunctions.COORD(1, 1);
			}
		}
		private void DrawGameOver()
		{
			string gameLine1 = @" /¯¯¯¯/|¯¯¯¯|   /¯¯¯¯/-\¯¯¯¯\   \¯¯¯¯\     /¯¯¯¯/  /¯¯¯¯/\¯¯¯¯\ ";
			string gameLine2 = @" |.: | |____|   |..::|_|::. |   |  .:||¯¯¯||.:: |  \____\_¯¯¯¯  ";
			string gameLine3 = @" |:::| \¯¯¯¯\   |.:::|¯|:::.|   |.:::|'\_/'|:::.|  /¯¯¯¯/¯____";
			string gameLine4 = @" \____\/____/   |____| |____|  /____/|\_'_/|\____\ \____\/____/ ";

			string overLine1 = @"  /¯¯¯/¯\¯¯¯\   |¯¯¯¯| |¯¯¯¯|  /¯¯¯¯/\¯¯¯¯\ |¯¯¯¯|\¯¯¯¯\  ";
			string overLine2 = @" |::..| |..::|  |.::.| |.::.|  \____\_¯¯¯¯  |.:::|/____/ ";
			string overLine3 = @" |.:::| |::: |  |::. | |.:::|  /¯¯¯¯/¯____  |::: |\¯¯¯¯\";
			string overLine4 = @"  \___\_/___/    \___\_/___/   \____\/____/ |____| |____| ";

			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(9, 6), gameLine1, ConsoleColor.Red);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(9, 7), gameLine2, ConsoleColor.Red);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(9, 8), gameLine3, ConsoleColor.Red);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(9, 9), gameLine4, ConsoleColor.Red);

			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(12, 11), overLine1, ConsoleColor.Red);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(12, 12), overLine2, ConsoleColor.Red);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(12, 13), overLine3, ConsoleColor.Red);
			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(12, 14), overLine4, ConsoleColor.Red);

			EngineFunctions.DrawToConsole(new EngineFunctions.COORD(35, 20), "PRESS SPACE...", ConsoleColor.White);
		}
        private void SaveGame(int slot)
        {
            Program.saver.SaveGame(slot, hud, player);
        }
	}
}
