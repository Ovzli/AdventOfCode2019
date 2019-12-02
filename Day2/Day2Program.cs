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
        public static void Main(string[] args)
        {
           Problem1();
        }  
		
		private static void Problem1()
		{
			string exampleElfCode1 = "1,0,0,0,99";
			string exampleElfCode2 = "2,3,0,3,99";
			string exampleElfCode3 = "2,4,4,5,99,0";
			string exampleElfCode4 = "1,1,1,4,99,5,6,0,99";
			string elfCode = FileImporter.Import("Problem2Input")[0];  //only want the first item of the list<string>
			List<int> program = SplitInstructions(elfCode);

			//Do Problem 1 specific stuff
			program[1] = 12;
			program[2] = 2;
			//

			program = RunElfCode(program);
			string writeableProgram = string.Join(",",program);
			Console.WriteLine(program[0]);
			Console.ReadKey(true);
		}

		private static List<int> SplitInstructions(string code)
		{
			List<string> instructionsAsString = new List<string>(code.Split(','));
			List<int> instructions = instructionsAsString.Select(int.Parse).ToList();
			return instructions;
		}

		private static List<int> RunElfCode(List<int> program)
		{
			int currentInstructionLoc = 0;
			List<int> instructionSet;
			int opCode = program[currentInstructionLoc];
			while (opCode != 99)
			{
				instructionSet = program.GetRange(currentInstructionLoc,4);
				if(opCode == 1) //Addition
				{
					int val1 = program[instructionSet[1]];
					int val2 = program[instructionSet[2]];
					program[instructionSet[3]] = val1 + val2; 
				}
				else if(opCode == 2) //Multiplication
				{
					int val1 = program[instructionSet[1]];
					int val2 = program[instructionSet[2]];
					program[instructionSet[3]] = val1 * val2; 
				}
				else if(opCode == 99)
				{
					//Do nothing, but we shouldn't get here in the first place
				}
				else
				{
					//Uh-oh
					Console.WriteLine("Something bad happened. Head Instruction Location: "+ currentInstructionLoc +" Head Instruction: "+opCode);
					Console.ReadKey(true);
					System.Environment.Exit(1);
				}

				//Move to next code block
				currentInstructionLoc += 4;
				opCode = program[currentInstructionLoc];
			}

			return program;
		}

    }
}
