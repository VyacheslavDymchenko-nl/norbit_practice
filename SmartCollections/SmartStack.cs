namespace SmartCollections.SmartStack
{
    public class SmartStack<T>
    {
        private T[] _items;
        private int _size;

        /// <summary>
        /// Количество элементов в стеке.
        /// </summary>
        public int Size => _size;

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
        /// <param name="length">Начальная емкость стека.</param>
        public SmartStack(int length)
        {
            _items = new T[length];
        }

        /// <summary>
        /// Создает стек, содержащий элементы указанной коллекции.
        /// </summary>
        /// <param name="collection">Коллекция, элементы которой будут помещены в стек.</param>
        public SmartStack(ICollection<T> collection)
        {
            _size = collection.Count;
            _items = new T[_size];

            int i = 0;
            foreach (var item in collection)
            {
                _items[i] = item;
                i++;
            }
        }

        /// <summary>
        /// Добавляет элемент на вершину стека. 
        /// </summary>
        /// <param name="item">Элемент.</param>
        public void Push(T item)
        {
            if (_size < _items.Length)
            {
                _items[_size] = item;
                _size++;
            }
            else
            {
                T[] newItems = new T[_items.Length * 2];
                Array.Copy(_items, newItems, _size);

                _items = newItems;
                _items[_size] = item;
                _size++;
            }
        }

        /// <summary>
        /// Удаляет и возвращает элемент с вершины стека.
        /// </summary>
        /// <returns>Удаленный элемент.</returns>
        /// <exception cref="InvalidOperationException">Выбрасывается, если стек был пуст.</exception>
        public T Pop()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("Стек пуст!");
            }

            _size--;
            T result = _items[_size];
            _items[_size] = default!;

            return result;
        }

        /// <summary>
        /// Возвращает элемент с вершины стека без его удаления.
        /// </summary>
        /// <returns>Элемент с вершины стека.</returns>
        /// <exception cref="InvalidOperationException">Выбрасывается, если стек был пуст.</exception>
        public T Peek()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException("Стек пуст!");
            }

            return _items[_size - 1];
        }
    }
}
