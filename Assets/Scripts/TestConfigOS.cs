using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "TestConfigOS",menuName = "GirlBoard/TestConfigOS")]
    public class TestConfigOS : ScriptableObject
    {
        [SerializeField]
        public SerializableDirectory<string, GameObject> boardDic;
    }

    [Serializable]
    public class SerializebleKeyValuePair<T1, T2>
    {
        public T1 key;
        public T2 value;

        public SerializebleKeyValuePair(T1 key, T2 value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public class SerializableDirectory<Tkey, Tvalue>
    {
        [SerializeField]
        private List<SerializebleKeyValuePair<Tkey,Tvalue>> _list;
        public List<SerializebleKeyValuePair<Tkey, Tvalue>> list => _list;
        
        public SerializableDirectory(Tkey key, Tvalue value)
        {
            _list = new List<SerializebleKeyValuePair<Tkey, Tvalue>>();
        }

        public void Add(Tkey key, Tvalue value)
        {
            _list.Add(new SerializebleKeyValuePair<Tkey, Tvalue>(key, value));
        }

        public bool TryGetValue(Tkey key,out Tvalue value)
        {
            SerializebleKeyValuePair<Tkey, Tvalue> item = null;
            foreach (var VARIABLE in _list)
            {
                if (VARIABLE.key.Equals(key))
                {
                    item = VARIABLE;
                }
            }

            if (item!= null)
            {
                value = item.value;
                return true;
            }
            
            value = default;
            return false;
        }
        
        public void Remove(Tkey key)
        {
            SerializebleKeyValuePair<Tkey, Tvalue> item = null;
            foreach (var VARIABLE in _list)
            {
                if (VARIABLE.key.Equals(key))
                {
                    item = VARIABLE;
                }
            }

            if (item!= null)
            {
                _list.Remove(item);
            }
        }
        
        
    }
    
    
}