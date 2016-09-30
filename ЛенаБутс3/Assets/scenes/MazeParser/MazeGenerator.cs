using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scenes.MazeParser
{
    public class MazeGenerator : MonoBehaviour
    {
        public TextAsset maze_file_name;

        public Vector2 CellSize = new Vector2(1f, 1f);

        public GameObject LeftWall;
        public GameObject TopWall;
        public GameObject TopLeftWall;

        private MazeCellEnum[][] maze_cells;
        private int rows;
        private int columns;

        // Use this for initialization
        void Start ()
        {

            var all_text = maze_file_name.text;

            var lines = new List<string>(all_text.Split('\n'));
            lines.RemoveAll(x => string.IsNullOrEmpty(x));

            rows = lines.Count;

            columns = lines[0].Length;


            maze_cells = new MazeCellEnum[rows][];
            for (int i = 0; i < rows; i++)
            {   
                maze_cells[i] = new MazeCellEnum[columns];
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    maze_cells[i][j] = ConverFromChar(lines[i][j]);
                }
            }


            GenerateMaze();
        }

        private void GenerateMaze()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    CreateCell(maze_cells[i][j], i, j);
                }
            }
        }

        private void CreateCell(MazeCellEnum mazeCellEnum, int i, int j)
        {
            if (mazeCellEnum == MazeCellEnum.None)
            {
                return;
            }

            //var pos = new Vector2(CellSize.x*i, CellSize.y*(-j));
            var pos = new Vector2(CellSize.x*j, CellSize.y*(rows - i));

            GameObject go = null;
            if (mazeCellEnum == (MazeCellEnum.Top | MazeCellEnum.Left))
            {
                go = (GameObject)GameObject.Instantiate(TopLeftWall, pos, Quaternion.identity);
            }
            else if (mazeCellEnum == MazeCellEnum.Top)
            {
                go = (GameObject)GameObject.Instantiate(TopWall, pos, Quaternion.identity);
            }
            else if (mazeCellEnum == MazeCellEnum.Left)
            {
                go = (GameObject)GameObject.Instantiate(LeftWall, pos, Quaternion.identity);
            }
            go.name = i + " " + j;
        }

        // Update is called once per frame
        void Update ()
        {

        }

        private MazeCellEnum ConverFromChar(char c)
        {
            if (c == '+')
            {
                return MazeCellEnum.Left | MazeCellEnum.Top;
            }
            if (c == '-')
            {
                return MazeCellEnum.Top;
            }
            if (c == '|')
            {
                return  MazeCellEnum.Left;
            }

            return MazeCellEnum.None;
        }
    }

    [Flags]
    public enum MazeCellEnum
    {
        None,
        Top,
        Left,
    }
}
