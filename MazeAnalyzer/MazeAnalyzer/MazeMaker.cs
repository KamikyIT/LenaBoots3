using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace MazeAnalyzer
{
    public class MazeMaker
    {

        public const int blockSize = 8;
        private Bitmap img;
        private List<PartLined> partsLined;
        private List<CellModel> cellModels;

        private string outfile;
        private int blocks_width_count;
        private int blocks_height_count;

        public MazeMaker(string file, string outfile)
        {
            this.outfile = outfile;

            var bmp = new Bitmap(file, true);

            img = new Bitmap(bmp.Width - 2, bmp.Height - 2);

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    img.SetPixel(i, j, bmp.GetPixel(i, j));
                }
            }

            SetToStrings();

            blocks_width_count = lines.First().Length/blockSize;

            blocks_height_count = lines.Count/blockSize;

            partsLined = new List<PartLined>();

            for (int i = 0; i < blocks_height_count; i++)
            {
                for (int j = 0; j < blocks_width_count; j++)
                {
                    var pl = new PartLined(lines, i * blockSize, j * blockSize, i, j);

                    partsLined.Add(pl);
                }
            }

            ParsePartsLined();

            MakeItOut();
        }

        private void MakeItOut()
        {
            cellModels.Sort(compare);


            var lines = new List<string>();

            for (int i = 0; i < blocks_height_count; i++)
            {
                var sb = new StringBuilder();

                for (int j = 0; j < blocks_width_count; j++)
                {
                    var cm = cellModels.FirstOrDefault(x => x.Row == i && x.Column == j);

                    if (cm == null)
                    {
                        var p = 5;
                        continue;
                    }

                    sb.Append(CellConvert(cm.Cell));
                    // Конечная стенка.
                }
                sb.Append(CellConvert(Program.MazeCell.Left));


                lines.Add(sb.ToString());
            }

            var sbFinal = new StringBuilder();

            for (int i = 0; i < lines.First().Length; i++)
            {
                sbFinal.Append(CellConvert(Program.MazeCell.Top));
            }
            lines.Add(sbFinal.ToString());

            if (File.Exists(outfile))
            {
                File.Delete(outfile);
            }
            File.Create(outfile).Close();

            File.WriteAllLines(outfile, lines);
        }

        private string CellConvert(Program.MazeCell cell)
        {
            if (cell == (Program.MazeCell.Top | Program.MazeCell.Left))
            {
                return "+";
            }

            if (cell == Program.MazeCell.Left)
            {
                return "|";
            }
            if (cell == Program.MazeCell.Top)
            {
                return "-";
            }
            

            return " ";
        }

        private int compare(CellModel a, CellModel b)
        {
            if (a.Row < b.Row)
            {
                return -1;
            }
            if (a.Row > b.Row)
            {
                return 1;
            }

            if (a.Column < b.Column)
            {
                return -1;
            }

            if (a.Column > b.Column)
            {
                return 1;
            }

            return 0;

        }

        private void ParsePartsLined()
        {
            cellModels = new List<CellModel>();

            foreach (var partLined in partsLined)
            {
                var cm = new CellModel(partLined);
                cellModels.Add(cm);
            }

        }

        private void SetToStrings()
        {
            lines =new List<string>();
            for (int i = 0; i < img.Width; i+=2)
            {
                var sb = new StringBuilder();
                for (int j = 0; j < img.Height; j+=2)
                {
                    var p = img.GetPixel(i, j);

                    if (p.ToArgb() == Color.White.ToArgb())
                    {
                        sb.Append(' ');
                    }
                    else if (p.ToArgb() == Color.Black.ToArgb())
                    {
                        sb.Append('+');
                    }
                    else
                    {
                        var pause = 5;
                    }
                }

                lines.Add(sb.ToString());
            }
        }

        public List<string> lines { get; set; }

        private class CellModel
        {
            public int Row;
            public int Column;
            public Program.MazeCell Cell;

            public CellModel(PartLined partLined)
            {
                this.Row = partLined.Row;
                this.Column = partLined.Column;

                // TOP
                if (partLined.lines[0] == "++++++++")
                {
                    this.Cell |= Program.MazeCell.Top;
                }

                // LEFT
                var l = true;
                for (int i = 0; i < partLined.lines.Length; i++)
                {
                    if (partLined.lines[i][0] != '+')
                    {
                        l = false;
                        break;
                    }
                }
                if (l)
                {
                    this.Cell |= Program.MazeCell.Left;
                }
            }

        }

        private class PartLined
        {
            public string[] lines { get; set; }

            public int Row { get; set; }
            public int Column { get; set; }


            public PartLined(List<string> sourceLinesList, int x, int y, int r, int c)
            {
                this.Row = r;
                this.Column = c;

                lines = new string[MazeMaker.blockSize];

                for (int i = 0; i < MazeMaker.blockSize; i++)
                {
                    var sb = new StringBuilder();

                    for (int j = 0; j < MazeMaker.blockSize; j++)
                    {
                        sb.Append(sourceLinesList[x + i][y + j]);
                    }

                    lines[i] = sb.ToString();
                }

            }
        }
    }
}