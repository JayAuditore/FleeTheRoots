using Survivor.Factory;
using UnityEngine;

namespace Survivor.Audio
{
    /// <summary>
    /// 声音工厂
    /// </summary>
    [CreateAssetMenu(fileName = "NewSoundEmitterFactory", menuName = "Factory/SoundEmitter Factory")]
    public class SoundEmitterFactorySO : FactorySO<SoundEmitter>
    {
        public SoundEmitter Prefab = default;

        public override SoundEmitter Create()
        {
            return Instantiate(Prefab);
        }
    }
}