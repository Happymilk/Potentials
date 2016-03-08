using System;
using System.Drawing;

namespace WindowsFormsApplication1
{
    public class Function
    {
        public Function(int xKoef, int yKoef, int xyKoef, int freeKoef)
        {
            XKoef = xKoef;
            YKoef = yKoef;
            XyKoef = xyKoef;
            FreeKoef = freeKoef;
        }

        public int XKoef { get; set; }
        public int YKoef { get; set; }
        public int XyKoef { get; set; }
        public int FreeKoef { get; set; }

        public int GetValue(Point point)
        {
            return FreeKoef + XKoef * point.X + YKoef * point.Y + XyKoef * point.X * point.Y;
        }

        public double GetY(double x)
        {
            return -(XKoef * x + FreeKoef) / (XyKoef * x + YKoef);
        }

        public override string ToString()
        {
            if (XyKoef != 0)
                return "y=(" + -XKoef + "*x" + (-FreeKoef < 0 ? "" : "+") + -FreeKoef + ")/(" +
                       XyKoef + "*x" + (YKoef < 0 ? "" : "+") + YKoef + ")";
            if (YKoef != 0)
            {
                return "y=" + -(double)XKoef / YKoef + "*x" +
                       (-(double)FreeKoef / YKoef < 0 ? "" : "+") + -(double)FreeKoef / YKoef;
            }
            return "x=" + -(double)FreeKoef / XKoef;
        }

        public static Function operator +(Function first, Function second)
        {
            return new Function(first.XKoef + second.XKoef, first.YKoef + second.YKoef,
                first.XyKoef + second.XyKoef, first.FreeKoef + second.FreeKoef);
        }

        public static Function operator *(int koef, Function function)
        {
            return new Function(koef * function.XKoef, koef * function.YKoef, koef * function.XyKoef,
                koef * function.FreeKoef);
        }
    }

    public class Potentials
    {
        private const int classCount = 2;
        private const int iterationsCount = 50;
        private int correction;

        public Function GetFunction(Point[][] teachingPoints)
        {
            var result = new Function(0, 0, 0, 0);
            bool nextIteration = true;
            int iterationNumber = 0;
            correction = 1;

            while (nextIteration && iterationNumber < iterationsCount)
            {
                iterationNumber++;
                nextIteration = DoOneIteration(teachingPoints, ref result);
            }
            return result;
        }

        private bool DoOneIteration(Point[][] teachingPoints, ref Function result)
        {
            bool nextIteration = false;

            for (int classNumber = 0; classNumber < classCount; classNumber++)
            {
                for (int i = 0; i < teachingPoints[classNumber].Length; i++)
                {
                    result += correction*PartFunction(teachingPoints[classNumber][i]);
                    int index = (i + 1)%teachingPoints[classNumber].Length;
                    int nextClassNumber = index == 0 ? (classNumber + 1)%classCount : classNumber;
                    Point nextPoint = teachingPoints[nextClassNumber][index];
                    correction = GetNewCorrection(nextPoint, result, nextClassNumber);
                    if (correction != 0) nextIteration = true;
                }
            }

            return nextIteration;
        }

        private int GetNewCorrection(Point nextPoint, Function result, int nextClassNumber)
        {
            int functionValue = result.GetValue(nextPoint);
            if (functionValue <= 0 && nextClassNumber == 0)
            {
                return 1;
            }
            if (functionValue > 0 && nextClassNumber == 1)
            {
                return -1;
            }
            return 0;
        }


        private Function PartFunction(Point point)
        {
            return new Function(4*point.X, 4*point.Y, 16*point.X*point.Y, 1);
        }
    }
}