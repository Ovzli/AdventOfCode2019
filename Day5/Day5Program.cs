using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;

namespace Day5
{
    class Day5Program
    {
        // https://adventofcode.com/2019/day/5
        [STAThread]
        static void Main(string[] args)
        {
            Problem1();
        }

        private static void Problem1()
        {
            string example = "3,0,4,0,99";
            List<int> test = ParseInstructions(example);
            RunElfCode(test);
            UsefulStuff.WriteSolution("Test");
        }

        public static List<int> RunElfCode(List<int> memory)
        {
            //declare stuff
            int instructionPointer = 0;
            int instructionLength = -1;
            List<int> instructionList;
            int opCode = memory[instructionPointer];

            //run the computer
            while (opCode != 99)
            {
                
                if (opCode == 1) //Addition
                {
                    instructionLength = 4;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);
                    int val1 = memory[instructionList[1]];
                    int val2 = memory[instructionList[2]];
                    memory[instructionList[3]] = val1 + val2;
                }
                else if (opCode == 2) //Multiplication
                {
                    instructionLength = 4;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);
                    int val1 = memory[instructionList[1]];
                    int val2 = memory[instructionList[2]];
                    memory[instructionList[3]] = val1 * val2;
                }
                else if (opCode == 3) //input
                {
                    instructionLength = 2;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);
                    Console.Write("Enter input: ");
                    int input = Int32.Parse(Console.ReadLine());
                    memory[instructionList[1]] = input;
                }
                else if (opCode == 4) //output
                {
                    instructionLength = 2;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);
                    Console.Write("Output: "+ memory[instructionList[1]].ToString());
                }
                else if (opCode == 99)
                {
                    instructionLength = 1;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);
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

        public static List<int> ParseInstructions(string code)
        {
            List<string> instructionsAsString = new List<string>(code.Split(','));
            List<int> instructions = instructionsAsString.Select(int.Parse).ToList();
            return instructions;
        }
    }
}
