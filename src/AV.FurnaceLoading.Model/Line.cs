using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AV.FurnaceLoading.Model
{
    public class Line
    {
        int X1 { get; set; }
        int Y1 { get; set; }
        int X2 { get; set; }
        int Y2 { get; set; }

        public Line(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;    
            X2 = x2;
            Y2 = y2;
        }

        //Оба отрезка вертикальны
        private bool BothSegmentsVertical(Line B)
        {
            if (X1 == B.X1)
            {
                //проверим пересекаются ли они, т.е. есть ли у них общий Y
                //для этого возьмём отрицание от случая, когда они НЕ пересекаются
                if (!((Math.Max(Y1, Y2) < Math.Min(B.Y1, B.Y2)) || (Math.Min(Y1, Y2) > Math.Max(B.Y1, B.Y2))))
                    return true;
            }
            return false;
        }

        //Один отрезок вертикальный
        private bool OneSegmentsVertical(Line B)
        {
            double Xa = X1;
            double A2 = (B.Y1 - B.Y2) / (B.X1 - B.X2);
            double b2 = B.Y1 - A2 * B.X1;
            double Ya = A2 * Xa + b2;

            if (B.X1 <= Xa && B.X2 >= Xa && Math.Min(Y1, Y2) <= Ya && Math.Max(Y1, Y2) >= Ya)
                return true;
            return false;
        }

        //Оба отрезка невертикальные
        public bool BothSegmentsNonVertical(Line B)
        {
            double A1 = (Y1 - Y2) / (X1 - X2);
            double A2 = (B.Y1 - B.Y2) / (B.X1 - B.X2);
            double b1 = Y1 - A1 * X1;
            double b2 = B.Y1 - A2 * B.X1;

            if (A1 == A2)
                return false; //отрезки параллельны

            //Xa - абсцисса точки пересечения двух прямых
            double Xa = (b2 - b1) / (A1 - A2);

            if ((Xa < Math.Max(X1, B.X1)) || (Xa > Math.Min(X2, B.X2)))
                return false; //точка Xa находится вне пересечения проекций отрезков на ось X 
            return true;
        }

        public bool CollisionLine(Line B)
        {
            int DXA = X1 - X2;
            int DXB = B.X1 - B.X2;
            if (DXA == 0 && DXB == 0)
                return BothSegmentsVertical(B);
            else if (DXA == 0)
                return OneSegmentsVertical(B);
            else if (DXB == 0)
                return B.OneSegmentsVertical(this);
            else
                return BothSegmentsNonVertical(B);
        }

        public int WherePoint(Coordinate point)
        {
            return (X2 - X1) * (point.Y - Y1) - (point.X - X1) * (Y2 - Y1);
        }

    }
}
