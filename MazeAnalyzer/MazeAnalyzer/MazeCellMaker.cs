using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace MazeAnalyzer
{
    public class MazeCellMaker
    {

        public MazeCellMaker(string filename)
        {
            var bmp = new Bitmap(filename, true);

            var lines = new List<string>();

            for (int i = 0; i < bmp.Width - 2; i += 2)
            {
                var sb = new StringBuilder();

                for (int j = 0; j < bmp.Height - 2; j += 2)
                {
                    sb.Append(bmp.GetPixel(i, j));
                }

                lines.Add(sb.ToString());
            }

            {
                var newfilename = filename.Replace(".png", "_source_map.txt");

                if (File.Exists(newfilename))
                {
                    File.Delete(newfilename);
                }
                else
                {
                    File.Create(newfilename).Close();
                }
                File.WriteAllLines(newfilename, lines);
            }

            

        }

        
    }
}