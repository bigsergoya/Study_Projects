namespace VisualChart3D.Common
{
    /// <summary>
    /// Режим визуализации
    /// </summary>
    enum TypePlot
    {
        /// <summary>
        /// Визуализация основных компонентов
        /// </summary>
        Main,

        /// <summary>
        /// Визуализация дочерних компонентов
        /// </summary>
        Subsidiary
    };

    /// <summary>
    /// Типы исходной матрицы
    /// </summary>
    public enum SourceFileMatrix
    {
        MatrixDistance,
        ObjectAttribute
    };

    /// <summary>
    /// Тип алгоритма, используемого для визуализации данных
    /// </summary>
    public enum AlgorithmType
    {
        FastMap,
        DisSpace,
        KohonenMap,
        SammonsMap
    };

    public enum Shapes
    {
        Bar3D,
        Cone3D,
        Cylinder3D,
        Ellipse3D,
        Pyramid3D
    }

    /// <summary>
    /// Размерность пространства, в которое будет проецироваться исходный набо данных.
    /// </summary>
    public enum Space
    {
        TwoDimensional = 2,
        ThreeDimensional = 3
    }

    /// <summary>
    /// Возможные способы для задания классов объктов
    /// </summary>
    public enum ClassInfoType
    {
        /// <summary>
        /// Объекты представлены файлом, где каждая строка равноценна объекту, а ее значение - класс объекта.
        /// </summary>
        OneToOne,

        /// <summary>
        /// Объекты представлены файлом, где каждая строка равноценна классу объектов, а ее значение - число объектов в классе.
        /// Число объектов - порядковое. 50, 50, 50 - это от 1 до 50, затем от 50 до 100, затем от 100 до 150.
        /// </summary>
        CountObj,

        /// <summary>
        /// Объекты представлены файлом, где каждая строка равноценна классу объектов, 
        /// а ее значение - начальный индекс данного типа объектов в общем массиве данных.
        /// </summary>
        StartObjects
    }

    /// <summary>
    /// Метрика
    /// </summary>
    public enum FastMapMetric
    {
        /// <summary>
        /// Евклидова метрика
        /// </summary>
        Euclidean,

        /// <summary>
        /// Неевклидова метрика
        /// </summary>
        NonEuclidean
    }
}
