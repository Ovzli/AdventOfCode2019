using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;

namespace Day7
{
    class Day7Program
    {
        // https://adventofcode.com/2019/day/7
        [STAThread]
        static void Main(string[] args)
        {
            Problem1();
        }

        private static void Problem1()
        {
            //string elfCode = "3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0";
            string elfCode = UsefulStuff.ImportTxtFileAsLines("Day7Input")[0];
            
            List<int> outputs = new List<int>();
            Stack<string> inputStack = new Stack<string>();

            string phaseSettings = "01234";
            int maxOutput = -999;
            List<string> phasePermutations = PremuteString(phaseSettings, 0, phaseSettings.Length - 1);

            foreach (string permutation in phasePermutations)
            {
                inputStack = new Stack<string>();
                IEnumerable<char> phases = permutation.ToCharArray().Reverse();
                foreach (char phase in phases)
                {
                    inputStack.Push(phase.ToString());
                }
                inputStack.Push("0");

                while (inputStack.Count() > 1)
                {
                    Stack<string> inputBuffer = new Stack<string>();
                    inputBuffer.Push(inputStack.Pop());
                    inputBuffer.Push(inputStack.Pop());
                    List<int> memory = ParseInstructions(elfCode);
                    outputs = RunElfCode(memory, inputBuffer);

                    inputStack.Push(outputs[outputs.Count() - 1].ToString());
                }
                if(maxOutput < outputs[outputs.Count() - 1]) { maxOutput = outputs[outputs.Count() - 1]; }
            }

            UsefulStuff.WriteSolution(maxOutput.ToString());
        }

        private static List<string> PremuteString(String str,
                                int l, int r)
        {
            List<string> permutations = new List<string>();
            if (l == r)
            {
                permutations.Add(str);
                return permutations;
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    str = swap(str, l, i);
                    permutations = permutations.Concat(PremuteString(str, l + 1, r)).ToList();
                    str = swap(str, l, i);
                }
                return permutations;
            }
        }
        
        public static String swap(String a,
                                  int i, int j)
        {
            char temp;
            char[] charArray = a.ToCharArray();
            temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
            string s = new string(charArray);
            return s;
        }

        //Stuff needed for elf programs below (for easy copy and paste)

        public static List<int> RunElfCode(List<int> memory, Stack<string> inputBuffer)
        {
            Console.WriteLine("- Program Start -");
            //declare stuff
            int instructionPointer = 0;
            int instructionLength = -1;
            bool pointerMoved = false;
            List<int> instructionList;
            int opCode = memory[instructionPointer] % 100;
            List<int> outputs = new List<int>();

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
                    int input;
                    instructionLength = 2;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);
                    if( inputBuffer.Count()!=0)
                    {
                        string providedInput = inputBuffer.Pop();
                        if (providedInput != "manual")
                        {
                            input = Int32.Parse(providedInput);
                            Console.WriteLine("Input used: " + providedInput);
                        }
                        else
                        {
                            Console.Write("Enter input: ");
                            input = Int32.Parse(Console.ReadLine());
                            Console.Write("\n");
                        }
                    }
                    else
                    {
                        Console.Write("Enter input: ");
                        input = Int32.Parse(Console.ReadLine());
                        Console.Write("\n");
                    }      
                    
                    memory[instructionList[1]] = input;
                }
                else if (opCode == 4) //output
                {
                    instructionLength = 2;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);

                    List<int> readInstructions = instructionList.GetRange(1, 1);
                    List<int> values = GetValues(instructionList[0], readInstructions, memory);

                    Console.WriteLine("Output: " + values[0].ToString());
                    outputs.Add(values[0]);
                }
                else if (opCode == 5) //jump if true
                {
                    instructionLength = 3;
                    instructionList = memory.GetRange(instructionPointer, instructionLength);

                    List<int> readInstructions = instructionList.GetRange(1, 2);
                    List<int> values = GetValues(instructionList[0], readInstructions, memory);

                    if (values[0] != 0) { instructionPointer = values[1]; pointerMoved = true; }
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
                opCode = memory[instructionPointer] % 100;
            }

            return outputs;
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
                modes = (int)Math.Floor((double)(modes / 10));
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
