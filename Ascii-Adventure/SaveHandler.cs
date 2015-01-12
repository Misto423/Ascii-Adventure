using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace ASCIIR2
{
    public class SaveHandler
    {
        private string[] saves;
        public SaveHandler()
        {
            saves = new string[4];
            string path = Assembly.GetCallingAssembly().Location;
            path = path.Remove(path.IndexOf("Ascii-Adventure.exe", StringComparison.CurrentCultureIgnoreCase));
            path += "Saves\\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            for (int q = 0; q < 4; q++)
            {
                saves[q] = path + "Save" + (q+1) + ".txt";
            }
        }

        public void SaveGame(int slot, HUD hudInfo, Player pInfo, List<int> exploredRooms, 
            EngineFunctions.COORD room, List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> unlockedDoors)
        {
            using (StreamWriter save = new StreamWriter(saves[slot], false, Encoding.ASCII, 400))
            {
                ConsoleColor last = hudInfo.Spheres.Count != 0 ? hudInfo.Spheres[hudInfo.Spheres.Count - 1] : ConsoleColor.Black;
                save.WriteLine(DateTime.Now.ToString());
                save.WriteLine(last.ToString());
                save.WriteLine(hudInfo.Bombs.ToString());
                save.WriteLine(hudInfo.Keys.ToString());
                save.WriteLine(hudInfo.MagicAmt.ToString());
                save.WriteLine(hudInfo.Money.ToString());
                save.WriteLine(pInfo.location.X.ToString() + "," + pInfo.location.Y.ToString());
                save.WriteLine(pInfo.curFacing.ToString());
                save.WriteLine(pInfo.CurHP.ToString() + "/" + pInfo.HP.ToString());
                save.WriteLine(pInfo.Sword.ToString());
                save.WriteLine(room.X + "," + room.Y);
                foreach (int e in exploredRooms)
                {
                    save.Write(e + " ");
                }
                save.Write("\n");
                foreach (Tuple<EngineFunctions.COORD, EngineFunctions.COORD> door in unlockedDoors)
                {
                    save.WriteLine(door.Item1.X + "," + door.Item1.Y + " " + door.Item2.X + "," + door.Item2.Y);
                }
            }
        }

        public List<string> GetSaveList()
        {
            List<string> saveInfo = new List<string>();

            for (int i = 0; i <= 3; i++)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(saves[i]))
                    {
                        string date = reader.ReadLine();
                        string dungeon = reader.ReadLine();
                        saveInfo.Add((i + 1) + ": " + date + "   " + dungeon + " Dungeon");
                    }
                }
                catch (IOException)
                {
                    saveInfo.Add((i + 1) + ": No Save");
                }
            }

            return saveInfo;
        }

        public void LoadGame(int slot, ref HUD hudInfo, ref Player pInfo, ref List<int> exploredRooms,
            ref EngineFunctions.COORD room, ref List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> unlockedDoors)
        {
            try
            {
                using (StreamReader reader = new StreamReader(saves[slot]))
                {
                    reader.ReadLine();
                    hudInfo = GetMap(reader.ReadLine());
                    hudInfo.Bombs = Byte.Parse(reader.ReadLine());
                    hudInfo.Keys = Byte.Parse(reader.ReadLine());
                    hudInfo.MagicAmt = Byte.Parse(reader.ReadLine());
                    hudInfo.Money = Int32.Parse(reader.ReadLine());

                    string pos = reader.ReadLine();
                    pInfo = new Player(new EngineFunctions.COORD(Int16.Parse(pos.Substring(0, pos.IndexOf(','))), 
                        Int16.Parse(pos.Substring(pos.IndexOf(',') + 1))));

                    pInfo.curFacing = GetDirection(reader.ReadLine());

                    string hp = reader.ReadLine();
                    pInfo.CurHP = Int16.Parse(pos.Substring(0, pos.IndexOf('/')));
                    pInfo.HP = Int16.Parse(pos.Substring(pos.IndexOf('/')));

                    pInfo.Sword = GetSword(reader.ReadLine());

                    string mapLoc = reader.ReadLine();
                    room = new EngineFunctions.COORD(Int16.Parse(mapLoc.Substring(0, mapLoc.IndexOf(','))), 
                        Int16.Parse(mapLoc.Substring(mapLoc.IndexOf(',') + 1));

                    string[] expred = reader.ReadLine().Split(' ');
                    exploredRooms = new List<int>();
                    for (int i = 0; i < expred.GetLength(0); i++)
                    {
                        exploredRooms.Add(Int32.Parse(expred[i]));
                    }

                    unlockedDoors = new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>();
                    string d = reader.ReadLine();
                    while (!d.Equals(""))
                    {
                        string[] rooms = d.Split(' ');
                        unlockedDoors.Add(new Tuple<EngineFunctions.COORD,EngineFunctions.COORD>(
                            new EngineFunctions.COORD(Int16.Parse(rooms[0].Substring(0, rooms[0].IndexOf(','))), 
                                Int16.Parse(rooms[0].Substring(rooms[0].IndexOf(',') + 1))), 
                            new EngineFunctions.COORD(Int16.Parse(rooms[1].Substring(0, rooms[1].IndexOf(','))), 
                                Int16.Parse(rooms[1].Substring(rooms[1].IndexOf(',') + 1)))));
                        d = reader.ReadLine();
                    }
                }
            }
            catch (IOException)
            {

            }
        }

        private FaceDirection GetDirection(string f)
        {
            if (f.Equals("UP", StringComparison.CurrentCultureIgnoreCase))
                return FaceDirection.Up;
            else if (f.Equals("DOWN", StringComparison.CurrentCultureIgnoreCase))
                return FaceDirection.Down;
            else if (f.Equals("LEFT", StringComparison.CurrentCultureIgnoreCase))
                return FaceDirection.Left;
            else
                return FaceDirection.Right;
        }
        private SwordType GetSword(string s)
        {
            if (s.Equals("WOOD", StringComparison.CurrentCultureIgnoreCase))
                return SwordType.Wood;
            else if (s.Equals("SHORT", StringComparison.CurrentCultureIgnoreCase))
                return SwordType.Short;
            else
                return SwordType.Great;
        }

        private HUD GetMap(string color)
        {
            List<ConsoleColor> spheres = new List<ConsoleColor>() 
                {ConsoleColor.Gray, ConsoleColor.Red, ConsoleColor.DarkBlue, ConsoleColor.Yellow, ConsoleColor.Cyan,
                 ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.DarkMagenta, ConsoleColor.White};
            HUD hud = new HUD();
            switch (color)
            {
                default:
                case "Black":
                    break;
                case "Red":
                    hud.Spheres.AddRange(spheres.GetRange(0, 1));
                    break;
                case "DarkBlue":
                    hud.Spheres.AddRange(spheres.GetRange(0, 2));
                    break;
                case "Yellow":
                    hud.Spheres.AddRange(spheres.GetRange(0, 3));
                    break;
                case "Cyan":
                    hud.Spheres.AddRange(spheres.GetRange(0, 4));
                    break;
                case "Blue":
                    hud.Spheres.AddRange(spheres.GetRange(0, 5));
                    break;
                case "Green":
                    hud.Spheres.AddRange(spheres.GetRange(0, 6));
                    break;
                case "Purple":
                    hud.Spheres.AddRange(spheres.GetRange(0, 7));
                    break;
                case "White":
                    hud.Spheres.AddRange(spheres.GetRange(0, 8));
                    break;
            }

            return hud;
        }
    }
}
