using System;

namespace aip
{
    public class Point{
        public int x;
        public int y;

        public Point(int x, int y){
            this.x = x;
            this.y = y;
        }
    }

    public class Board{
        public Point left_up;
        public Point right_down;

        public Board(Point A, Point B){
            this.left_up = A;
            this.right_down = B;
        }

        public bool CheckPoint(Point point){
            return point.x >= this.left_up.x && 
                   point.y <= this.left_up.y && 
                   point.x <= this.right_down.x && 
                   point.y >= this.right_down.y;
        }
    }

    public delegate void PointOutBorderEventHandler(Point point);

    public class PointOutHandler{
        public void HandlePointOut(Point point){
            Console.WriteLine($"Точка вышла за пределы поля: ({point.x}, {point.y})");
        }
    }

    class Program{
        static void Main(string[] args){
            Random rand = new Random();
            Point A = new Point(10, 100);
            Point B = new Point(60, 40);
            Board board = new Board(A, B);
            PointOutHandler pointOutHandler = new PointOutHandler();
            PointOutBorderEventHandler pointOutEvent = pointOutHandler.HandlePointOut;
            Point point = new Point(rand.Next(A.x, B.x), rand.Next(B.y, A.y));
            
            while (true){
                Console.WriteLine($"Текущие координаты точки: ({point.x}, {point.y})");
                point.x += rand.Next(-10, 10);
                point.y += rand.Next(-10, 10);
                
                if (!board.CheckPoint(point))
                {
                    pointOutEvent(point);
                    break;
                }
            }
        }
    }
}