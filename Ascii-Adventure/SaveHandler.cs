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
                save.WriteLine(pInfo.curFacing.ToString());
                save.WriteLine(pInfo.CurHP.ToString() + "/" + pInfo.HP.ToString());
                save.WriteLine(pInfo.location.X.ToString() + "," + pInfo.location.Y.ToString());
                save.WriteLine(pInfo.Sword.ToString());
                save.WriteLine(room.X + "," + room.Y);
                foreach (Tuple<EngineFunctions.COORD, EngineFunctions.COORD> door in unlockedDoors)
                {
                    save.WriteLine(door.Item1.X + "," + door.Item1.Y + " " + door.Item2.X + "," + door.Item2.Y);
                }
                foreach (int e in exploredRooms)
                {
                    save.Write(e + " ");
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

        public void LoadGame()
        {


        }
    }
}
