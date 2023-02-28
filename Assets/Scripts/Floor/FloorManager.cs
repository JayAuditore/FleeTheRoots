using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Roots.Utility;

namespace Roots.Floor
{
    public enum FloorType
    {
        NORMAL,
        THORN,
        MAGIC
    }

    public class FloorManager : BaseSingletonWithMono<FloorManager>
    {
        #region 字段

        private int[] bound;
        private float timer;

        public Dictionary<FloorType, List<GameObject>> FloorPool;

        #endregion

        #region Unity回调

        private void Awake()
        {
            FloorPool = new Dictionary<FloorType, List<GameObject>>();
            FloorPool.Add(FloorType.NORMAL, new List<GameObject>());
            FloorPool.Add(FloorType.THORN, new List<GameObject>());
            FloorPool.Add(FloorType.MAGIC, new List<GameObject>());
            bound = new int[3] { -10, 3, -6 };
        }

        private void Start()
        {
            GetFloorFromPool();
            GetFloorFromPool();
            GetFloorFromPool();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > 1.2f)
            {
                GetFloorFromPool();
            }
        }

        #endregion

        #region 方法

        private FloorType RandomType()
        {
            int x = Random.Range(0, 3);
            switch (x)
            {
                case 0:
                    {
                        return FloorType.NORMAL;
                    }
                case 1:
                    {
                        return FloorType.MAGIC;
                    }
                case 2:
                    {
                        return FloorType.THORN;
                    }
                default:
                    break;
            }
            return FloorType.NORMAL;
        }

        /// <summary>
        /// 从对象池获取地板
        /// </summary>
        /// <param name="type">地板类型</param>
        /// <returns>对象</returns>
        public GameObject GetFloorFromPool()
        {
            if (FloorPool == null)
            {
                FloorPool = new Dictionary<FloorType, List<GameObject>>();
                FloorPool.Add(FloorType.NORMAL, new List<GameObject>());
                FloorPool.Add(FloorType.THORN, new List<GameObject>());
                FloorPool.Add(FloorType.MAGIC, new List<GameObject>());
            }
            FloorType type = RandomType();

            FloorPool.TryGetValue(type, out List<GameObject> list);

            if (list.Count == 0)
            {
                var floor = Resources.Load<GameObject>(GetPrefabPath(type));
                var obj = RandomInstantiate(floor);
                FloorPool[type].Add(obj);
                timer = 0f;
                return obj;
            }
            else
            {
                var obj = RandomInstantiate(list[0]);
                list.RemoveAt(0);
                timer = 0f;
                return obj;
            }
        }

        /// <summary>
        /// 把地板放回对象池
        /// </summary>
        /// <param name="floor">对象</param>
        public void ReturnFloor(GameObject floor, FloorType type)
        {
            floor.SetActive(false);
            FloorPool[type].Add(floor);
        }

        public string GetPrefabPath(FloorType type)
        {
            switch (type)
            {
                case FloorType.NORMAL:
                    {
                        return "NormalFloor";
                    }
                case FloorType.THORN:
                    {
                        return "ThornFloor";
                    }
                case FloorType.MAGIC:
                    {
                        return "MagicFloor";
                    }
                default:
                    break;
            }
            return "\0";
        }

        public GameObject RandomInstantiate(GameObject target)
        {
            int randomX = Random.Range(-10, 3);
            return GameObject.Instantiate(target, new Vector3(randomX, -6, 0), target.transform.rotation);
        }

        #endregion
    }
}