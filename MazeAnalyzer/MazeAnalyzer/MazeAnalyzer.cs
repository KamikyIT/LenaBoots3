using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace MazeAnalyzer
{
    public class ShitMazeAnalyzer
    {
        private Bitmap bitmapFromFile;

        private Color[][] bmp;

        private int widthPixel;

        private int heightPixel;

        private static readonly Size squadSize = new Size(16, 16);

        public ShitMazeAnalyzer(string filename)
        {
            
            this.bitmapFromFile = new Bitmap(filename, true);

            if (bitmapFromFile.Width % 10 == 2 && bitmapFromFile.Height % 10 == 2)
            {
                bmp = new Color[bitmapFromFile.Width - 2][];
                for (int i = 0; i < bmp.Length; i++)
                {
                    bmp[i] = new Color[bitmapFromFile.Height - 2];
                }

                for (int i = 0; i < bitmapFromFile.Width - 2; i++)
                {
                    for (int j = 0; j < bitmapFromFile.Height - 2; j++)
                    {
                        bmp[i][j] = bitmapFromFile.GetPixel(i, j);
                    }
                }
            }
            
        }

        private List<BitmapPartClass> bmpParts;

        public void Anamlyze()
        {

            var parts_width_count = bmp.Length / squadSize.Width;//.Width%squadSize.Width;
            var parts_height_count = bmp[0].Length / squadSize.Height;//.Height%squadSize.Height;

            bmpParts = new List<BitmapPartClass>();

            for (int i = 0; i < parts_width_count; i++)
            {
                for (int j = 0; j < parts_height_count; j++)
                {
                    var x = i*squadSize.Width;
                    var y = j*squadSize.Height;

                    var bitmap_cut_part = CutPixelSquad(bmp, x, y);

                    var bmpPartClass = new BitmapPartClass(bitmap_cut_part, i, j);

                    bmpPartClass.RecalculatePixels();

                    bmpParts.Add(bmpPartClass);
                }
            }

            {
                var sb = new StringBuilder();

                bmpParts.ForEach(x =>
                    {
                        for (int i = 0; i < x.Part.Length; i++)
                        {
                            for (int j = 0; j < x.Part[i].Length; j++)
                            {
                                if (x.Part[i][j].ToArgb() == Color.White.ToArgb())
                                {
                                    sb.Append(' ');
                                }
                                else if (x.Part[i][j].ToArgb() == Color.Black.ToArgb())
                                {
                                    sb.Append('+');
                                }
                                else
                                {
                                    var p = 5;
                                }
                            }
                        }
                        sb.Append(';');
                    }
                );

                var ls = sb.ToString().Split(';');

                File.WriteAllLines("my_file_" + DateTime.Now.Ticks.ToString() + ".txt", ls);
            }
            

            Console.ReadLine();
        }

        public void SaveToFile(string filename)
        {
            
        }



        private Color[,] CutPixelSquad(Color[][] bitmap, int x, int y)
        {
            var newSquad = new Color[squadSize.Width, squadSize.Height];


            for (int i = 0; i < squadSize.Width; i++)
            {
                for (int j = 0; y < squadSize.Height; y++)
                {
                    newSquad[i, j] = bitmap[x + i][y + j];

                    if (bitmap[x + i][ y + i].ToArgb() ==Color.Black.ToArgb())
                    {
                        var p = 5;
                    }
                }
            }

            return newSquad;
        }

        
        
    }

    public class BitmapPartClass
    {
        public BitmapPartClass(Color[,] part, int X, int Y)
        {
            this.Part = new Color[(int)Math.Sqrt(part.Length)][];
            for (int i = 0; i < Part.Length; i++)
            {
                Part[i] = new Color[(int)Math.Sqrt(part.Length)];
            }

            for (int i = 0; i < Part.Length; i++)
            {
                for (int j = 0; j < Part[i].Length; j++)
                {
                    Part[i][j] = part[i, j];
                }
            }
            
            this.X = X;
            this.Y = Y;
        }

        public Color[][] Part;

        public int X { get; private set; }
        public int Y { get; private set; }

        private string CalcString()
        {
            var sb = new StringBuilder();
            if (Part != null)
            {
                var sqr = (int)Math.Sqrt(Part.Length);
                for (int i = 0; i < sqr; i++)
                {
                    for (int j = 0; j < sqr; j++)
                    {
                        sb.Append(Part[i][j].ToString());
                    }
                }
            }

            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return this.CalcString().GetHashCode();
        }

        public void RecalculatePixels()
        {
            if (this.Part.Length == 16)
            {
                var newPart = new Color[8][];
                for (int i = 0; i < 8; i++)
                {
                    newPart[i] = new Color[8];
                }


                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        newPart[i][j] = Part[i*2][j*2];
                    }
                }
                this.Part = newPart;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < this.Part.Length; i++)
            {
                for (int j = 0; j < this.Part.Length; j++)
                {
                    sb.Append(Part[i][j].ToArgb() == Color.White.ToArgb() ? ' ' : '+');
                }
                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}