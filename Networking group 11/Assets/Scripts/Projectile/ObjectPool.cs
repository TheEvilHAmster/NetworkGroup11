using System;
using System.Collections.Generic;
using UnityEngine;


    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private T prefab;
        [SerializeField] private int warmupObjects = 0;
        public static ObjectPool<T> Instance { get; set; }
        private Queue<T> objects = new();
        
        private void Awake()
        {
            Instance = this;
            AddObjects(warmupObjects);
        }

        public T GetObject()
        {
            if (objects.Count == 0)
            {
                AddObjects(1);
            }
            return objects.Dequeue();
        }
        public void ReturnToPool(T returnedObject) 
        {
            returnedObject.gameObject.SetActive(false);
            objects.Enqueue(returnedObject);
        }
        private void AddObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var newObject = Instantiate(prefab);
                newObject.gameObject.SetActive(false);
                objects.Enqueue(newObject);
            }
            
        }
    }

