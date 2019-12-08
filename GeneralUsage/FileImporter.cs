using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GeneralUsage
{
    public class UsefulStuff
    {

        public static List<string> ImportTxtFileAsLines(string file)
        {
            //string path = "C:\\Users\\14pie\\source\\repos\\AdventOfCode2019\\GeneralUsage\\Data\\" + file + ".txt";
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\"+file+".txt");
			List<string> stringArray = new List<string>(path.Split('\\'));
			stringArray.RemoveRange(6,3);
			stringArray.Insert(6,"GeneralUsage");
			path = String.Join("\\",stringArray);
            string[] lines = System.IO.File.ReadAllLines(path);
            List<string> lineList = new List<string>(lines);
            return lineList;
        }

		public static void WriteSolution(string text)
		{
            Console.WriteLine("");
			Console.WriteLine(text);
			Clipboard.SetText(text);
			Console.ReadKey(true);
		}

    }
}
