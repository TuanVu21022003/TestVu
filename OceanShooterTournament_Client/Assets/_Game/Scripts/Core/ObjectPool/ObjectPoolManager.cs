using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoSingletonDontDestroyOnLoad<ObjectPoolManager>
{
    [SerializeField] private Transform _objectPoolParent;

    private readonly Dictionary<string, List<GameObject>> _objectPool = new();

    public void Push<T>(T obj) where T : MonoBehaviour, IGetPoolID
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_objectPoolParent);
        if (_objectPool.TryGetValue(obj.GetPoolID(), out var value))
        {
            value.Add(obj.gameObject);
        }
        else
        {
            _objectPool.Add(obj.GetPoolID(), new List<GameObject> { obj.gameObject });
        }
    }

    public T Get<T>(string objectId, Transform parent, Func<T> CreateNewObject)
            where T : MonoBehaviour, IGetPoolID
    {
        T obj;
        if (!_objectPool.TryGetValue(objectId, out var value))
        {
            _objectPool.Add(objectId, new List<GameObject>());
            value = _objectPool[objectId];
        }

        if (value.Count > 0)
        {
            obj = _objectPool[objectId][0].GetComponent<T>();
            _objectPool[objectId].RemoveAt(0);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            return obj;
        }


        obj = CreateNewObject?.Invoke();
        if (obj == null)
        {
            return null;
        }

        obj.gameObject.SetActive(true);
        obj.transform.SetParent(parent);
        return obj;
    }
}