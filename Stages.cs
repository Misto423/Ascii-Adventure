using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCIIR2
{
	public class Stages
	{
        //STAGE 1
		public static readonly KeyValuePair<ConsoleColor, char> STAGE_ONE_INFO = 
			new KeyValuePair<ConsoleColor, char>(ConsoleColor.Gray, '#');
		public static readonly byte[,] STAGE_ONE = new byte[,] {{8, 7},
																{6, 5}};
        public static readonly List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> STAGE_ONE_DOORS =
            new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>()
            {
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,0), new EngineFunctions.COORD(0,1)),
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,1), new EngineFunctions.COORD(1,1))
            };
		public static readonly EngineFunctions.COORD STAGE_ONE_START =
			new EngineFunctions.COORD(0, 0);

        //STAGE 2
		public static readonly KeyValuePair<ConsoleColor, char> STAGE_TWO_INFO = 
			new KeyValuePair<ConsoleColor, char>(ConsoleColor.Red, '#');
		public static readonly byte[,] STAGE_TWO = new byte[,] {{8, 13, 2, 7},
		                                                        {14, 9, 3, 11},
		                                                        {4, 2, 0, 10},
		                                                        {6, 1, 1, 10}};
        public static readonly List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> STAGE_TWO_DOORS =
           new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>()
            {
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,0), new EngineFunctions.COORD(2,0)),
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(2,1), new EngineFunctions.COORD(2,0)),
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,2), new EngineFunctions.COORD(2,2)),
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,3), new EngineFunctions.COORD(2,3))
            };
		public static readonly EngineFunctions.COORD STAGE_TWO_START =
			new EngineFunctions.COORD(0, 0);

        //STAGE 3
		public static readonly KeyValuePair<ConsoleColor, char> STAGE_THREE_INFO =
			new KeyValuePair<ConsoleColor, char>(ConsoleColor.DarkBlue, (char)0xA5);
		public static readonly byte[,] STAGE_THREE = new byte[,] {{12, 8, 2, 10},
																{6, 0, 3, 12},
																{16, 14, 6, 3},
																{12, 4, 7, 14},
																{6, 1, 0, 3},
																{9,13,5,11}};
        public static readonly List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> STAGE_THREE_DOORS =
           new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>()
           {
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,1), new EngineFunctions.COORD(1,2)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(2,1), new EngineFunctions.COORD(2,2)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,3), new EngineFunctions.COORD(0,4)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,5), new EngineFunctions.COORD(1,5)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,4), new EngineFunctions.COORD(3,5))
           };
		public static readonly EngineFunctions.COORD STAGE_THREE_START =
			new EngineFunctions.COORD(0, 0);

        //STAGE 4
		public static readonly KeyValuePair<ConsoleColor, char> STAGE_FOUR_INFO =
			new KeyValuePair<ConsoleColor, char>(ConsoleColor.Yellow, '#');
		public static readonly byte[,] STAGE_FOUR = new byte[,] {
																{8, 13, 13, 2, 2, 7},
																{14, 8, 7, 11, 11, 14},
																{14, 11, 14, 8, 7, 14},
																{14, 12, 6, 5, 14, 14},
																{14, 4, 7, 8, 3, 14},
																{14, 11, 14, 14, 11, 14},
																{6, 13, 1, 1, 13, 5}
																};
        public static readonly List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> STAGE_FOUR_DOORS =
           new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>()
           {
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,0), new EngineFunctions.COORD(1,0)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(4,0), new EngineFunctions.COORD(5,0)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,6), new EngineFunctions.COORD(1,6)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(5,6), new EngineFunctions.COORD(4,6)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(2,6), new EngineFunctions.COORD(2,5)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,6), new EngineFunctions.COORD(3,5)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,4), new EngineFunctions.COORD(4,4)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,2), new EngineFunctions.COORD(4,2)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,1), new EngineFunctions.COORD(2,1))
           };
		public static readonly EngineFunctions.COORD STAGE_FOUR_START =
			new EngineFunctions.COORD(0, 0);

        //STAGE 5
		public static readonly KeyValuePair<ConsoleColor, char> STAGE_FIVE_INFO =
			new KeyValuePair<ConsoleColor, char>(ConsoleColor.Cyan, '~');
		public static readonly byte[,] STAGE_FIVE = new byte[,] {
																{8, 2, 13, 2, 7},
																{4, 0, 2, 0, 3},
																{14, 4, 0, 3, 14},
																{4, 0, 1, 0, 3},
																{6, 1, 13, 1, 5}
																};
        public static readonly List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> STAGE_FIVE_DOORS =
           new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>()
           {
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,0), new EngineFunctions.COORD(0,1)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,0), new EngineFunctions.COORD(1,0)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(4,0), new EngineFunctions.COORD(3,0)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(4,0), new EngineFunctions.COORD(4,1)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,4), new EngineFunctions.COORD(0,3)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,4), new EngineFunctions.COORD(1,4)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,4), new EngineFunctions.COORD(4,4)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(4,3), new EngineFunctions.COORD(4,4)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(2,1), new EngineFunctions.COORD(1,1)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,2), new EngineFunctions.COORD(1,1)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,3), new EngineFunctions.COORD(1,2)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,3), new EngineFunctions.COORD(2,3)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,3), new EngineFunctions.COORD(2,3)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,3), new EngineFunctions.COORD(3,2)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,1), new EngineFunctions.COORD(3,2)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,1), new EngineFunctions.COORD(2,1))
           };
		public static readonly EngineFunctions.COORD STAGE_FIVE_START =
			new EngineFunctions.COORD(2, 2);

        //STAGE 6
		public static readonly KeyValuePair<ConsoleColor, char> STAGE_SIX_INFO =
			new KeyValuePair<ConsoleColor, char>(ConsoleColor.Blue, '~');
		public static readonly byte[,] STAGE_SIX = new byte[,] {
															   {9, 13, 2, 13, 10},
															   {8, 2, 0, 2, 7},
															   {14,14,11,14,14},
															   {14,4,13,3,14},
															   {14,14,12,14,14},
															   {6, 1, 0, 1, 5},
															   {9, 13,1, 13, 10}
															   };
        public static readonly List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> STAGE_SIX_DOORS =
            new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>()
            {
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,0), new EngineFunctions.COORD(1,0)),
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(4,0), new EngineFunctions.COORD(3,0)),
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(0,6), new EngineFunctions.COORD(1,6)),
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(4,6), new EngineFunctions.COORD(3,6)),
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,3), new EngineFunctions.COORD(2,3)),
                new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(2,3), new EngineFunctions.COORD(3,3))
            };
		public static readonly EngineFunctions.COORD STAGE_SIX_START =
			new EngineFunctions.COORD(2,3);

        //STAGE 7
		public static readonly KeyValuePair<ConsoleColor, char> STAGE_SEVEN_INFO =
			new KeyValuePair<ConsoleColor, char>(ConsoleColor.Green, '|');
		public static readonly byte[,] STAGE_SEVEN = new byte[,] {
																 {16,16,8,2,7,16,16},
																 {16,8,3,14,4,7,16},
																 {8,5,11,14,11,6,7},
																 {14,11,16,14,16,11,14},
																 {6,7,12,14,12,8,5},
																 {16,6,3,14,4,5,16},
																 {16,16,6,1,5,16,16}
																 };
        public static readonly List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> STAGE_SEVEN_DOORS =
           new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>()
           {
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,0), new EngineFunctions.COORD(3,1)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,6), new EngineFunctions.COORD(3,5))
           };
		public static readonly EngineFunctions.COORD STAGE_SEVEN_START =
			new EngineFunctions.COORD(3, 3);

        //STAGE 8
		public static readonly KeyValuePair<ConsoleColor, char> STAGE_EIGHT_INFO =
			new KeyValuePair<ConsoleColor, char>(ConsoleColor.DarkMagenta, (char)0xB0);
		public static readonly byte[,] STAGE_EIGHT = new byte[,] {
																 {8,7,16,16,16,8,7},
																 {6,0,13,2,13,0,5},
																 {16,14,16,14,16,14,16},
																 {16,4,13,0,13,3,16},
																 {16,14,16,14,16,14,16},
																 {8,0,13,1,13,0,7},
																 {6,5,16,16,16,6,5}
																 };
        public static readonly List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> STAGE_EIGHT_DOORS =
           new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>()
           {
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,1), new EngineFunctions.COORD(2,1)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,1), new EngineFunctions.COORD(1,2)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(5,1), new EngineFunctions.COORD(4,1)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(5,1), new EngineFunctions.COORD(5,2)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,5), new EngineFunctions.COORD(1,4)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,5), new EngineFunctions.COORD(2,5)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(5,5), new EngineFunctions.COORD(5,4)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(5,5), new EngineFunctions.COORD(4,5)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,1), new EngineFunctions.COORD(3,2)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(3,4), new EngineFunctions.COORD(3,5)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(1,3), new EngineFunctions.COORD(2,3)),
               new Tuple<EngineFunctions.COORD, EngineFunctions.COORD>(new EngineFunctions.COORD(4,3), new EngineFunctions.COORD(5,3))
           };
		public static readonly EngineFunctions.COORD STAGE_EIGHT_START =
			new EngineFunctions.COORD(3, 3);

        //STAGE 9
		public static readonly KeyValuePair<ConsoleColor, char> STAGE_NINE_INFO =
			new KeyValuePair<ConsoleColor, char>(ConsoleColor.White, '#');
		public static readonly byte[,] STAGE_NINE = new byte[,] {{15}};
        public static readonly List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>> STAGE_NINE_DOORS =
           new List<Tuple<EngineFunctions.COORD, EngineFunctions.COORD>>(){  };
		public static readonly EngineFunctions.COORD STAGE_NINE_START =
			new EngineFunctions.COORD(0, 0);
	}
}
