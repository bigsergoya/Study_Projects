// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;

namespace VisualChart3D.Common
{
    public static class ExtensionsMethods
    {
        /// <summary>
        /// Преобразует цвет Windows Forms в цвет WPF
        /// </summary>
        /// <param name="color">Цвет WindowsForms</param>
        /// <returns>Цвет WPF</returns>
        public static System.Windows.Media.Color ToColor(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Получить русское название из <see cref="Shapes"/>
        /// </summary>
        /// <param name="shape">текущая фигура</param>
        /// <returns>Русское название</returns>
        public static string GetRusName(this Shapes shape)
        {
            switch (shape)
            {
                case Shapes.Bar3D:
                    return "Брусок";
                case Shapes.Cone3D:
                    return "Конус";
                case Shapes.Cylinder3D:
                    return "Цилиндр";
                case Shapes.Ellipse3D:
                    return "Шар";
                case Shapes.Pyramid3D:
                    return "Пирамида";
                default:
                    throw new ArgumentOutOfRangeException("shape");
            }
        }
    }
}
