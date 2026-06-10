using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*Написать класс Rectangle, задающий прямоугольник с указанными координатами левого верхнего угла, 
 * шириной и высотой, а также свойствами, позволяющими узнать периметр и площадь прямоугольника. 
 * Обеспечить нахождение объекта в заведомо корректном состоянии (отрицательные значения недопустимы). 
 * Написать программу, демонстрирующую использование данного прямоугольника.*/
namespace Task3
{
    public class Rectangle
    {
        private int _x;
        private int _y;
        private double _width;
        private double _heigth;
        public int X
        {
            get => _x;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Координата X должна быть неотрицательной!");
                _x = value;
            }
        }
        public int Y
        {
            get => _y;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Координата Y должна быть неотрицательной!");
                _y = value;
            }
        }
        public double Width
        {
            get => _width;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Ширина должна быть положительной!");
                _width = value;
            }
        }
        public double Heigth
        {
            get => _heigth;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Высота должна быть положительной!");
                _heigth = value;
            }
        }

        public double Square
        {
            get => Width * Heigth;
        }

        public double Perimeter
        {
            get => 2 * (Width + Heigth);
        }

        public Rectangle(int x, int y, int width, int heigth)
        {
            X = x;
            Y = y;
            Width = width;
            Heigth = heigth;
        }
    }
}