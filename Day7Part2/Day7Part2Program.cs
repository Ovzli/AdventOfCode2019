using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;

namespace Day7Part2
{
    class Day7Part2Program
    {
        //in a new project because this is different enough
        // https://adventofcode.com/2019/day/7
        [STAThread]
        static void Main(string[] args)
        {
            Problem2();
        }

        private static void Problem2()
        {
            //string elfCode = "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5";
            string elfCode = UsefulStuff.ImportTxtFileAsLines("Day7Input")[0];

            List<int> outputs = new List<int>();
            Stack<string> inputStack = new Stack<string>();

            string phaseSettings = "56789";
            int maxOutput = -999;
            List<string> phasePermutations = PremuteString(phaseSettings, 0, phaseSettings.Length - 1);

            foreach (string permutation in phasePermutations)
            {
                inputStack = new Stack<string>();
                char[] phases = permutation.ToCharArray();
                string phaseA = phases[0].ToString();
                string phaseB = phases[1].ToString();
                string phaseC = phases[2].ToString();
                string phaseD = phases[3].ToString();
                string phaseE = phases[4].ToString();

                ElfComputer AmpA = new ElfComputer(ParseInstructions(elfCode));
                Hashtable outputA = AmpA.ProcessStep(phaseA);              
                ElfComputer AmpB = new ElfComputer(ParseInstructions(elfCode));
                Hashtable outputB = AmpB.ProcessStep(phaseB);
                ElfComputer AmpC = new ElfComputer(ParseInstructions(elfCode));
                Hashtable outputC = AmpC.ProcessStep(phaseC);
                ElfComputer AmpD = new ElfComputer(ParseInstructions(elfCode));
                Hashtable outputD = AmpD.ProcessStep(phaseD);
                ElfComputer AmpE = new ElfComputer(ParseInstructions(elfCode));
                Hashtable outputE = AmpE.ProcessStep(phaseE);

                string lastOutput = "0";
                while ((string)outputA["Status"] != "Halt" ||
                       (string)outputB["Status"] != "Halt" ||
                       (string)outputC["Status"] != "Halt" ||
                       (string)outputD["Status"] != "Halt" ||
                       (string)outputE["Status"] != "Halt")
                {
                    bool runFlagA = true;
                    while(runFlagA && (string)outputA["Status"] != "Halt")
                    {
                        if((string)outputA["NextOpCode"] == "3") { outputA = AmpA.ProcessStep(lastOutput); }
                        else { outputA = AmpA.ProcessStep("NA"); }
                        runFlagA = !outputA.ContainsKey("Output");
                    }
                    if ((string)outputA["Status"] != "Halt") { lastOutput = (string)outputA["Output"]; }
                    

                    bool runFlagB = true;
                    while (runFlagB && (string)outputB["Status"] != "Halt")
                    {
                        if ((string)outputB["NextOpCode"] == "3") { outputB = AmpB.ProcessStep(lastOutput); }
                        else { outputB = AmpB.ProcessStep("NA"); }
                        runFlagB = !outputB.ContainsKey("Output");
                    }
                    if ((string)outputB["Status"] != "Halt")
                    { lastOutput = (string)outputB["Output"]; }

                    bool runFlagC = true;
                    while (runFlagC && (string)outputC["Status"] != "Halt")
                    {
                        if ((string)outputC["NextOpCode"] == "3") { outputC = AmpC.ProcessStep(lastOutput); }
                        else { outputC = AmpC.ProcessStep("NA"); }
                        runFlagC = !outputC.ContainsKey("Output");
                    }
                    if ((string)outputC["Status"] != "Halt")
                    { lastOutput = (string)outputC["Output"]; }

                    bool runFlagD = true;
                    while (runFlagD && (string)outputD["Status"] != "Halt")
                    {
                        if ((string)outputD["NextOpCode"] == "3") { outputD = AmpD.ProcessStep(lastOutput); }
                        else { outputD = AmpD.ProcessStep("NA"); }
                        runFlagD = !outputD.ContainsKey("Output");
                    }
                    if ((string)outputD["Status"] != "Halt")
                    { lastOutput = (string)outputD["Output"]; }

                    bool runFlagE = true;
                    while (runFlagE && (string)outputE["Status"] != "Halt")
                    {
                        if ((string)outputE["NextOpCode"] == "3") { outputE = AmpE.ProcessStep(lastOutput); }
                        else { outputE = AmpE.ProcessStep("NA"); }
                        runFlagE = !outputE.ContainsKey("Output");
                    }
                    if ((string)outputE["Status"] != "Halt")
                    { lastOutput = (string)outputE["Output"]; }
                }
                int lastVal = Int32.Parse(lastOutput);
                if (maxOutput < lastVal) { maxOutput = lastVal; }
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

        class ElfComputer
        {
            List<int> memory;
            private int instructionPointer;
            int instructionLength;
            bool pointerMoved;
            List<int> instructionList;
            int opCode;

            public ElfComputer(List<int> inputMemory)
                {
                memory = inputMemory;
                instructionPointer = 0;
                instructionLength = -1;
                pointerMoved = false;
                opCode = memory[instructionPointer] % 100;
            }

            public Hashtable ProcessStep(string inputStr)
            {
                Hashtable output = new Hashtable();
               if (opCode != 99)
                {
                    output.Add("Status", "Running");
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
                        int input = new int();
                        instructionLength = 2;
                        instructionList = memory.GetRange(instructionPointer, instructionLength);
                        if (inputStr != "NA")
                        {
                            if (inputStr != "manual")
                            {
                                input = Int32.Parse(inputStr);
                                Console.WriteLine("Input used: " + inputStr);
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
                            Console.WriteLine("Something bad happened. Input expected.");
                            Console.ReadKey(true);
                            System.Environment.Exit(1);
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
                        output.Add("Output", values[0].ToString());
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
                    output.Add("NextOpCode", opCode.ToString());
                    return output;
                }
               else
                {
                    output.Add("Status", "Halt");
                    return output;
                }
                

            }
        }

        //public static List<int> RunElfCode(List<int> memory, Stack<string> inputBuffer)
        //{
        //    Console.WriteLine("- Program Start -");
        //    //declare stuff
        //    int instructionPointer = 0;
        //    int instructionLength = -1;
        //    bool pointerMoved = false;
        //    List<int> instructionList;
        //    int opCode = memory[instructionPointer] % 100;
        //    List<int> outputs = new List<int>();

        //    //run the computer
        //    while (opCode != 99)
        //    {
        //        if (opCode == 1) //Addition
        //        {
        //            instructionLength = 4;
        //            instructionList = memory.GetRange(instructionPointer, instructionLength);

        //            List<int> readInstructions = instructionList.GetRange(1, 2);
        //            List<int> values = GetValues(instructionList[0], readInstructions, memory);

        //            memory[instructionList[3]] = values[0] + values[1];
        //        }
        //        else if (opCode == 2) //Multiplication
        //        {
        //            instructionLength = 4;
        //            instructionList = memory.GetRange(instructionPointer, instructionLength);

        //            List<int> readInstructions = instructionList.GetRange(1, 2);
        //            List<int> values = GetValues(instructionList[0], readInstructions, memory);

        //            memory[instructionList[3]] = values[0] * values[1];
        //        }
        //        else if (opCode == 3) //input
        //        {
        //            int input;
        //            instructionLength = 2;
        //            instructionList = memory.GetRange(instructionPointer, instructionLength);
        //            if (inputBuffer.Count() != 0)
        //            {
        //                string providedInput = inputBuffer.Pop();
        //                if (providedInput != "manual")
        //                {
        //                    input = Int32.Parse(providedInput);
        //                    Console.WriteLine("Input used: " + providedInput);
        //                }
        //                else
        //                {
        //                    Console.Write("Enter input: ");
        //                    input = Int32.Parse(Console.ReadLine());
        //                    Console.Write("\n");
        //                }
        //            }
        //            else
        //            {
        //                Console.Write("Enter input: ");
        //                input = Int32.Parse(Console.ReadLine());
        //                Console.Write("\n");
        //            }

        //            memory[instructionList[1]] = input;
        //        }
        //        else if (opCode == 4) //output
        //        {
        //            instructionLength = 2;
        //            instructionList = memory.GetRange(instructionPointer, instructionLength);

        //            List<int> readInstructions = instructionList.GetRange(1, 1);
        //            List<int> values = GetValues(instructionList[0], readInstructions, memory);

        //            Console.WriteLine("Output: " + values[0].ToString());
        //            outputs.Add(values[0]);
        //        }
        //        else if (opCode == 5) //jump if true
        //        {
        //            instructionLength = 3;
        //            instructionList = memory.GetRange(instructionPointer, instructionLength);

        //            List<int> readInstructions = instructionList.GetRange(1, 2);
        //            List<int> values = GetValues(instructionList[0], readInstructions, memory);

        //            if (values[0] != 0) { instructionPointer = values[1]; pointerMoved = true; }
        //        }
        //        else if (opCode == 6) //jump if false
        //        {
        //            instructionLength = 3;
        //            instructionList = memory.GetRange(instructionPointer, instructionLength);

        //            List<int> readInstructions = instructionList.GetRange(1, 2);
        //            List<int> values = GetValues(instructionList[0], readInstructions, memory);

        //            if (values[0] == 0) { instructionPointer = values[1]; pointerMoved = true; }
        //        }
        //        else if (opCode == 7) //less than
        //        {
        //            instructionLength = 4;
        //            instructionList = memory.GetRange(instructionPointer, instructionLength);

        //            List<int> readInstructions = instructionList.GetRange(1, 2);
        //            List<int> values = GetValues(instructionList[0], readInstructions, memory);

        //            memory[instructionList[3]] = Convert.ToInt32(values[0] < values[1]);
        //        }
        //        else if (opCode == 8) //equals
        //        {
        //            instructionLength = 4;
        //            instructionList = memory.GetRange(instructionPointer, instructionLength);

        //            List<int> readInstructions = instructionList.GetRange(1, 2);
        //            List<int> values = GetValues(instructionList[0], readInstructions, memory);

        //            memory[instructionList[3]] = Convert.ToInt32(values[0] == values[1]);
        //        }
        //        else if (opCode == 99)
        //        {
        //            instructionLength = 1;
        //            instructionList = memory.GetRange(instructionPointer, instructionLength);
        //            Console.WriteLine("Halt");
        //            //Do nothing, but we shouldn't get here in the first place
        //        }
        //        //error conditions below
        //        else
        //        {
        //            //Uh-oh
        //            Console.WriteLine("Something bad happened. Instruction Pointer Location: " + instructionPointer + " Opcode: " + opCode);
        //            Console.ReadKey(true);
        //            System.Environment.Exit(1);
        //        }

        //        if (instructionLength == -1) //yes, this is meant to be separate from the if block above
        //        {
        //            //Uh-oh
        //            Console.WriteLine("Instruction length never set. Instruction Pointer Location: " + instructionPointer + " Opcode: " + opCode);
        //            Console.ReadKey(true);
        //            System.Environment.Exit(1);
        //        }


        //        //Move to next code block
        //        if (!pointerMoved)
        //        {
        //            instructionPointer += instructionLength;
        //        }
        //        pointerMoved = false;
        //        instructionLength = -1;
        //        opCode = memory[instructionPointer] % 100;
        //    }

        //    return outputs;
        //}

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
