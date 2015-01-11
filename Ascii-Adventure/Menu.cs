using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ASCIIR2
{
    public struct MenuItem
    {
        public string text;
        public EngineFunctions.COORD position;
        public MenuItem(string text, EngineFunctions.COORD position)
        {
            this.text = text;
            this.position = position;
        }
    }

    public class Menu
    {
        private readonly string line1 = @"   ___|   _ \    \  |   ___|    _ \   |      ____|";
        private readonly string line2 = @"  |      |   |    \ | \___ \   |   |  |      __|  ";
        private readonly string line3 = @"  |      |   |  |\  |       |  |   |  |      |    ";
        private readonly string line4 = @" \____| \___/  _| \_| _____/  \___/  _____| _____|";

        private readonly string line5 = @"      |      ____|   ___|  ____|   \  |  __ \  ";
        private readonly string line6 = @"      |      __|    |      __|      \ |  |   | ";
        private readonly string line7 = @"      |      |      |   |  |      |\  |  |   | ";
        private readonly string line8 = @"     _____| _____| \____| _____| _| \_| ____/ ";

        private int selection = 0;
        private Thread inputThread;
        private TimeSpan timeSinceUpdate = TimeSpan.Zero;
        private Input input;
        private char c;

        private bool showSaves = false;
        List<string> saveData;

        List<MenuItem> defaultItems, saveItems;

        public Menu()
        {
            input = new Input();
            inputThread = new Thread(new ThreadStart(GetInput));
            inputThread.Start();
            saveData = new List<string>();
            defaultItems = new List<MenuItem>();
            saveItems = new List<MenuItem>();

            defaultItems.Add(new MenuItem("New Game", new EngineFunctions.COORD(35, 20)));
            defaultItems.Add(new MenuItem("Load Game", new EngineFunctions.COORD(35, 22)));
            defaultItems.Add(new MenuItem("Exit Game", new EngineFunctions.COORD(35, 24)));
        }

        public void GetInput()
        {
            while (true)
            {
                EngineFunctions.Flush();
                if (DateTime.Now.TimeOfDay.Subtract(timeSinceUpdate) > TimeSpan.FromMilliseconds(50))
                {
                    c = input.GetInput();
                    timeSinceUpdate = DateTime.Now.TimeOfDay;
                }
            }
        }

        public int UpdateMenu()
        {
            if (!showSaves)
            {
                if (c == 'w')
                {
                    if (selection == 0)
                    {
                        selection = 2;
                    }
                    else
                    {
                        selection--;
                    }
                }
                else if (c == 's')
                {
                    if (selection == 2)
                    {
                        selection = 0;
                    }
                    else
                    {
                        selection++;
                    }
                }
                else if (c == ' ')
                {
                    switch (selection)
                    {
                        default:
                        case 0:
                            inputThread.Abort();
                            inputThread.Join();
                            return 0;
                        case 1:
                            saveData = Program.saver.GetSaveList();
                            showSaves = true;
                            selection = 0;
                            saveItems.Clear();
                            for (int y = 20, x = 0; x < 4; y+=2, x++)
                            {
                                if (saveData[x].Contains("No Save"))
                                {
                                    saveItems.Add(new MenuItem(saveData[x], new EngineFunctions.COORD(35, (short)y)));
                                }
                                else
                                {
                                    saveItems.Add(new MenuItem(saveData[x], new EngineFunctions.COORD(25, (short)y)));
                                }
                            }
                            saveItems.Add(new MenuItem("Cancel", new EngineFunctions.COORD(37, 28)));
                            break;
                        case 2:
                            inputThread.Abort();
                            inputThread.Join();
                            return 2;
                    }
                }
            }
            else
            {
                if (c == 'w')
                {
                    if (selection == 0)
                    {
                        selection = 4;
                    }
                    else
                    {
                        selection--;
                    }
                }
                else if (c == 's')
                {
                    if (selection == 4)
                    {
                        selection = 0;
                    }
                    else
                    {
                        selection++;
                    }
                }
                else if (c == ' ')
                {
                    if (selection == 4 || saveData[selection].Contains("No Save"))
                    {
                        showSaves = false;
                        selection = 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            c = 'n';
            return -1;
        }

        public void DrawMenu()
        {
            EngineFunctions.DrawToConsole(new EngineFunctions.COORD(10, 5), line1, ConsoleColor.White);
            EngineFunctions.DrawToConsole(new EngineFunctions.COORD(10, 6), line2, ConsoleColor.White);
            EngineFunctions.DrawToConsole(new EngineFunctions.COORD(10, 7), line3, ConsoleColor.White);
            EngineFunctions.DrawToConsole(new EngineFunctions.COORD(10, 8), line4, ConsoleColor.White);

            EngineFunctions.DrawToConsole(new EngineFunctions.COORD(20, 12), line5, ConsoleColor.White);
            EngineFunctions.DrawToConsole(new EngineFunctions.COORD(20, 13), line6, ConsoleColor.White);
            EngineFunctions.DrawToConsole(new EngineFunctions.COORD(20, 14), line7, ConsoleColor.White);
            EngineFunctions.DrawToConsole(new EngineFunctions.COORD(20, 15), line8, ConsoleColor.White);

            for (int x = 0; x < Console.WindowWidth; x++)
            {
                EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)x, 17), '-', ConsoleColor.White);
            }

            if (!showSaves)
            {
                foreach(MenuItem mi in defaultItems)
                {
                    EngineFunctions.DrawToConsole(mi.position, mi.text, ConsoleColor.White);
                }
                SetSelection(defaultItems);
            }
            else
            {
                foreach (MenuItem mi in saveItems)
                {
                    EngineFunctions.DrawToConsole(mi.position, mi.text, ConsoleColor.White);
                }
                SetSelection(saveItems);
            }
        }

        private void SetSelection(List<MenuItem> items)
        {
            EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(items[selection].position.X - 4),
                (short)items[selection].position.Y), "->", ConsoleColor.White);
        }
    }
}
