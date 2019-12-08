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
            Problem();
        }

        private static void Problem() // P1 input 1, P2 input 2
        {
            //string example = "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99";
            //List<int> memory = ParseInstructions(example);
            //RunElfCode(memory);

            string elfCode = UsefulStuff.ImportTxtFileAsLines("Day5Input")[0];
            List<int> memory = ParseInstructions(elfCode);
            RunElfCode(memory);

            UsefulStuff.WriteSolution("Done");
        }

        public static List<int> RunElfCode(List<int> memory)
        {
            //declare stuff
            int instructionPointer = 0;
            int instructionLength = -1;
            bool pointerMoved = false;
            List<int> instructionList;
            int opCode = memory[instructionPointer]%100;

            //run the computer
            while (opCode != 99)
            {
                if (opCode == 1) //Addition
                {
                    instructionLength = 4;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);

                    List<int> readInstructions = instructionList.GetRange(1, 2);
                    List<int> values = GetValues(instructionList[0], readInstructions, memory);

                    memory[instructionList[3]] = values[0] + values[1];
                }
                else if (opCode == 2) //Multiplication
                {
                    instructionLength = 4;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);

                    List<int> readInstructions = instructionList.GetRange(1, 2);
                    List<int> values = GetValues(instructionList[0], readInstructions, memory);

                    memory[instructionList[3]] = values[0] * values[1];
                }
                else if (opCode == 3) //input
                {
                    instructionLength = 2;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);
                    Console.Write("Enter input: ");
                    int input = Int32.Parse(Console.ReadLine());
                    memory[instructionList[1]] = input;

                    Console.Write("\n");
                }
                else if (opCode == 4) //output
                {
                    instructionLength = 2;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);
                    
                    List<int> readInstructions = instructionList.GetRange(1, 1);
                    List<int> values = GetValues(instructionList[0], readInstructions, memory);

                    Console.WriteLine("Output: "+ values[0].ToString());
                }
                else if (opCode == 5) //jump if true
                {
                    instructionLength = 3;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);

                    List<int> readInstructions = instructionList.GetRange(1, 2);
                    List<int> values = GetValues(instructionList[0], readInstructions, memory);

                    if(values[0] != 0) { instructionPointer = values[1]; pointerMoved = true; }
                }
                else if (opCode == 6) //jump if false
                {
                    instructionLength = 3;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);

                    List<int> readInstructions = instructionList.GetRange(1, 2);
                    List<int> values = GetValues(instructionList[0], readInstructions, memory);

                    if (values[0] == 0) { instructionPointer = values[1]; pointerMoved = true; }
                }
                else if (opCode == 7) //less than
                {
                    instructionLength = 4;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);

                    List<int> readInstructions = instructionList.GetRange(1, 2);
                    List<int> values = GetValues(instructionList[0], readInstructions, memory);

                    memory[instructionList[3]] = Convert.ToInt32(values[0] < values[1]);
                }
                else if (opCode == 8) //equals
                {
                    instructionLength = 4;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);

                    List<int> readInstructions = instructionList.GetRange(1, 2);
                    List<int> values = GetValues(instructionList[0], readInstructions, memory);

                    memory[instructionList[3]] = Convert.ToInt32(values[0] == values[1]);
                }
                else if (opCode == 99)
                {
                    instructionLength = 1;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);
                    Console.WriteLine("Halt");
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
                if (!pointerMoved)
                {
                    instructionPointer += instructionLength;
                }
                pointerMoved = false;
                instructionLength = -1;
                opCode = memory[instructionPointer]%100;
            }

            return memory;
        }

        private static List<int> GetValues(int fullOp, List<int> instructions, List<int> memory)
        {
            int modes = (int)Math.Floor((double)(fullOp / 100));
            List<int> values = new List<int>();

            foreach (int instruction in instructions)
            {
                int currentMode = modes % 10;
                if (currentMode == 0) { values.Add(memory[instruction]); } //paramter mode
                else if (currentMode == 1) { values.Add(instruction); }
                else
                {
                    //Uh-oh
                    Console.WriteLine("Bad instruction mode. Mode: " + currentMode);
                    Console.ReadKey(true);
                    System.Environment.Exit(1);
                }
                modes = (int) Math.Floor((double) (modes / 10));
            }
            return values;
        }

        public static List<int> ParseInstructions(string code)
        {
            List<string> instructionsAsString = new List<string>(code.Split(','));
            List<int> instructions = instructionsAsString.Select(int.Parse).ToList();
            return instructions;
        }
    }
}
