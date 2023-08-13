using System;

namespace SoundDesigner.Helper;

public class RoundRobinObjectPool<T>
{
    private readonly T?[] _pool;
    private int _nextAvailableIndex;

    public RoundRobinObjectPool(int size, Func<T> objectGenerator)
    {
        _pool = new T?[size];
        _nextAvailableIndex = 0;

        for (var i = 0; i < size; i++)
        {
            _pool[i] = objectGenerator();
        }
    }

    public T? GetNext(Func<T, bool>? filter = null)
    {
        var startIndex = _nextAvailableIndex;

        do
        {
            var item = _pool[_nextAvailableIndex];
            _nextAvailableIndex = (_nextAvailableIndex + 1) % _pool.Length;

            if (item != null && (filter == null || filter(item)))
            {
                return item;
            }
        } while (_nextAvailableIndex != startIndex);

        // All items are filtered out; return default value for the type.
        return default;
    }
}