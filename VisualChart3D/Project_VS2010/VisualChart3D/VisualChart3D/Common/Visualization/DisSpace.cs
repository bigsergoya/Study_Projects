// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;

namespace VisualChart3D.Common.Visualization
{
    public class DisSpace
    {
        private int firstBasisObject;
        private int secondBasisObject;
        private int thirdBasisObject;
        private bool basicObjectsColorMode;

        private Space space = Space.TwoDimensional;
        private double[,] coords;

        public int FirstBasisObject { get => firstBasisObject; set => firstBasisObject = value; }
        public int SecondBasisObject { get => secondBasisObject; set => secondBasisObject = value; }
        public int ThirdBasisObject { get => thirdBasisObject; set => thirdBasisObject = value; }
        public Space Space { get => space; set => space = value; }
        public bool BasicObjectsColorMode { get => basicObjectsColorMode; set => basicObjectsColorMode = value; }

        public DisSpace()
        {
            firstBasisObject = 1;
            secondBasisObject = 2;
            thirdBasisObject = 3;
            coords = null;
            basicObjectsColorMode = false;
        }

        public int[] getBasicObjectsArray()
        {
            int[] arr = new int[3];
            arr[0] = firstBasisObject;
            arr[1] = secondBasisObject;
            arr[2] = thirdBasisObject;
            return arr;

        }

        //public int getBasicObjectsNumber() { return (int)Char.GetNumericValue(space[0]); }
        public int getBasicObjectsNumber() { return (int)space; }

        public DisSpace setBasicObjects(int firstObject, int secondObject, int thirdObject)
        {
            firstBasisObject = firstObject;
            secondBasisObject = secondObject;
            thirdBasisObject = thirdObject;
            return this;
        }

        public double[,] GetCoords(Engine SetFile)
        {
            if (space.Equals(Space.TwoDimensional))
            {
                coords = new double[2, SetFile.CountObjects];

                for (int j = 0; j < SetFile.CountObjects; j++)
                {
                    coords[0, j] = SetFile.ArraySource[firstBasisObject - 1, j];
                    coords[1, j] = SetFile.ArraySource[secondBasisObject - 1, j];
                }
            }
            else
            {
                coords = new double[3, SetFile.CountObjects];

                for (int j = 0; j < SetFile.CountObjects; j++)
                {
                    coords[0, j] = SetFile.ArraySource[firstBasisObject - 1, j];
                    coords[1, j] = SetFile.ArraySource[secondBasisObject - 1, j];
                    coords[2, j] = SetFile.ArraySource[thirdBasisObject - 1, j];
                }
            }

            return coords;
        }
    }
}
