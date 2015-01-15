using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

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
                saves[q] = path + "Save" + (q+1) + ".xml";
            }
        }

        public void SaveGame(int slot, HUD hudInfo, Player pInfo, Map m)
        {
            using (XmlWriter writer = new XmlTextWriter(new FileStream(saves[slot], FileMode.OpenOrCreate), Encoding.ASCII))
            {
                XmlSerializer s = new XmlSerializer(typeof(Player));
                s.Serialize(writer, pInfo);
            }
            //using (StreamWriter save = new StreamWriter(saves[slot], false, Encoding.ASCII, 400))
            //{
            //    ConsoleColor last = hudInfo.Spheres.Count != 0 ? hudInfo.Spheres[hudInfo.Spheres.Count - 1] : ConsoleColor.Black;
            //    save.WriteLine(DateTime.Now.ToString());
            //    save.WriteLine(last.ToString());
            //    save.WriteLine(hudInfo.Bombs.ToString());
            //    save.WriteLine(hudInfo.Keys.ToString());
            //    save.WriteLine(hudInfo.MagicAmt.ToString());
            //    save.WriteLine(hudInfo.Money.ToString());
            //    save.WriteLine(pInfo.location.X.ToString() + "," + pInfo.location.Y.ToString());
            //    save.WriteLine(pInfo.curFacing.ToString());
            //    save.WriteLine(pInfo.CurHP.ToString() + "/" + pInfo.HP.ToString());
            //    save.WriteLine(pInfo.Sword.ToString());
            //    save.WriteLine(room.X + "," + room.Y);
            //    foreach (int e in exploredRooms)
            //    {
            //        save.Write(e + " ");
            //    }
            //    save.Write("\n");
            //    foreach (Tuple<EngineFunctions.COORD, EngineFunctions.COORD> door in unlockedDoors)
            //    {
            //        save.WriteLine(door.Item1.X + "," + door.Item1.Y + " " + door.Item2.X + "," + door.Item2.Y);
            //    }
            //}
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
            //try
            //{
            //    using (StreamReader reader = new StreamReader(saves[slot]))
            //    {
            //        reader.ReadLine();
            //        hudInfo = GetMap(reader.ReadLine());
            //        hudInfo.Bombs = Byte.Parse(reader.ReadLine());
            //        hudInfo.Keys = Byte.Parse(reader.ReadLine());
            //        hudInfo.MagicAmt = Byte.Parse(reader.ReadLine());
            //        hudInfo.Money = Int32.Parse(reader.ReadLine());

            //        string pos = reader.ReadLine();
            //        pInfo = new Player(new EngineFunctions.COORD(Int16.Parse(pos.Substring(0, pos.IndexOf(','))), 
            //            Int16.Parse(pos.Substring(pos.IndexOf(',') + 1))));

            //        pInfo.curFacing = GetDirection(reader.ReadLine());

            //        string hp = reader.ReadLine();
            //        pInfo.CurHP = Int32.Parse(hp.Substring(0, hp.IndexOf('/')));
            //        pInfo.HP = Int32.Parse(hp.Substring(hp.IndexOf('/') + 1));

            //        pInfo.Sword = GetSword(reader.ReadLine());

            //        string mapLoc = reader.ReadLine();
            //        room = new EngineFunctions.COORD(Int16.Parse(mapLoc.Substring(0, mapLoc.IndexOf(','))), 
            //            Int16.Parse(mapLoc.Substring(mapLoc.IndexOf(',') + 1)));

            //        string[] expred = reader.ReadLine().Split(' ');
            //        exploredRooms = new List<int>();
            //        for (int i = 0; i < expred.GetLength(0) - 1; i++)
            //        {
            //            exploredRooms.Add(Int32.Parse(expred[i]));
            //        }

            //        unlockedDoors = new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>();
            //        string d = reader.ReadLine();
            //        while (d != null && !d.Equals(""))
            //        {
            //            string[] rooms = d.Split(' ');
            //            unlockedDoors.Add(new Tuple<EngineFunctions.COORD,EngineFunctions.COORD>(
            //                new EngineFunctions.COORD(Int16.Parse(rooms[0].Substring(0, rooms[0].IndexOf(','))), 
            //                    Int16.Parse(rooms[0].Substring(rooms[0].IndexOf(',') + 1))), 
            //                new EngineFunctions.COORD(Int16.Parse(rooms[1].Substring(0, rooms[1].IndexOf(','))), 
            //                    Int16.Parse(rooms[1].Substring(rooms[1].IndexOf(',') + 1)))));
            //            d = reader.ReadLine();
            //        }
            //    }
            //}
            //catch (IOException)
            //{

            //}
        }
    }
}
