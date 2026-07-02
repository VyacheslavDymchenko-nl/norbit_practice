using System.Collections;

namespace SmartCollections.SmartStack
{
    /// <summary>
    /// Представляет стек, реализованный на основе массива.
    /// </summary>
    public class SmartStack<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _count;

        /// <summary>
        /// Количество элементов в стеке.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Внутренняя емкость стека.
        /// </summary>
        public int Capacity => _items.Length;

        /// <summary>
        /// Создает пустой стек с начальной емкостью по умолчанию.
        /// </summary>
        public SmartStack()
        {
            _items = new T[4];
        }

        /// <summary>
        /// Создает пустой стек с указанной начальной емкостью.
        /// </summary>
        /// <param name="capacity">Начальная емкость стека.</param>
        public SmartStack(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Отрицательная длина стека!");
            }

            _items = new T[capacity];
        }

        /// <summary>
        /// Создает стек, содержащий элементы указанной коллекции.
        /// </summary>
        /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="collection"/> равен <see langword="null"/></exception>
        /// <param name="collection">Коллекция, элементы которой будут помещены в стек.</param>
        public SmartStack(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection), "Не существующая коллекция!");
            }

            _items = new T[4];

            foreach (var item in collection)
            {
                Push(item);
            }
        }

        /// <summary>
        /// Добавляет элемент на вершину стека. 
        /// </summary>
        /// <param name="item">Элемент.</param>
        public void Push(T item)
        {
            if (_count < _items.Length)
            {
                _items[_count] = item;
                _count++;
            }
            else
            {
                var newCapacity = _items.Length == 0 ? 4 : _items.Length * 2;
                var newItems = new T[newCapacity];

                Array.Copy(_items, newItems, _count);

                _items = newItems;
                _items[_count] = item;
                _count++;
            }
        }

        /// <summary>
        /// Удаляет и возвращает элемент с вершины стека.
        /// </summary>
        /// <returns>Удаленный элемент.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public T Pop()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек пуст!");
            }

            _count--;
            T result = _items[_count];
            _items[_count] = default!;

            return result;
        }

        /// <summary>
        /// Возвращает элемент с вершины стека без его удаления.
        /// </summary>
        /// <returns>Элемент с вершины стека.</returns>
        /// <exception cref="InvalidOperationException">Выбрасывается, если стек был пуст.</exception>
        public T Peek()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек пуст!");
            }

            return _items[_count - 1];
        }

        /// <summary>
        /// Проверяет наличие элемента в стеке.
        /// </summary>
        /// <param name="item">Элемент.</param>
        /// <returns>true, если элемент найден и false в противном случае.</returns>
        public bool Contains(T item)
        {
            bool contains = false;
            int i = 0;

            while (i < _count && !contains)
            {
                if (EqualityComparer<T>.Default.Equals(_items[i], item))
                {
                    contains = true;
                }

                i++;
            }

            return contains;
        }

        /// <summary>
        /// Добавляет на вершину стека содержимое коллекции.
        /// </summary>
        /// <param name="collection">Добавляемая коллекция.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void PushRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                Push(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = _count - 1; i >= 0; i--)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
