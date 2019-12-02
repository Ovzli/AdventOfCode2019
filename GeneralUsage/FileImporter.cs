using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUsage
{
    public class FileImporter
    {

        public static List<string> Import(string file)
        {
            string path = "C:\\Users\\14pie\\source\\repos\\AdventOfCode2019\\GeneralUsage\\Data\\" + file + ".txt";
            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\"+file+".txt");     make dynamic later
            string[] lines = System.IO.File.ReadAllLines(path);
            List<string> lineList = new List<string>(lines);
            return lineList;
        }
    }
}
