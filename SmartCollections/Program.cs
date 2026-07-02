using SmartCollections.SmartStack;

namespace SmartCollections.Program
{
    internal class Program
    {
        static void Main()
        {
            var stack1 = new SmartStack<int>();
            var stack2 = new SmartStack<int>(3);
            var stack3 = new SmartStack<int>(new List<int> { 1, 2 });

            stack1.Push(1334);

            Console.WriteLine("Count:");
            Console.WriteLine(stack1.Count);
            Console.WriteLine(stack2.Count);
            Console.WriteLine(stack3.Count);
            Console.WriteLine("Capacity:");
            Console.WriteLine(stack1.Capacity);
            Console.WriteLine(stack2.Capacity);
            Console.WriteLine(stack3.Capacity);

            try
            {
                new SmartStack<int>(-1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                new SmartStack<int>(null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine($"Элемент 1 стека: {stack1.Pop()}");

            try
            {
                stack1.Pop();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
