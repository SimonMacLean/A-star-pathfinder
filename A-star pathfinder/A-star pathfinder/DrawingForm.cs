using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_star_pathfinder
{
    public partial class DrawingForm : Form
    {
        public static Random r = new Random();
        public Spot[][] grid = new Spot[30][];
        public Spot startingSpot;
        public Spot endingSpot;
        public List<Spot> openSet;
        public List<Spot> closedSet;
        public List<Spot> path;
        Timer t;
        double distanceBetween(bool useTaxiCab, int ax, int ay, int bx, int by)
        {
            if(useTaxiCab)
            {
                return Math.Abs(ax - bx) + Math.Abs(ay - by);
            }
            return Math.Sqrt(Math.Pow(ax - bx, 2) + Math.Pow(ay - by, 2));
        }
        public DrawingForm()
        {
            InitializeComponent();
            typeof(DrawingForm).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            this, new object[] { true });
            init();
        }
        public void init()
        {
            t = new Timer();
            t.Enabled = true;
            t.Interval = 1;
            t.Tick += T_Tick;
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new Spot[30];
                for (int j = 0; j < grid[i].Length; j++)
                {
                    grid[i][j] = new Spot(r.Next(3) == 1, j, i);
                }
            }
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    grid[i][j].addNeighbors(grid);
                }
            }
            grid[0][0].isBlocked = false;
            grid[grid.Length - 1][grid[grid.Length - 1].Length - 1].isBlocked = false;
            startingSpot = grid[0][0]; // top-left corner
            endingSpot = grid[grid.Length - 1][grid[grid.Length - 1].Length - 1]; // bottom-right corner
            openSet = new List<Spot>();
            closedSet = new List<Spot>();
            openSet.Add(startingSpot);
        }
        private void T_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
        private void DrawingForm_Paint(object sender, PaintEventArgs e)
        {
            if (openSet.Count > 0)
            {
                int lowestIndex = 0;
                for(int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].bestDistance < openSet[lowestIndex].bestDistance)
                        lowestIndex = i;
                }
                Spot current = openSet[lowestIndex];
                closedSet.Add(current);
                openSet.Remove(current);
                if (current.x == endingSpot.x && current.y == endingSpot.y)
                {
                    t.Enabled = false;
                }
                Spot neighbor;
                for(int i = 0; i < current.neighbors.Count; i++)
                {
                    neighbor = current.neighbors[i];
                    if (!closedSet.Contains(neighbor) && !neighbor.isBlocked)
                    {
                        bool newPath = false;
                        double tempG = current.distanceMoved + distanceBetween(false, current.x, current.y, neighbor.x, neighbor.y);
                        if (openSet.Contains(neighbor))
                        {
                            if (tempG < neighbor.distanceMoved)
                            {
                                neighbor.distanceMoved = tempG;
                                newPath = true;
                            }
                        }
                        else
                        {
                            neighbor.distanceMoved = tempG;
                            openSet.Add(neighbor);
                            newPath = true;
                        }
                        if (newPath)
                        {
                            neighbor.distanceFromEnd = distanceBetween(false, endingSpot.x, endingSpot.y, neighbor.x, neighbor.y);
                            neighbor.bestDistance = neighbor.distanceMoved + neighbor.distanceFromEnd;
                            neighbor.previous = current;
                        }
                    }
                }
                path = new List<Spot>();
                Spot temp = current;
                path.Add(temp);
                while(temp.previous != null)
                {
                    path.Add(temp.previous);
                    temp = temp.previous;
                }
            }
            else
            {
                path = new List<Spot>();
                t.Enabled = false;
            }
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    grid[i][j].Draw(e.Graphics);
                }
            }
            /*
            for (int i = 0; i < openSet.Count; i++)
            {
                openSet[i].Draw(e.Graphics, Color.Green);
            }
            for (int i = 0; i < closedSet.Count; i++)
            {
                closedSet[i].Draw(e.Graphics, Color.Red);
            }
            */
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].previous != null)
                    e.Graphics.DrawLine(Pens.Red, path[i].previous.x * Spot.Size + Spot.Size / 2, path[i].previous.y * Spot.Size + Spot.Size / 2, path[i].x * Spot.Size + Spot.Size / 2, path[i].y * Spot.Size + Spot.Size / 2);
                 //path[i].Draw(e.Graphics, Color.Blue);
            }
        }

        private void newMazeButton_Click(object sender, EventArgs e)
        {
            init();
        }
    }
    public class Spot
    {
        public const int Size = 20;
        public bool isBlocked;
        public int x;
        public int y;
        public double bestDistance;
        public double distanceMoved;
        public double distanceFromEnd;
        public Spot previous = null;
        //public Rectangle enclosingRectangle = new Rectangle();
        public Rectangle fillingRectangle = new Rectangle();
        public List<Spot> neighbors = new List<Spot>();
        public Spot(bool isBlocked, int x, int y)
        {
            this.isBlocked = isBlocked;
            this.x = x;
            this.y = y;
            //enclosingRectangle = new Rectangle(x * Size, y * Size, Size, Size);
            fillingRectangle = new Rectangle(x * Size+Size/4, y * Size+Size/4, Size/2, Size/2);
        }
        public void Draw(Graphics canvas)
        {
            //canvas.DrawRectangle(Pens.Black, enclosingRectangle);
            if (isBlocked)
                canvas.FillEllipse(Brushes.Black, fillingRectangle);
        }
        public void addNeighbors(Spot[][] grid)
        {
            if (y < grid.Length - 1)
            {
                neighbors.Add(grid[y + 1][x]);
            }
            if (y > 0)
            {
                neighbors.Add(grid[y - 1][x]);
            }
            if (x < grid[y].Length - 1)
            {
                neighbors.Add(grid[y][x + 1]);
            }
            if (x > 0)
            {
                neighbors.Add(grid[y][x - 1]);
            }

            if (y < grid.Length - 1 && x < grid[y].Length - 1)
            {
                neighbors.Add(grid[y + 1][x + 1]);
            }
            if (y > 0 && x < grid[y].Length - 1)
            {
                neighbors.Add(grid[y - 1][x + 1]);
            }
            if (y < grid.Length - 1 && x >0)
            {
                neighbors.Add(grid[y + 1][x - 1]);
            }
            if (y > 0 && x >0)
            {
                neighbors.Add(grid[y - 1][x - 1]);
            }
        }
    }
}
