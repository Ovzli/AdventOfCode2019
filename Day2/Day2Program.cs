using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;

namespace Day2
{
    class Day2Program
    {
		//https://adventofcode.com/2019/day/2
		[STAThread]
		public static void Main(string[] args)
        {
           Problem2();
        }  
		
		private static void Problem1()
		{
			string exampleElfCode1 = "1,0,0,0,99";
			string exampleElfCode2 = "2,3,0,3,99";
			string exampleElfCode3 = "2,4,4,5,99,0";
			string exampleElfCode4 = "1,1,1,4,99,5,6,0,99";
			string elfCode = UsefulStuff.ImportTxtFileAsLines("Problem2Input")[0];  //only want the first item of the list<string>
			List<int> program = SplitInstructions(elfCode);

			//Do Problem 1 specific stuff
			program[1] = 12;
			program[2] = 2;
			//

			program = RunElfCode(program);
			string writeableProgram = string.Join(",",program);
			UsefulStuff.WriteSolution(program[0].ToString());
		}

		private static void Problem2()
		{
			string elfCode = UsefulStuff.ImportTxtFileAsLines("Problem2Input")[0];  //only want the first item of the list<string>
			List<int> program;

			int maxNoun = 99;
			int maxVerb = 99;
			int targetValue = 19690720;

			//Saddleback search
			int output = FindPair(maxNoun, maxVerb, targetValue, elfCode);
			if(output == -1)
			{
				UsefulStuff.WriteSolution("Not Found");
			}
			else
			{
				UsefulStuff.WriteSolution(output.ToString());
			}

			//original quick n' dirty search
			for (int i = 0; i <= 99; i++)
				if (false) { 				
				{
					for(int j = 0; j<=99; j++)
					{
						program = SplitInstructions(elfCode);
						program[1] = i;
						program[2] = j;

						//Console.WriteLine("Noun: " + i + " Verb: " + j);
						program = RunElfCode(program);		
						if(i==0 || j == 0)
						{
							Console.WriteLine("Noun: " + i + " Verb: " + j);
							Console.WriteLine(program[0]);
						}
						if(program[0] == 19690720)
						{
							Console.WriteLine(100 * i + j);
							Console.ReadKey(true);
							System.Environment.Exit(1);
						}
					}
				}
			}
		}

		private static int FindPair(int m, int n, int desiredValue, string elfCode)
		{
			int i = m;
			int j = 0;
			List<int> program;
			while (i >= 0 && j <= n )
			{
				program = SplitInstructions(elfCode);
				program[1] = i;
				program[2] = j;

				Console.WriteLine("Noun: " + i + " Verb: " + j);
				program = RunElfCode(program);
				
				if (program[0] == desiredValue)
				{
					return 100 * i + j;
				}
				else if(program[0] > desiredValue)
				{
					i--;
				}
				else //program[0] < desiredValue
				{
					j++;
				}
			}

			return -1;
		}

		private static List<int> SplitInstructions(string code)
		{
			List<string> instructionsAsString = new List<string>(code.Split(','));
			List<int> instructions = instructionsAsString.Select(int.Parse).ToList();
			return instructions;
		}

		private static List<int> RunElfCode(List<int> memory)
		{
			int instructionPointer = 0;
			int instructionLength = -1;
			List<int> instructionList;
			int opCode = memory[instructionPointer];

			while (opCode != 99)
			{
				instructionList = memory.GetRange(instructionPointer,4);
				if(opCode == 1) //Addition
				{
					instructionLength = 4;
					int val1 = memory[instructionList[1]];
					int val2 = memory[instructionList[2]];
					memory[instructionList[3]] = val1 + val2; 
				}
				else if(opCode == 2) //Multiplication
				{
					instructionLength = 4;
					int val1 = memory[instructionList[1]];
					int val2 = memory[instructionList[2]];
					memory[instructionList[3]] = val1 * val2; 
				}
				else if(opCode == 99)
				{
					instructionLength = 1;
					//Do nothing, but we shouldn't get here in the first place
				}
				//error conditions below
				else
				{
					//Uh-oh
					Console.WriteLine("Something bad happened. Instruction Pointer Location: " + instructionPointer + " Opcode: " + opCode);
					Console.ReadKey(true);
					System.Environment.Exit(1);
				}

				if (instructionLength == -1) //yes, this is meant to be separate from the if block above
				{
					//Uh-oh
					Console.WriteLine("Instruction length never set. Instruction Pointer Location: " + instructionPointer + " Opcode: " + opCode);
					Console.ReadKey(true);
					System.Environment.Exit(1);
				}
				

				//Move to next code block
				instructionPointer += instructionLength;
				instructionLength = -1;
				opCode = memory[instructionPointer];
			}

			return memory;
		}

    }
}
