using UnityEngine;

namespace Survivor.Factory
{
    /// <summary>
    /// 抽象工厂模式的抽象工厂
    /// </summary>
    /// <typeparam name="T">工厂类型</typeparam>
    public abstract class FactorySO<T> : ScriptableObject, IFactory<T>
    {
        public abstract T Create();
    }
}