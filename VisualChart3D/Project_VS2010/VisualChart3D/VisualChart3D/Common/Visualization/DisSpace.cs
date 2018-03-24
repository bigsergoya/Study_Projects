// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;

namespace VisualChart3D.Common.Visualization
{
    public class DisSpace
    {
        private const int DefaultFirstObjectID = 1;
        private const int DefaultSecondObjectID = 2;
        private const int DefaultThirdObjectID = 3;
        private const int ObjectsCount = 3;

        private int _firstBasisObject;
        private int _secondBasisObject;
        private int _thirdBasisObject;
        private bool _basicObjectsColorMode;
        private double[,] _arraySource;
        private int _countOfObjects;
        private ITimer _timer;
        private Space _space = Space.TwoDimensional;
        private double[,] _coords;

        public int FirstBasisObject { get => _firstBasisObject; set => _firstBasisObject = value; }
        public int SecondBasisObject { get => _secondBasisObject; set => _secondBasisObject = value; }
        public int ThirdBasisObject { get => _thirdBasisObject; set => _thirdBasisObject = value; }
        public Space Space { get => _space; set => _space = value; }
        public bool BasicObjectsColorMode { get => _basicObjectsColorMode; set => _basicObjectsColorMode = value; }
        public double[,] ArraySource { get => _arraySource; }

        public DisSpace(double[,] arraySource, int countOfObjects)
        {
            _timer = new CustomTimer();
            _firstBasisObject = DefaultFirstObjectID;
            _secondBasisObject = DefaultSecondObjectID;
            _thirdBasisObject = DefaultThirdObjectID;
            _coords = null;
            _basicObjectsColorMode = false;
            _arraySource = arraySource;
            _countOfObjects = countOfObjects;
        }

        public int[] getBasicObjectsArray()
        {
            int[] arr = new int[ObjectsCount];
            arr[0] = _firstBasisObject;
            arr[1] = _secondBasisObject;
            arr[2] = _thirdBasisObject;
            return arr;

        }

        //public int getBasicObjectsNumber() { return (int)Char.GetNumericValue(space[0]); }
        public int getBasicObjectsNumber() { return (int)_space; }

        public DisSpace setBasicObjects(int firstObject, int secondObject, int thirdObject)
        {
            _firstBasisObject = firstObject;
            _secondBasisObject = secondObject;
            _thirdBasisObject = thirdObject;
            return this;
        }

        public double[,] ToProject()
        {
            _timer.Start();

            if (_space.Equals(Space.TwoDimensional))
            {
                _coords = new double[2, _countOfObjects];

                for (int j = 0; j < _countOfObjects; j++)
                {
                    _coords[0, j] = _arraySource[_firstBasisObject - 1, j];
                    _coords[1, j] = _arraySource[_secondBasisObject - 1, j];
                }
            }
            else
            {
                _coords = new double[3, _countOfObjects];

                for (int j = 0; j < _countOfObjects; j++)
                {
                    _coords[0, j] = _arraySource[_firstBasisObject - 1, j];
                    _coords[1, j] = _arraySource[_secondBasisObject - 1, j];
                    _coords[2, j] = _arraySource[_thirdBasisObject - 1, j];
                }
            }

            _timer.Stop();
            return _coords;
        }
    }
}
