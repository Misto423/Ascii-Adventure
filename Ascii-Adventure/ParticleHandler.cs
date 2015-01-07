using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ASCIIR2
{
	public class Particle
	{
		public char PartChar { get; set; }
		public int Speed { get; set; }
		public int Life { get; set; }
		public ConsoleColor Color { get; set; }
		public EngineFunctions.COORD Pos { get; set; }
		public Stopwatch timer;
		public bool KillFlag { get; set; }
		public int Moved { get; set; }

		public Particle(char c, int s, int l, ConsoleColor color, short x, short y)
		{
			PartChar = c;
			Speed = s;
			Life = l;
			Color = color;
			Pos = new EngineFunctions.COORD(x, y);
			timer = new Stopwatch();
			KillFlag = false;
			Moved = 0;
		}
	}

	public class ParticleHandler
	{
		private List<Particle> particles;
		private char CurParticleChar;

		public bool NoParticles
		{
			get { return particles.Count == 0; }
		}

		public ParticleHandler(char c)
		{
			particles = new List<Particle>();
			CurParticleChar = (char)0xA6;
		}

		private ConsoleColor GetRandColor()
		{
			int i = Game.rand.Next(0, 16);
			switch (i)
			{
				case 15:
				default:
					return ConsoleColor.White;
				case 14:
					return ConsoleColor.Yellow;
				case 13:
					return ConsoleColor.Magenta;
				case 12:
					return ConsoleColor.Red;
				case 11:
					return ConsoleColor.Cyan;
				case 10:
					return ConsoleColor.Green;
				case 9:
					return ConsoleColor.Blue;
				case 8:
					return ConsoleColor.DarkGray;
				case 7:
					return ConsoleColor.Gray;
				case 6:
					return ConsoleColor.DarkYellow;
				case 5:
					return ConsoleColor.DarkMagenta;
				case 4:
					return ConsoleColor.DarkRed;
				case 3:
					return ConsoleColor.DarkCyan;
				case 2:
					return ConsoleColor.DarkGreen;
				case 1:
					return ConsoleColor.DarkBlue;
				case 0:
					return ConsoleColor.Black;
			}
		}

		public void SpawnParticles()
		{
			int num = Game.rand.Next(50, 151);
			for (int x = 0; x < num; x++)
			{
				short xPos = (short)Game.rand.Next(10, 71);
				short yPos = (short)Game.rand.Next(15, 30);
				int spd = Game.rand.Next(1,5);
				int life = Game.rand.Next(1, 31);
				particles.Add(new Particle(CurParticleChar, spd, life, GetRandColor(), xPos, yPos));
			}
		}

		public void UpdateParticles()
		{
			foreach (Particle p in particles)
			{
				if (!p.timer.IsRunning)
				{
					p.timer.Start();
				}
				if (p.timer.ElapsedMilliseconds >= 250)
				{
					p.Pos = new EngineFunctions.COORD(p.Pos.X, (short)(p.Pos.Y - p.Speed));
					p.Moved += p.Speed;
					if (p.Pos.Y < 0 || p.Moved > p.Life) p.KillFlag = true;
					p.timer.Restart();
				}
				if (p.Pos.Y < 20 && p.Pos.Y >= 0)
					EngineFunctions.DrawToConsole(p.Pos, p.PartChar, p.Color, Map.CurLevelInfo.Key);
			}

			particles.RemoveAll(x => x.KillFlag == true);
		}
	}
}
