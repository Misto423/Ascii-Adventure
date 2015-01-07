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
        private FileStream[] saves;
        public SaveHandler()
        {
            saves = new FileStream[4];
            string path = Assembly.GetCallingAssembly().Location;
            path = path.Remove(path.IndexOf("asciir2.exe",StringComparison.CurrentCultureIgnoreCase));
            for (int q = 0; q < 4; q++)
            {
                saves[q] = new FileStream(path + "save" + (q+1) + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
        }

        public void SaveGame(int slot, HUD hudInfo, Player pInfo)
        {
            byte[] total; 
            byte[] date = Encoding.ASCII.GetBytes(DateTime.Now.ToFileTime().ToString() + "\n");
            byte[] hInfo = Encoding.ASCII.GetBytes(hudInfo.Bombs.ToString() + "\n" + hudInfo.Keys.ToString() + "\n" +
                hudInfo.MagicAmt.ToString() + "\n" + hudInfo.Money.ToString() + "\n" + hudInfo.Spheres.ToString() + "\n");
            byte[] player = Encoding.ASCII.GetBytes(pInfo.curFacing.ToString() + "\n" + pInfo.CurHP.ToString() +
                "/" + pInfo.HP.ToString() + "\n" + pInfo.location.ToString() + "\n" + pInfo.Sword.ToString());
            total = date.Concat(hInfo).ToArray();
            total = total.Concat(player).ToArray();
            Array.Resize<byte>(ref total, 150); 
            saves[slot].Write(total, 0, 150);
        }
    }
}
