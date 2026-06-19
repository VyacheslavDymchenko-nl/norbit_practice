namespace Geometry.Program
{
    using Geometry.Shapes;
    internal class Program
    {
        public static void PrintRectangle(Rectangle rectangle)
        {
            Console.WriteLine($"X: {rectangle.X}");
            Console.WriteLine($"Y: {rectangle.Y}");
            Console.WriteLine($"Ширина: {rectangle.Width}");
            Console.WriteLine($"Высота: {rectangle.Height}");
            Console.WriteLine($"Периметр: {rectangle.Perimeter}");
            Console.WriteLine($"Площадь: {rectangle.Area}");
        }
        static void Main()
        {
            var shape = new Rectangle
            {
                X = 0,
                Y = 0,
                Width = 12.5,
                Height = 7.4
            };

            PrintRectangle(shape);

            shape.X = 3;
            shape.Y = 5;
            shape.Width = 23.4;
            shape.Height = 100;

            PrintRectangle(shape);

            try
            {
                shape.X = -1;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                shape.Y = -2;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                shape.Width = 0;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                shape.Height = -4;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
