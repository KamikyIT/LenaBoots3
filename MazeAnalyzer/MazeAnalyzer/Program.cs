using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeAnalyzer
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            AllocConsole();

            var filename = @"maze.png";

            var outsourcefile = @"my_map_source.txt";

            var outfile = @"my_map.txt";
            
            var mazeMaker = new MazeMaker(filename, outfile);


            //ReadFromFileAndWritemap(filename, outsourcefile, outfile);

            //var mazeAnalyzer = new ShitMazeAnalyzer(filename);

            //mazeAnalyzer.Anamlyze();
        }


        [Flags()]
        public enum MazeCell
        {
            Top = 1,
            Left = 2,
            Right = 4,
            Bot = 8,
        }

        public static void ReadFromFileAndWritemap(string sourcefileme, string outsourcefile, string outfile)
        {
            var bmp = new Bitmap(sourcefileme, true);// Bitmap.FromFile(sourcefileme);

            var lines = new List<string>();
            
            for (int i = 0; i < bmp.Width; i++)
            {
                var sb = new StringBuilder();
                for (int j = 0; j < bmp.Height; j++)
                {
                    if (bmp.GetPixel(i, j).ToArgb() == Color.White.ToArgb())
                    {
                        sb.Append(' ');
                    }
                    else if (bmp.GetPixel(i, j).ToArgb() == Color.Black.ToArgb())
                    {
                        sb.Append('+');
                    }
                    else
                    {
                        var p = 5;

                        sb.Append('W');
                    }
                }

                lines.Add(sb.ToString());
            }

            var sourcelines = lines.ToArray();

            lines = new List<string>();
            
            for (int i = 0; i < sourcelines.Length; i += 2)
            {
                var sl = sourcelines[i];
                {
                    var sb = new StringBuilder();

                    for (int j = 0; j < sl.Length; j+= 2)
                    {
                        sb.Append(sl[j]);
                    }

                    lines.Add(sb.ToString());
                }
            }

            {
                var itemsCount = 0;

                foreach (var line in lines)
                {
                    itemsCount += line.Count(x => x == '+');
                }

                Console.WriteLine("itemsCount = " + itemsCount.ToString());
            }

            var cell_width_count = (lines.First().Length - 1)/8;
            var cell_height_count = (lines.Count - 1)/8;

            var cells = new MazeCell[cell_height_count][];
            for (int i = 0; i < cell_height_count; i++)
            {
                cells[i] = new MazeCell[cell_width_count];
            }

            {
                foreach (var line in lines)
                {
                    

                }
            }



            if (File.Exists(outsourcefile))
            {
                File.Delete(outsourcefile);
            }
            else
            {
                File.Create(outsourcefile).Close();
            }

            if (File.Exists(outfile))
            {
                File.Delete(outfile);
            }
            else
            {
                File.Create(outfile).Close();
            }

            File.WriteAllLines(outsourcefile, sourcelines);
            File.WriteAllLines(outfile, lines);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();


        
        
    }
}
