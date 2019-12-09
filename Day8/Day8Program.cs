using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralUsage;

namespace Day8
{
    class Day8Program
    {
        // https://adventofcode.com/2019/day/7
        [STAThread]
        static void Main(string[] args)
        {
            Problem2();
        }

        static void Problem1()
        {
            // string example = "123456789012";
            string input = UsefulStuff.ImportTxtFileAsLines("Day8Input")[0];
            int width = 25;
            int height = 6;
            List<int[,]> img = LayerizeInput(width,height,input.ToCharArray());

            int minZeroCount = width*height;
            int[,] minZeroLayer = new int[width,height];
            foreach (int[,] layer in img)
            {
                int zeroCount = 0;
                for (int count = 0; count < (width*height); count++)
                {
                    int x = count % width;
                    int y = (int)(Math.Floor((double)count / width) % height);
                    if (layer[x,y] == 0) { zeroCount++; }
                }
                
                if(zeroCount < minZeroCount)
                {
                    minZeroCount = zeroCount;
                    minZeroLayer = layer;
                }
            }
            int oneCount = 0;
            int twoCount = 0;
            for (int count = 0; count < (width * height); count++)
            {
                int x = count % width;
                int y = (int)(Math.Floor((double)count / width) % height);
                if (minZeroLayer[x, y] == 1) { oneCount++; }
                if (minZeroLayer[x, y] == 2) { twoCount++; }
            }

            UsefulStuff.WriteSolution((oneCount * twoCount).ToString());

        }

        static void Problem2()
        {
            // string example = "123456789012";
            string input = UsefulStuff.ImportTxtFileAsLines("Day8Input")[0];
            int width = 25;
            int height = 6;
            List<int[,]> img = LayerizeInput(width, height, input.ToCharArray());
            int[,] finalImg = new int[width, height];
            for (int count = 0; count < (width * height); count++)
            {
                int x = count % width;
                int y = (int)(Math.Floor((double)count / width) % height);

                int layer = 0;
                int visiblePixel = img[layer][x, y];
                while (visiblePixel == 2)
                {
                    layer++;
                    visiblePixel = img[layer][x, y];
                }
                finalImg[x, y] = visiblePixel;
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(finalImg[x, y]);
                }
                Console.WriteLine("");
            }
            UsefulStuff.WriteSolution("done");
        }

        private static List<int[,]> LayerizeInput(int width, int height, char[] values)
        {
            List<int[,]> output = new List<int[,]>();
            output.Add(new int[width, height]);
            int count = 0;
            foreach (char valueChar in values )
            {
                int x = count % width;
                int y = (int) (Math.Floor((double)count / width) % height);
                int z = (int) (Math.Floor(Math.Floor((double)count / width) / height));
                if(z >= output.Count()) { output.Add(new int[width, height]); }
                output[z][x,y] = Int32.Parse(valueChar.ToString());
                count++;
            }
            return output;
        }
    }
}
