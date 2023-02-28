using UnityEngine;
using Survivor.Pool;
using Survivor.Factory;

namespace Survivor.Audio
{
    [CreateAssetMenu(fileName = "NewSoundEmitterPool", menuName = "Pool/SoundEmitter Pool")]
    public class SoundEmitterPoolSO : ComponentPoolSO<SoundEmitter>
    {
        [SerializeField]
        private SoundEmitterFactorySO factory;

        public override IFactory<SoundEmitter> Factory
        {
            get
            {
                return factory;
            }
            set
            {
                factory = value as SoundEmitterFactorySO;
            }
        }
    }
}