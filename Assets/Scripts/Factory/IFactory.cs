namespace Survivor.Factory
{
    /// <summary>
    /// 抽象工厂模式的接口
    /// </summary>
    /// <typeparam name="T">工厂类型</typeparam>
    public interface IFactory<T>
    {
        T Create();
    }
}