namespace Geometry.Shapes
{
    /// <summary>
    /// Представляет прямоугольник с координатами левого верхнего угла, шириной и высотой
    /// </summary>
    public class Rectangle
    {
        private int _x;
        private int _y;
        private double _width;
        private double _height;

        /// <summary>
        /// Координата X левого верхнего угла прямоугольника.
        /// Значение должно быть неотрицательным.
        /// </summary>
        public required int X
        {
            get => _x;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(X), "Координата X должна быть неотрицательной!");
                _x = value;
            }
        }

        /// <summary>
        /// Координата Y левого верхнего угла прямоугольника.
        /// Значение должно быть неотрицательным.
        /// </summary>
        public required int Y
        {
            get => _y;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(Y), "Координата Y должна быть неотрицательной!");
                _y = value;
            }
        }

        /// <summary>
        /// Ширина прямоугольника.
        /// Значение должно быть больше нуля.
        /// </summary>
        public required double Width
        {
            get => _width;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(Width), "Ширина должна быть больше нуля!");
                _width = value;
            }
        }

        /// <summary>
        /// Высота прямоугольника.
        /// Значение должно быть больше нуля.
        /// </summary>
        public required double Height
        {
            get => _height;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(Height), "Высота должна быть больше нуля!");
                _height = value;
            }
        }

        /// <summary>
        /// Периметр прямоугольника.
        /// </summary>
        public double Perimeter => 2 * (Width + Height);

        /// <summary>
        /// Площадь прямоугольника.
        /// </summary>
        public double Area => Width * Height;
    }
}