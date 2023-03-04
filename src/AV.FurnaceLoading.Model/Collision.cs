using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AV.FurnaceLoading.Model
{
    public class Collision
    {
        

        public Coordinate CoordinateAdjustment(Coordinate _main, Coordinate second, double angleRotation)
        {
            int onePart = (int)((_main.X - second.X) * Math.Cos(angleRotation) - (_main.Y - second.Y) * Math.Sin(angleRotation) + second.X);
            int twoPart = (int)((_main.X - second.X) * Math.Sin(angleRotation) + (_main.Y - second.Y) * Math.Cos(angleRotation) + second.Y);
            return new Coordinate(onePart, twoPart);
        }

        public bool CircleVsBox(CircleLoadPlace circle, Cassette cassette)
        {
            var Radius = circle.Diameter / 2;
            var leftPoint = circle.Center.X - Radius;
            var rightPoint = circle.Center.X + Radius;
            var topPoint = circle.Center.Y + Radius;
            var bottomPoint = circle.Center.Y - Radius;
            if (leftPoint <= 0 || bottomPoint <= 0 || rightPoint >= cassette.Width || topPoint >= cassette.Height)
                return true;
            return false;
        }

        private List<Coordinate> ListCoordinates(RectLoadPlace rect)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            var halfWidth = rect.Width / 2;
            var halfHeight = rect.Height / 2;
            coordinates.Add(new Coordinate(rect.Center.X - halfWidth, rect.Center.Y - halfHeight));
            coordinates.Add(new Coordinate(rect.Center.X + halfWidth, rect.Center.Y - halfHeight));
            coordinates.Add(new Coordinate(rect.Center.X + halfWidth, rect.Center.Y + halfHeight));
            coordinates.Add(new Coordinate(rect.Center.X - halfWidth, rect.Center.Y + halfHeight));
            var angleRotation = rect.GetRotation() * Math.PI / 180;
            for (int i = 0; i < 4; i++)
            {
                coordinates[i] = CoordinateAdjustment(coordinates[i], rect.Center, angleRotation);
            }
            return coordinates;
        }

        /// <summary>
        /// Проверка колизии 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="cassette"></param>
        /// <returns></returns>
        public bool RectVsBox(RectLoadPlace rect, Cassette cassette)
        {
            List<Coordinate> coordinates = ListCoordinates(rect);
            for (int i = 0; i < 4; i++)
            {
                if (coordinates[i].X <= 0 || coordinates[i].Y <= 0 || coordinates[i].X >= cassette.Width || coordinates[i].Y >= cassette.Height)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool CircleVsCircleCheck(CircleLoadPlace A, CircleLoadPlace B)
        {
            double RadiusSum = (A.Diameter + B.Diameter) / 2;
            double tempX = A.Center.X - B.Center.X;
            double tempY = A.Center.Y - B.Center.Y;
            return (tempX * tempX + tempY * tempY) <= RadiusSum * RadiusSum;
        }

        /// <summary>
        /// Возвращает true если есть колизия между кругом и прямоугольником
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool CircleVsRectCheck(CircleLoadPlace circle, RectLoadPlace rect)
        {
            var halfWidth = rect.Width / 2;
            var halfHeight = rect.Height / 2;
            var copyCircle = new CircleLoadPlace(circle.Diameter, circle.Center);
            if (rect.Rotation != RectLoadPlace.RotationType.None)
            {
                var angleRotation = rect.GetRotation() * Math.PI / 180;
                copyCircle.Center = CoordinateAdjustment(circle.Center, rect.Center, angleRotation);
            }

            var delteX = copyCircle.Center.X - Math.Max(rect.Center.X - halfWidth, Math.Min(copyCircle.Center.X, rect.Center.X + halfWidth));
            var deltaY = copyCircle.Center.Y - Math.Max(rect.Center.Y - halfHeight, Math.Min(copyCircle.Center.Y, rect.Center.Y + halfHeight));
            var Radius = copyCircle.Diameter / 2;
            return (delteX * delteX + deltaY * deltaY) <= (Radius * Radius);
        }

        //public bool RectVsRectCheck(RectLoadPlace A, RectLoadPlace B)
        //{
        //    List<Coordinate> rectA = ListCoordinates(A);
        //    List<Coordinate> rectB = ListCoordinates(B);
        //    for(int i = 0; i < 4; ++i)
        //    {
        //        Line lineA;
        //        if (i == 3)
        //            lineA = new Line(rectA[i].X, rectA[i].Y, rectA[0].X, rectA[0].Y);
        //        else
        //            lineA = new Line(rectA[i].X, rectA[i].Y, rectA[i + 1].X, rectA[i + 1].Y);
        //        for(int j = 0; j < 4; ++j)
        //        {
        //            Line lineB;
        //            if(j == 3)
        //                lineB = new Line(rectB[j].X, rectB[j].Y, rectB[0].X, rectB[0].Y);
        //            else
        //                lineB = new Line(rectB[j].X, rectB[j].Y, rectB[j + 1].X, rectB[j + 1].Y);
        //            if (lineA.CollisionLine(lineB))
        //                return true;
        //        }
        //    }
        //    return false;
        //}


        public bool RectVsRectCheck(RectLoadPlace A, RectLoadPlace B)
        {
            List<Coordinate> rectA = ListCoordinates(A);
            List<Coordinate> rectB = ListCoordinates(B);
            for (int i = 0; i < 4; ++i)
            {
                List<int> ResulLineWithPoin = new List<int>();
                for (int j = 0; j < 4; ++j)
                {
                    Line lineA;
                    if (j == 3)
                        lineA = new Line(rectA[j].X, rectA[j].Y, rectA[0].X, rectA[0].Y);
                    else
                        lineA = new Line(rectA[j].X, rectA[j].Y, rectA[j + 1].X, rectA[j + 1].Y);
                    ResulLineWithPoin.Add(lineA.WherePoint(rectB[i]));
                }
                if (ResulLineWithPoin[0] >= 0 && ResulLineWithPoin[1] >= 0 && ResulLineWithPoin[2] >= 0 && ResulLineWithPoin[3] >= 0)
                    return true;
            }
            return false;
        }
    }
}
