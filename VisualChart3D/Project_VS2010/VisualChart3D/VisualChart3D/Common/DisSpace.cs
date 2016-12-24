// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualChart3D
{
    class DisSpace
    {
        private int firstBasisObject;
        private int secondBasisObject; 
        private int thirdBasisObject;
        private bool basicObjectsColorMode;

        private string space = "2D";
        private double[,] coords;
        public DisSpace()
        {
            firstBasisObject = 1;
            secondBasisObject = 2;
            thirdBasisObject = 3;
            coords = null;
            basicObjectsColorMode = false;
        }
        public int getFirstBasicObject(){ return firstBasisObject; }
        public int getSecondBasicObject(){ return secondBasisObject; }
        public int getThirdBasicObject(){ return thirdBasisObject; }
        public bool getBasicObjectsColorMode() { return basicObjectsColorMode; }
        public string getSpace(){ return space; }
        public int[] getBasicObjectsArray(){
            int[] arr = new int[3];
            arr[0] = firstBasisObject;
            arr[1] = secondBasisObject;
            arr[2] = thirdBasisObject;
            return arr;
            
        }
        public DisSpace setBasicObjectsMode(bool basObjectsMode){
            basicObjectsColorMode = basObjectsMode;
            return this;
        }
        public int getBasicObjectsNumber(){ return (int)Char.GetNumericValue(space[0]); }
        public DisSpace setSizeOfSpace(string size){
            space = size;
            return this;
        }
        public DisSpace setBasicObjects(int firstObject, int secondObject, int thirdObject){

            firstBasisObject = firstObject;
            secondBasisObject = secondObject;
            thirdBasisObject = thirdObject;
            return this;
        }
        public double[,] GetCoords(SettingsFiles SetFile)
        {
            if (space.Equals("2D"))
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
