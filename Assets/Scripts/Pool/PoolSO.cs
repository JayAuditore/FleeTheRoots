using System.Collections.Generic;
using UnityEngine;
using Survivor.Factory;

namespace Survivor.Pool
{
    /// <summary>
    /// 通过工厂按需生成T类型成员的泛型池
    /// </summary>
    /// <typeparam name="T">对象池的类型</typeparam>
    public abstract class PoolSO<T> : ScriptableObject, IPool<T>
    {
        #region 字段

        /// <summary>
        /// 将会用于创建类型为<typeparamref name="T"/>的元素
        /// </summary>
        public abstract IFactory<T> Factory { get; set; }

        protected bool HasBeenPrewarmed { get; set; }
        protected readonly Stack<T> Available = new Stack<T>();

        #endregion

        #region Unity回调

        public virtual void OnDisable()
        {
            Available.Clear();
            HasBeenPrewarmed = false;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 工厂函数
        /// </summary>
        /// <returns>构建的新对象</returns>
        protected virtual T Create()
        {
            return Factory.Create();
        }

        /// <summary>
        /// 预生成一个大小为<paramref name="num"/>、类型为<typeparamref name="T"/>的对象池
        /// </summary>
        /// <param name="num">对象池大小</param>
        /// <remarks>注意：这个方法可以在任何时候调用，但是只能在对象池生命周期内调用一次</remarks>
        public virtual void Prewarm(int num)
        {
            if (HasBeenPrewarmed)
            {
                Debug.LogWarning($"Pool {name} has already been prewarmed.");
                return;
            }
            for (int i = 0; i < num; i++)
            {
                Available.Push(Create());
            }
            HasBeenPrewarmed = true;
        }

        /// <summary>
        /// 从对象池中获取一个类型为<typeparamref name="T"/>的对象
        /// </summary>
        /// <returns>类型为<typeparamref name="T"/>的对象</returns>
        public virtual T Request()
        {
            return Available.Count > 0 ? Available.Pop() : Create();
        }

        /// <summary>
        /// 从对象池中获取所有类型为<typeparamref name="T"/>的对象
        /// </summary>
        /// <returns>一个类型为<typeparamref name="T"/>的对象集合</returns>
        public virtual IEnumerable<T> Request(int num = 1)
        {
            List<T> members = new List<T>(num);
            for (int i = 0; i < num; i++)
            {
                members.Add(Request());
            }
            return members;
        }

        /// <summary>
        /// 把一个类型为<typeparamref name="T"/>的对象放回池子
        /// </summary>
        /// <param name="member">类型为<typeparamref name="T"/>的对象</param>
        public virtual void Return(T member)
        {
            Available.Push(member);
        }

        /// <summary>
        /// 把一个类型为<typeparamref name="T"/>的元素集合放回池子
        /// </summary>
        /// <param name="members">类型为<typeparamref name="T"/>的元素集合</param>
        public virtual void Return(IEnumerable<T> members)
        {
            foreach (T member in members)
            {
                Return(member);
            }
        }

        #endregion
    }
}