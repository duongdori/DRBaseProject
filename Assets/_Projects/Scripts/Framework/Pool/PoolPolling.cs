using System.Collections.Generic;
using UnityEngine;

public abstract class PoolPolling<T> where T : Component
{
    private readonly T _prefab;
    private readonly Queue<T> _pool = new Queue<T>();
    private readonly LinkedList<T> _inuse = new LinkedList<T>();
    private readonly Queue<LinkedListNode<T>> _nodePool = new Queue<LinkedListNode<T>>();
    private int _lastCheckFrame = -1;

    protected PoolPolling(T prefab)
    {
        _prefab = prefab;
    }

    private void CheckInUse()
    {
        var node = _inuse.First;
        while (node != null)
        {
            var current = node;
            node = node.Next;

            if (!IsActive(current.Value))
            {
                current.Value.gameObject.SetActive(false);
                _pool.Enqueue(current.Value);
                _inuse.Remove(current);
                _nodePool.Enqueue(current);
            }
        }
    }

    protected T Get()
    {
        if (_lastCheckFrame != Time.frameCount)
        {
            _lastCheckFrame = Time.frameCount;
            CheckInUse();
        }

        var item = _pool.Count == 0 ? Object.Instantiate(_prefab) : _pool.Dequeue();

        if (_nodePool.Count == 0)
            _inuse.AddLast(item);
        else
        {
            var node = _nodePool.Dequeue();
            node.Value = item;
            _inuse.AddLast(node);
        }

        item.gameObject.SetActive(true);

        return item;
    }

    protected abstract bool IsActive(T component);
}
