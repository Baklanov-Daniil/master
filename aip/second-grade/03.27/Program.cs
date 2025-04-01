// старт, финиш
// 3 объекта которые учавствуют в гонке, каждый обхъект характеризуется:
// имя, начальная скорость, расстояние
// необходимо обработать событие - выигрыш одного из участника (кто выиграл)
// смена скорости у каждого объекта через t интервалов. Смена скорости рандомна от 0 до 60
// Интервал задаем сами, начальную скорость тоже

using System;

namespace aip{
    class Program{
        
        public class Racer{
            public string name;
            public int speed;
            public int distance=0;
            public Racer(string name, int speed){   
                this.name = name;
                this.speed = speed;
            }
        }

        public delegate void RacerWinEventHandler(Racer racer);

        public class RacerWinHandler{
            public void HandleRacerWin(Racer racer){
                Console.WriteLine($"Гонка окончена. Победил: {racer.name}");
            }
        }


        static void Main(string[] args) {

            static bool IsFinish(Racer racer) => racer.distance >= 1000;

            Racer racer1 = new Racer("первый", 10);
            Racer racer2 = new Racer("второй", 10);
            Racer racer3 = new Racer("третий", 10);
            Random rand = new Random();
            RacerWinHandler racerWinHandler = new RacerWinHandler();
            RacerWinEventHandler racerWinEventHandler = racerWinHandler.HandleRacerWin;
            int t = 7;
            while (true){
                racer1.speed = rand.Next(0, 60);
                racer1.distance += racer1.speed*t;
                if (IsFinish(racer1)){ 
                    racerWinEventHandler(racer1);
                    break;
                }
                racer2.speed = rand.Next(0, 60);
                racer2.distance += racer2.speed*t;
                if (IsFinish(racer2)){ 
                    racerWinEventHandler(racer2);
                    break;
                }
                racer3.speed = rand.Next(0, 60);
                racer3.distance += racer3.speed*t;
                if (IsFinish(racer3)){ 
                    racerWinEventHandler(racer3);
                    break;
                }
            }

        }
    }
}

