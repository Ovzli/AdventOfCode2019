using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;

namespace Day4
{
	class Day4Program
	{
		// https://adventofcode.com/2019/day/4
		[STAThread]
		static void Main(string[] args)
		{
			Problem2();
		}

		public static void Problem1()
		{
			
			string[] ranges = UsefulStuff.ImportTxtFileAsLines("Day4Input")[0].Split('-');
			int lowerBound = Int32.Parse(ranges[0]);
			int upperBound = Int32.Parse(ranges[1]);
			int count = 0;
			for (int i = lowerBound; i < upperBound; i++)
			{
				bool increasing = true;
				bool pairExists = false;
				int[] currentNum = GetIntArray(i);

				for (int digitIndex = 0; digitIndex < 5; digitIndex++)
				{
					if (currentNum[digitIndex] > currentNum[digitIndex+1]) { increasing = false; }
					if (currentNum[digitIndex] == currentNum[digitIndex+1]) { pairExists = true; }
				}

				if(increasing && pairExists) { count++; }
			}

			UsefulStuff.WriteSolution(count.ToString());
		}

		public static void Problem2()
		{

			string[] ranges = UsefulStuff.ImportTxtFileAsLines("Day4Input")[0].Split('-');
			int lowerBound =  Int32.Parse(ranges[0]);
			int upperBound =  Int32.Parse(ranges[1]);
			//int lowerBound = 123444;
			//int upperBound = 123444;
			int count = 0;
			for (int i = lowerBound; i <= upperBound; i++)
			{
				bool increasing = true;
				
				int[] currentNum = GetIntArray(i);

				for (int digitIndex = 0; digitIndex < 5; digitIndex++)
				{
					if (currentNum[digitIndex] > currentNum[digitIndex + 1]) { increasing = false; }
					if (currentNum[digitIndex] == currentNum[digitIndex + 1])
					{

					}
				}

				bool pairExists = false;
				bool lockPE = false;
				int groupCount = 0;
				int groupValue = -1;
				for (int digitIndex = 0; digitIndex <= 5; digitIndex++)
				{
					if(currentNum[digitIndex] == groupValue)
					{
						groupCount++;
						if(groupCount == 2) { pairExists = true; }
						else //groupCount > 2
						{
							if(!lockPE) { pairExists = false; }
						}
					}
					else
					{
						if(pairExists) { lockPE = true; }
						groupCount = 1;
						groupValue = currentNum[digitIndex];
					}
				}

				if (increasing && pairExists) { count++; }
			}

			UsefulStuff.WriteSolution(count.ToString());
		}

		private static int[] GetIntArray(int num)
		{
			List<int> listOfInts = new List<int>();
			while (num > 0)
			{
				listOfInts.Add(num % 10);
				num = num / 10;
			}
			listOfInts.Reverse();
			return listOfInts.ToArray();
		}
	}
}
