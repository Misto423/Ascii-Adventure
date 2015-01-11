using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCIIR2
{
	public class Map
	{
		private byte[,] roomByteArray { get; set; }
		public EngineFunctions.COORD roomLocation { get; set; }
		private Room[,] roomArray;
		private int levelIndex = 0;
        public List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> lockedDoors;
        public List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> unlockedDoors;
		public static KeyValuePair<ConsoleColor,char> CurLevelInfo { get; private set; }

		public Map(EngineFunctions.COORD start)
		{
			roomLocation = start;
			ChangeMap();
		}

		public void ChangeMap()
		{
			levelIndex++;
			switch (levelIndex)
			{
				case 1:
				default:
					CurLevelInfo = Stages.STAGE_ONE_INFO;
					roomByteArray = Stages.STAGE_ONE;
					roomLocation = Stages.STAGE_ONE_START;
                    lockedDoors = Stages.STAGE_ONE_DOORS;
					break;
				case 2:
					CurLevelInfo = Stages.STAGE_TWO_INFO;
					roomByteArray = Stages.STAGE_TWO;
					roomLocation = Stages.STAGE_TWO_START;
                    lockedDoors = Stages.STAGE_TWO_DOORS;
					break;
				case 3:
					CurLevelInfo = Stages.STAGE_THREE_INFO;
					roomByteArray = Stages.STAGE_THREE;
					roomLocation = Stages.STAGE_THREE_START;
                    lockedDoors = Stages.STAGE_THREE_DOORS;
					break;
				case 4:
					CurLevelInfo = Stages.STAGE_FOUR_INFO;
					roomByteArray = Stages.STAGE_FOUR;
					roomLocation = Stages.STAGE_FOUR_START;
                    lockedDoors = Stages.STAGE_FOUR_DOORS;
					break;
				case 5:
					CurLevelInfo = Stages.STAGE_FIVE_INFO;
					roomByteArray = Stages.STAGE_FIVE;
					roomLocation = Stages.STAGE_FIVE_START;
                    lockedDoors = Stages.STAGE_FIVE_DOORS;
					break;
				case 6:
					CurLevelInfo = Stages.STAGE_SIX_INFO;
					roomByteArray = Stages.STAGE_SIX;
					roomLocation = Stages.STAGE_SIX_START;
                    lockedDoors = Stages.STAGE_SIX_DOORS;
					break;
				case 7:
					CurLevelInfo = Stages.STAGE_SEVEN_INFO;
					roomByteArray = Stages.STAGE_SEVEN;
					roomLocation = Stages.STAGE_SEVEN_START;
                    lockedDoors = Stages.STAGE_SEVEN_DOORS;
					break;
				case 8:
					CurLevelInfo = Stages.STAGE_EIGHT_INFO;
					roomByteArray = Stages.STAGE_EIGHT;
					roomLocation = Stages.STAGE_EIGHT_START;
                    lockedDoors = Stages.STAGE_EIGHT_DOORS;
					break;
				case 9:
					CurLevelInfo = Stages.STAGE_NINE_INFO;
					roomByteArray = Stages.STAGE_NINE;
					roomLocation = Stages.STAGE_NINE_START;
                    lockedDoors = Stages.STAGE_NINE_DOORS;
					break;
			}
            unlockedDoors = new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>();
			roomArray = new Room[roomByteArray.GetLength(0), roomByteArray.GetLength(1)];
			roomArray[roomLocation.Y, roomLocation.X] = new Room(roomByteArray[roomLocation.Y, roomLocation.X], false);
			roomArray[roomLocation.Y, roomLocation.X].IsExplored = true;
		}

        public List<int> GetExploredRooms()
        {
            List<int> explored = new List<int>();
            for (int y = 0; y < roomArray.GetLength(0); y++)
            {
                for (int x = 0; x < roomArray.GetLength(1); x++)
                {
                    if (roomArray[y,x] != null && roomArray[y,x].IsExplored)
                    {
                        explored.Add(y * roomArray.GetLength(1) + x);
                    }
                }
            }
            return explored;
        }

		public Room getRoom()
		{
			return roomArray[roomLocation.Y, roomLocation.X];
		}
		public void SetRoom(Room r)
		{
			roomArray[roomLocation.Y, roomLocation.X] = r;
		}

        public List<FaceDirection> GetLockedDoors()
        {
            List<FaceDirection> doors = new List<FaceDirection>();
            List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> d = FindDoor(roomLocation);
            foreach (Tuple<EngineFunctions.COORD, EngineFunctions.COORD> tu in d)
            {
                if (tu.Item1.X == roomLocation.X && tu.Item1.Y == roomLocation.Y)
                {
                    doors.Add(GetDirection(tu));
                }
                else
                {
                    doors.Add(GetDirection(new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(tu.Item2, tu.Item1)));
                }
            }
            return doors;
        }

        public void UnlockDoor(Tuple<EngineFunctions.COORD, EngineFunctions.COORD> door)
        {
            Tuple<EngineFunctions.COORD, EngineFunctions.COORD> reverse = 
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(door.Item2, door.Item1);
            if (lockedDoors.Remove(door))
            {
                unlockedDoors.Add(door);
            }
            else
            {
                lockedDoors.Remove(reverse);
                unlockedDoors.Add(reverse);
            }
        }

        private FaceDirection GetDirection(Tuple<EngineFunctions.COORD, EngineFunctions.COORD> tuple)
        {
            if (tuple.Item1.X < tuple.Item2.X)
            {
                return FaceDirection.Right;
            }
            else if (tuple.Item1.X > tuple.Item2.X)
            {
                return FaceDirection.Left;
            }
            else if (tuple.Item1.Y < tuple.Item2.Y)
            {
                return FaceDirection.Down;
            }
            else if (tuple.Item1.Y > tuple.Item2.Y)
            {
                return FaceDirection.Up;
            }
            return FaceDirection.noDir;
        }

		public int GetRoomCount()
		{
			return roomArray.GetLength(0) * roomArray.GetLength(1);
		}

		public int RoomNum
		{
			get { return roomByteArray[roomLocation.Y, roomLocation.X]; }
		}

		public void MoveToNextRoom(FaceDirection direction)
		{
			switch (direction)
			{
				default:
				case FaceDirection.Up:
					if (roomLocation.Y > 0)
					{
						roomLocation = new EngineFunctions.COORD(roomLocation.X, (short)(roomLocation.Y - 1));
					}
					break;
				case FaceDirection.Down:
					if (roomLocation.Y < roomArray.GetLength(0))
					{
						roomLocation = new EngineFunctions.COORD(roomLocation.X, (short)(roomLocation.Y + 1));
					}
					break;
				case FaceDirection.Left:
					if (roomLocation.X > 0)
					{
						roomLocation = new EngineFunctions.COORD((short)(roomLocation.X - 1), roomLocation.Y);
					}
					break;
				case FaceDirection.Right:
					if (roomLocation.X < roomArray.GetLength(1))
					{
						roomLocation = new EngineFunctions.COORD((short)(roomLocation.X + 1), roomLocation.Y);
					}
					break;
			}
			if (roomArray[roomLocation.Y, roomLocation.X] == null)
			{
				roomArray[roomLocation.Y, roomLocation.X] = new Room(roomByteArray[roomLocation.Y, roomLocation.X], true);
			}
		}

		public void DrawRoom()
		{
			roomArray[roomLocation.Y, roomLocation.X].DrawRoom();
            List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> lockedDoorsInRoom = FindDoor(roomLocation);
            if (lockedDoorsInRoom.Count > 0)
            {
                foreach (Tuple<EngineFunctions.COORD, EngineFunctions.COORD> d in lockedDoorsInRoom)
                {
                    if (d.Item1.X == roomLocation.X && d.Item1.Y == roomLocation.Y)
                    {
                        DrawDoor(d);
                    }
                    else //room is in tuple spot 2
                    {
                        DrawDoor(new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(d.Item2, d.Item1));
                    }
                }
            }
		}

        private void DrawDoor(Tuple<EngineFunctions.COORD, EngineFunctions.COORD> t)
        {
            if (t.Item1.X < t.Item2.X) //door to the right
            {
                for (int y = (Console.WindowHeight - 10) / 2 - 2; y <= (Console.WindowHeight - 10) / 2 + 2; y++)
                {
                    EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)(Console.WindowWidth - 1), (short)y), 'X', ConsoleColor.DarkYellow);
                }
            }
            else if (t.Item1.X > t.Item2.X) //door to the left
            {
                for (int y = (Console.WindowHeight - 10) / 2 - 2; y <= (Console.WindowHeight - 10) / 2 + 2; y++)
                {
                    EngineFunctions.DrawToConsole(new EngineFunctions.COORD(0, (short)y), 'X', ConsoleColor.DarkYellow);
                }
            }
            else if (t.Item1.Y < t.Item2.Y) // door to the bottom
            {
                for (int x = Console.WindowWidth / 2 - 2; x <= Console.WindowWidth / 2 + 2; x++)
                {
                    EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)x, (short)(Console.WindowHeight - 10)), 'X', ConsoleColor.DarkYellow);
                }
            }
            else if (t.Item1.Y > t.Item2.Y) // door to the top
            {
                for (int x = Console.WindowWidth / 2 - 2; x <= Console.WindowWidth / 2 + 2; x++)
                {
                    EngineFunctions.DrawToConsole(new EngineFunctions.COORD((short)x, 0), 'X', ConsoleColor.DarkYellow);
                }
            }
        }

        private List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> FindDoor(EngineFunctions.COORD room)
        {
            List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> l = new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>();
            foreach (Tuple<EngineFunctions.COORD, EngineFunctions.COORD> t in lockedDoors)
            {
                if ((t.Item1.X == room.X && t.Item1.Y == room.Y) ||
                            (t.Item2.X == room.X && t.Item2.Y == room.Y))
                {
                    l.Add(t);
                }
            }
            return l;
        }
	}
}
