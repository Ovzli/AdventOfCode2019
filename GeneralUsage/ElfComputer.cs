using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUsage
{
    public class ElfComputer
    {
        public static List<int> RunElfCode(int noun, int verb, List<int> memory)
        {
            //declare stuff
            int instructionPointer = 0;
            int instructionLength = -1;
            List<int> instructionList;
            int opCode = memory[instructionPointer];

            //set up memory
            memory[1] = noun;
            memory[2] = verb;           

            //run the computer
            while (opCode != 99)
            {
                instructionList = memory.GetRange(instructionPointer, 4);
                if (opCode == 1) //Addition
                {
                    instructionLength = 4;
                    int val1 = memory[instructionList[1]];
                    int val2 = memory[instructionList[2]];
                    memory[instructionList[3]] = val1 + val2;
                }
                else if (opCode == 2) //Multiplication
                {
                    instructionLength = 4;
                    int val1 = memory[instructionList[1]];
                    int val2 = memory[instructionList[2]];
                    memory[instructionList[3]] = val1 * val2;
                }
                else if (opCode == 99)
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

        public static List<int> ParseInstructions(string code)
        {
            List<string> instructionsAsString = new List<string>(code.Split(','));
            List<int> instructions = instructionsAsString.Select(int.Parse).ToList();
            return instructions;
        }
    }
}
