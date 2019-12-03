using System;
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
			//string exampleWire1A = "R8,U5,L5,D3";
			//string exampleWire1B = "U7,R6,D4,L4";
			List<string> wires = UsefulStuff.ImportTxtFileAsLines("Problem3Input");

			List<string> wireOneDirections = new List<string>(wires[0].Split(','));
			List<string> wireTwoDirections = new List<string>(wires[1].Split(','));

			List<WirePosition> wireOnePath = CalculateWirePath(wireOneDirections);
			List<WirePosition> wireTwoPath = CalculateWirePath(wireTwoDirections);

			//old brute force code, this takes two minutes to run
			//foreach (wirePosition i in wireOnePath) 
			//List<wirePosition> wireIntersections = new List<wirePosition>();
			//{
			//	foreach (wirePosition j in wireTwoPath)
			//	{
			//		if ((i.xPos == j.xPos) && (i.yPos == j.yPos))
			//		{
			//			wireIntersections.Add(i);
			//		}
			//	}
			//}
			IEnumerable<WirePosition> wireIntersections = (wireOnePath).Intersect(wireTwoPath, new WirePositionComparer());
			List<int> intersectionDistances = new List<int>();
			foreach(WirePosition intersection in wireIntersections)
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

		private static void Problem2()
		{
			//string exampleWire1A = "R8,U5,L5,D3";
			//string exampleWire1B = "U7,R6,D4,L4";
			//string exampleWire2A = "R75,D30,R83,U83,L12,D49,R71,U7,L72";
			//string exampleWire2B = "U62,R66,U55,R34,D71,R55,D58,R83";
			List<string> wires = UsefulStuff.ImportTxtFileAsLines("Problem3Input");

			List<string> wireOneDirections = new List<string>(wires[0].Split(','));
			List<string> wireTwoDirections = new List<string>(wires[1].Split(','));

			List<WirePosition> wireOnePath = CalculateWirePath(wireOneDirections);
			List<WirePosition> wireTwoPath = CalculateWirePath(wireTwoDirections);
			
			List<int> intersectionSignalDelays = new List<int>();
			//IEnumerable<wirePosition> wireIntersections = (wireOnePath).Intersect(wireTwoPath);
			foreach (WirePosition i in wireOnePath) //lol brute force, this takes two minutes to run
			{
				if (i.xPos != 0 && i.yPos != 0)
				{
					foreach (WirePosition j in wireTwoPath)
					{
						if ((i.xPos == j.xPos) && (i.yPos == j.yPos))
						{
							intersectionSignalDelays.Add(i.stepsFromOrigin + j.stepsFromOrigin);
						}
					}
				}
			}
			int closestIntersection = intersectionSignalDelays.Min();
			UsefulStuff.WriteSolution(closestIntersection.ToString());
		}

		private static List<WirePosition> CalculateWirePath(List<string> wireDirections)
		{
			WirePosition origin = new WirePosition();
			origin.xPos = 0;
			origin.yPos = 0;
			origin.stepsFromOrigin = 0;
			List<WirePosition> wirePath = new List<WirePosition> { origin };
			foreach (string instruction in wireDirections)
			{
				string direction = instruction.Substring(0, 1);
				int distance = Int32.Parse(instruction.Substring(1));

				for(int i = 0; i < distance; i++)
				{
					WirePosition newWirePosition = wirePath[wirePath.Count - 1].ShallowCopy();
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
					newWirePosition.stepsFromOrigin++;
					wirePath.Add(newWirePosition);
				}
			}
			return wirePath;
		}

		private class WirePosition
		{
			public int xPos;
			public int yPos;
			public int stepsFromOrigin;

			public WirePosition ShallowCopy()
			{
				return (WirePosition)this.MemberwiseClone();
			}
		}

		private class WirePositionComparer : IEqualityComparer<WirePosition>
		{
			public bool Equals(WirePosition a, WirePosition b)
			{
				if(ReferenceEquals(a,b)) { return true; }

				if(ReferenceEquals(a,null) || ReferenceEquals(b,null)) { return false; }

				return a.xPos == b.xPos && a.yPos == b.yPos;
			}

			public int GetHashCode(WirePosition w)
			{
				if (ReferenceEquals(w, null)) { return 0; }

				int hashWirePositionXPos = w.xPos.GetHashCode();
				int hashWirePositionYPos = w.yPos.GetHashCode();

				return hashWirePositionXPos ^ hashWirePositionYPos;
			}
		}
	}
}
