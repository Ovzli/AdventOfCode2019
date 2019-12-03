﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;

namespace Day3
{
	class Day3Program
	{
		//https://adventofcode.com/2019/day/3
		[STAThread]
		public static void Main(string[] args)
		{
			Problem1();
		}

		public static void Problem1()
		{
			string exampleWire1A = "R8,U5,L5,D3";
			string exampleWire1B = "U7,R6,D4,L4";
			List<string> wires = UsefulStuff.ImportTxtFileAsLines("Problem3Input");

			List<string> wireOneDirections = new List<string>(wires[0].Split(','));
			List<string> wireTwoDirections = new List<string>(wires[1].Split(','));

			List<wirePosition> wireOnePath = CalculateWirePath(wireOneDirections);
			List<wirePosition> wireTwoPath = CalculateWirePath(wireTwoDirections);

			List<wirePosition> wireIntersections = new List<wirePosition>();
			//IEnumerable<wirePosition> wireIntersections = (wireOnePath).Intersect(wireTwoPath);
			foreach (wirePosition i in wireOnePath) //lol brute force, this takes two minutes to run
			{
				foreach (wirePosition j in wireTwoPath)
				{
					if ((i.xPos == j.xPos) && (i.yPos == j.yPos))
					{
						wireIntersections.Add(i);
					}
				}
			}
			List<int> intersectionDistances = new List<int>();
			foreach(wirePosition intersection in wireIntersections)
			{
				if (intersection.xPos != 0 && intersection.yPos != 0)
				{
					intersectionDistances.Add(Math.Abs(intersection.xPos) + Math.Abs(intersection.yPos));
				}
				else
				{
					//is origin, do nothing
				}
			}
			int closestIntersection = intersectionDistances.Min();
			UsefulStuff.WriteSolution(closestIntersection.ToString());
		}

		private static List<wirePosition> CalculateWirePath(List<string> wireDirections)
		{
			wirePosition origin = new wirePosition();
			origin.xPos = 0;
			origin.yPos = 0;
			List<wirePosition> wirePath = new List<wirePosition> { origin };
			foreach (string instruction in wireDirections)
			{
				string direction = instruction.Substring(0, 1);
				int distance = Int32.Parse(instruction.Substring(1));

				for(int i = 0; i < distance; i++)
				{
					wirePosition newWirePosition = wirePath[wirePath.Count - 1].ShallowCopy();
					if (direction == "U") {newWirePosition.yPos++; }
					else if (direction == "D") { newWirePosition.yPos--; }
					else if (direction == "R") { newWirePosition.xPos++; }
					else if (direction == "L") { newWirePosition.xPos--; }
					else
					{
						//Uh-oh
						Console.WriteLine("Something bad happened. Direction: " + direction + " Distance: " + distance);
						Console.ReadKey(true);
						System.Environment.Exit(1);
					}
					wirePath.Add(newWirePosition);
				}
			}
			return wirePath;
		}

		private class wirePosition
		{
			public int xPos;
			public int yPos;

			public wirePosition ShallowCopy()
			{
				return (wirePosition)this.MemberwiseClone();
			}
		}
	}
}
