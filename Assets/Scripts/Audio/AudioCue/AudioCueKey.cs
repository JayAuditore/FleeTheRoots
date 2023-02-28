using System;
using System.Collections.Generic;
using System.Linq;
namespace Survivor.Audio
{
    public struct AudioCueKey
    {
        #region 字段

        public static AudioCueKey Invalid = new AudioCueKey(-1, null);

        internal int Value;
        internal AudioCueSO AudioCue;

        #endregion

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value">顺序</param>
        /// <param name="audioCue">音频的SO容器</param>
        internal AudioCueKey(int value, AudioCueSO audioCue)
        {
            Value = value;
            AudioCue = audioCue;
        }

        public override bool Equals(Object obj)
        {
            return obj is AudioCueKey x && Value == x.Value && AudioCue == x.AudioCue;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ AudioCue.GetHashCode();
        }
        public static bool operator ==(AudioCueKey x, AudioCueKey y)
        {
            return x.Value == y.Value && x.AudioCue == y.AudioCue;
        }
        public static bool operator !=(AudioCueKey x, AudioCueKey y)
        {
            return !(x == y);
        }

        #endregion
    }
}