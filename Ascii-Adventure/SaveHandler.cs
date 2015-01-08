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
            for (int q = 0; q < 4; q++)
            {
                saves[q] = path + "Saves/Save" + (q+1) + ".txt";
            }
        }

        public void SaveGame(int slot, HUD hudInfo, Player pInfo)
        {
            using (StreamWriter save = new StreamWriter(saves[slot], false, Encoding.ASCII, 150))
            {
                ConsoleColor last = hudInfo.Spheres.Count != 0 ? hudInfo.Spheres[hudInfo.Spheres.Count - 1] : ConsoleColor.Black;
                string date = DateTime.Now.ToString() + "\n";
                string hInfo = hudInfo.Bombs.ToString() + "\n" + hudInfo.Keys.ToString() + "\n" +
                    hudInfo.MagicAmt.ToString() + "\n" + hudInfo.Money.ToString() + "\n" +
                    last.ToString() + "\n";
                string player = pInfo.curFacing.ToString() + "\n" + pInfo.CurHP.ToString() +
                    "/" + pInfo.HP.ToString() + "\n" + pInfo.location.X.ToString() + "," + pInfo.location.Y.ToString() +
                    "\n" + pInfo.Sword.ToString();
                save.Write(date + hInfo + player);
            }
        }
    }
}
