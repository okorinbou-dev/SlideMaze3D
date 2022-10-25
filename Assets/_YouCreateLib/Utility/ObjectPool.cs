using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YCLib
{
    namespace Utility
    {
        public class ObjectPool
        {
            //静的なインスタンスとして持つ
            private static ObjectPool _instance;

            private static bool _isInitialized = false;

            private static int _poolNum = 0;

            private static bool[] _isSetPrefab;
            private static List<GameObject> _prefab;

            private static List<GameObject>[] _pool;
            private static int[] _poolObjectNum;

            //コンストラクタはprivate
            private ObjectPool()
            {
            }

            //インスタンス取得のためのstaticメソッド
            public static ObjectPool GetInstance()
            {
                if (_instance == null)
                {
                    //インスタンス生成
                    _instance = new ObjectPool();
                }
                return _instance;
            }

            public static void Initialize(int poolnum)
            {
                if (_isInitialized)
                {
                    Debug.Log("ObjectPool(Initialize) : already initialized");
                    return;
                }

                _isInitialized = true;
                _poolNum = poolnum;
                _pool = new List<GameObject>[_poolNum];
                _poolObjectNum = new int[_poolNum];

                _isSetPrefab = new bool[_poolNum];
                _prefab = new List<GameObject>();

                for (int i=0; i< _poolNum; i++)
                {
                    _isSetPrefab[i] = false;
                    _poolObjectNum[i] = 0;
                    _pool[i] = new List<GameObject>();
                }

            }

            public static void SetPrefab(int poolno, GameObject prefab)
            {
                if (!_isInitialized)
                {
                    Debug.Log("ObjectPool(SetPrefab) : not initialized");
                    return;
                }

                if (poolno >= _poolNum)
                {
                    Debug.Log("ObjectPool(SetPrefab) : SetPrefab overflow");
                    return;
                }

                _prefab.Add( prefab);
                _isSetPrefab[poolno] = true;
            }

            public static GameObject Rent(int poolno, Transform parent = null)
            {
                foreach (GameObject obj in _pool[poolno])
                {
                    if (!obj.activeSelf)
                    {
                        obj.SetActive(true);
                        return obj;
                    }
                }

                _pool[poolno].Add(Object.Instantiate(_prefab[poolno]));

                _pool[poolno][_pool[poolno].Count - 1].transform.SetParent(parent);
                _pool[poolno][_pool[poolno].Count - 1].transform.localPosition = new Vector3(0,0,0);

                _pool[poolno][_pool[poolno].Count - 1].SetActive(true);

                _poolObjectNum[poolno]++;

                return _pool[poolno][_pool[poolno].Count - 1];
            }


            public static void Return(int poolno, GameObject obj)
            {
                obj.SetActive(false);
                obj.transform.SetParent(null);

                _poolObjectNum[poolno]--;
            }
        }
    }
}
