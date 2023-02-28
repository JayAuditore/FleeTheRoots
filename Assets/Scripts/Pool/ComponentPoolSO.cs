using Survivor.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Pool
{
    public abstract class ComponentPoolSO<T> : PoolSO<T> where T : Component
    {
        #region 字段

        private Transform poolRoot;
        private Transform PoolRoot
        {
            get
            {
                if (poolRoot == null)
                {
                    poolRoot = new GameObject(name).transform;
                    poolRoot.SetParent(parent);
                }
                return poolRoot;
            }
        }
        private Transform parent;

        #endregion

        #region 方法

        /// <summary>
        /// 将对象池的父物体设置为<paramref name="t"/>
        /// </summary>
        /// <param name="t">对象池的父物体</param>
        /// <remarks>注意：将父对象设置为DontDestroyOnLoad将有效地使对象池DontDestoryOnLoad。<br/>
        /// 这只能通过手动销毁对象或其父对象，或者将父对象设置为未标记为DontDestroyOnLoad来避免。</remarks>
        public void SetParent(Transform t)
        {
            parent = t;
            PoolRoot.SetParent(parent);
        }

        public override T Request()
        {
            T member = base.Request();
            member.gameObject.SetActive(true);
            return member;
        }

        public override void Return(T member)
        {
            member.transform.SetParent(PoolRoot.transform);
            member.gameObject.SetActive(false);
            base.Return(member);
        }

        protected override T Create()
        {
            T newMember = base.Create();
            newMember.transform.SetParent(PoolRoot.transform);
            newMember.gameObject.SetActive(false);
            return newMember;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (poolRoot != null)
            {
#if UNITY_EDITOR
                DestroyImmediate(poolRoot.gameObject);
#else
				Destroy(poolRoot.gameObject);
#endif
            }
        }

        #endregion
    }
}