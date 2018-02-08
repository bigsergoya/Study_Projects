using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualChart3D.Common.DataReader
{
    class SCVDataReader : IUniversalReader
    {
        private const int Compensation = 1;
        private const int FirstNumericLine = 1;

        private InputFileType _windowFileType;
        private List<string> _firstColumn;
        private List<List<string>> _fileData;
        private List<int> _nonReadingColumn;
        private string _sourceMatrixFile;

        //Я это сделал потому, что все - тлен :(
        private string _classNameColumn;
        private string _objectNameColumn;
        private string[] _classNameColumnData;
        private string[] _objectNameColumnData;
        private double[,] _arraySource;
        private SourceFileMatrixType _inputMatrixType;
        private int _minkovskiDegree;
        private bool _needRecalculate;

        public SCVDataReader(InputFileType windowFileType)
        {
            this._windowFileType = windowFileType;
            _firstColumn = new List<string>();
            _nonReadingColumn = new List<int>();
            _needRecalculate = false;
            _minkovskiDegree = 2;
        }

        private void RecalculateArraySource()
        {
            int n;
            int m;

            CalculateCountOfDeletedColumns(out n, out m);
            _arraySource = new double[n, m];

            //Переписать так, чтобы читалось по столбцам, а не по строкам.
            try
            {
                int numericDataIndex = 0;
                for (int i = Compensation; i < n + Compensation; i++)
                {
                    for (int j = 0; j < _firstColumn.Count; j++)
                    {
                        if (_nonReadingColumn.Contains(j))
                        {
                            continue;
                        }

                        _arraySource[i - Compensation, numericDataIndex] = double.Parse(_fileData[i][j], CultureInfo.InvariantCulture);
                        numericDataIndex++;
                    }

                    numericDataIndex = 0;
                }
            }
            catch
            {
                _arraySource = null;
                return;
            }
        }

        /// <summary>
        /// Возвращает число от 0 до 2 в зависимости от наличия дополнительных стобцов
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        private void CalculateCountOfDeletedColumns(out int n, out int m)
        {
            n = _fileData.Count - Compensation;
            m = _firstColumn.Count - _nonReadingColumn.Count;
            //m = _firstColumn.Count - Convert.ToInt32(IsNull(_classNameColumn)) - Convert.ToInt32(IsNull(_objectNameColumn));
        }

        private bool IsNull(object something)
        {
            if (something == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// возвращает столбец данных на основе его имени 
        /// </summary>
        /// <param name="classNameColumn">имя столбца</param>
        /// <returns>столбец данных</returns>
        private string[] GetColumn(string classColumn)
        {

            int count = (_fileData.Count);
            string[] data = new string[count - Compensation];
            int column = _firstColumn.FindIndex(p => p == classColumn);

            for (int i = 1; i < count; i++)
            {
                data[i - Compensation] = _fileData[i][column];
            }

            _nonReadingColumn.Add(column);
            return data;
        }

        private List<string> DivideLine(string s, char separator)
        {
            List<string> subLine = new List<string>();
            string[] parts;

            s = s.Trim();
            parts = s.Split(separator);

            subLine.AddRange(parts);

            return subLine;
        }

        public List<string> FirstLine => _firstColumn;
        public string[] ClassNameColumnData { get => _classNameColumnData; }
        public string[] ObjectNameColumnData { get => _objectNameColumnData; }

        public string ClassNameColumn {
            get {
                return _classNameColumn;
            }

            set {
                _classNameColumn = value;
                string[] nameColumn = GetColumn(_classNameColumn);
                _classNameColumnData = nameColumn;
                _needRecalculate = true;
            }
        }

        public string ObjectNameColumn {
            get {
                return _objectNameColumn;
            }

            set {
                _objectNameColumn = value;
                string[] objectColumn = GetColumn(_objectNameColumn);
                _objectNameColumnData = objectColumn;
                _needRecalculate = true;
            }

        }

        public double[,] ArraySource {
            get {
                if (_needRecalculate)
                {
                    RecalculateArraySource();
                    _needRecalculate = false;
                    _nonReadingColumn.Clear();
                }

                return _arraySource;
            }
        }

        public SourceFileMatrixType SourceMatrixType => _inputMatrixType;

        public string SourceMatrixFile => _sourceMatrixFile;

        public InputFileType InputFileType { get => _windowFileType; set => _windowFileType = value; }
        public int MinkovskiDegree { get => _minkovskiDegree; set => _minkovskiDegree = value; }

        public bool CheckSourceMatrix(string SourceMatrixFile, SourceFileMatrixType SourceMatrixType)
        {
            return ReadFile(SourceMatrixFile, SourceMatrixType);
        }

        private bool ReadFile(string SourceMatrixFile, SourceFileMatrixType SourceMatrixType)
        {
            /*if (string.IsNullOrEmpty(SourceMatrixFile) || !File.Exists(SourceMatrixFile))
            {
                return false;
            }*/

            _fileData = new List<List<string>>();

            try
            {
                StreamReader fs = new StreamReader(SourceMatrixFile);

                string s = fs.ReadLine();
                while (s != null)
                {
                    List<string> parts = new List<string>();

                    parts = DivideLine(s, ',');
                    _fileData.Add(parts);
                    s = fs.ReadLine();
                }

                _firstColumn = _fileData[0];
            }
            catch (IOException e)
            {
                return false;
            }

            _inputMatrixType = SourceMatrixType;
            _sourceMatrixFile = SourceMatrixFile;
            _needRecalculate = true;
            return true;
        }
    }
}
