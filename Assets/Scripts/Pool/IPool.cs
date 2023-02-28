namespace Survivor.Pool
{
    /// <summary>
    /// T类型对象的池
    /// </summary>
    /// <typeparam name="T">对象池元素的类型</typeparam>
    public interface IPool<T>
    {
        void Prewarm(int num);
        T Request();
        void Return(T member);
    }
}