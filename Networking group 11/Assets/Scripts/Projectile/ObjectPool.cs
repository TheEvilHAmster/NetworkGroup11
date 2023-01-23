using System;
using System.Collections.Generic;
using UnityEngine;


    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private T prefab;
        public static ObjectPool<T> Instance { get; set; }
        private Queue<T> objects = new();

        private void Awake()
        {
            Instance = this;
            AddObjects(150);
        }

        public T GetObject()
        {
            if (objects.Count == 0)
            {
                AddObjects(1);
            }
            return objects.Dequeue();
        }

        private void AddObjects(int count)
        {
            var newObject = GameObject.Instantiate(prefab);
            newObject.gameObject.SetActive(false);
            objects.Enqueue(newObject);
        }
    }

