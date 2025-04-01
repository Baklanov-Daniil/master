using System;

namespace aip{
    class Program{

        public interface Imethods{
            double petrimetr();
            double area();
        }



        class Shape{
            string name;
        }

        class Circle: Shape, Imethods{
            string name = "Окружность";
            int radius;

            public Circle(int radius){
                this.radius = radius;
            }

            public double petrimetr(){
                return 2*Math.PI*this.radius;
            }

            public double area(){
                return Math.Pow(this.radius, 2)*Math.PI;
            }
        }

        class Square: Shape, Imethods{
            string name = "Квадарт";
            int len;

            public Square(int len){
                this.len = len;
            }

            public double petrimetr(){
                return this.len*4;
            }

            public double area(){
                return Math.Pow(this.len, 2);
            }
        }
        
        class Triangle: Shape, Imethods{
            string name = "Равносторонний треугольник";
            int len;

            public Triangle(int len){
                this.len = len;
            }

            public double petrimetr(){
                return this.len*3;
            }

            public double area(){
                return Math.Sqrt(3)*Math.Pow(this.len, 2)/4;
            }
        }
        static void Main(string[] args){
            Circle circle = new Circle(1);
            Square square = new Square(2);
            Triangle triangle = new Triangle(3);
            Console.WriteLine($"Круг: L={circle.petrimetr()}, S={circle.area()}");
            Console.WriteLine($"Квадарт: L={square.petrimetr()}, S={square.area()}");
            Console.WriteLine($"Равносторонний треугольник: L={triangle.petrimetr()}, S={triangle.area()}");
            
        }
    }
}