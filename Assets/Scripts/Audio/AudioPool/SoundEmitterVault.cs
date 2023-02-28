using System.Collections.Generic;

namespace Survivor.Audio
{
    public class SoundEmitterVault
    {
        #region 字段

        private int nextUniqueKey = 0;
		private List<AudioCueKey> emittersKey;
		private List<SoundEmitter[]> emittersList;

        #endregion

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        public SoundEmitterVault()
		{
			emittersKey = new List<AudioCueKey>();
			emittersList = new List<SoundEmitter[]>();
		}

		/// <summary>
		/// 根据SO获取音频标签
		/// </summary>
		/// <param name="cue">SO容器</param>
		/// <returns>音频标签</returns>
		public AudioCueKey GetKey(AudioCueSO cue)
		{
			return new AudioCueKey(nextUniqueKey++, cue);
		}

		public void Add(AudioCueKey key, SoundEmitter[] emitter)
		{
			emittersKey.Add(key);
			emittersList.Add(emitter);
		}

		/// <summary>
		/// 添加声音发射器
		/// </summary>
		/// <param name="cue">存放音频的容器</param>
		/// <param name="emitter">发射器</param>
		/// <returns>音频的标签</returns>
		public AudioCueKey Add(AudioCueSO cue, SoundEmitter[] emitter)
		{
			AudioCueKey emitterKey = GetKey(cue);

			emittersKey.Add(emitterKey);
			emittersList.Add(emitter);

			return emitterKey;
		}

		/// <summary>
		/// 获取声音发射器
		/// </summary>
		/// <param name="key">要发射的声音</param>
		/// <param name="emitter">返回的发射器</param>
		/// <returns>是否成功</returns>
		public bool Get(AudioCueKey key, out SoundEmitter[] emitter)
		{
			int index = emittersKey.FindIndex(x => x == key);

			if (index < 0)
			{
				emitter = null;
				return false;
			}

			emitter = emittersList[index];
			return true;
		}

		/// <summary>
		/// 移除
		/// </summary>
		/// <param name="key">音频标签</param>
		/// <returns>是否成功</returns>
		public bool Remove(AudioCueKey key)
		{
			int index = emittersKey.FindIndex(x => x == key);
			return RemoveAt(index);
		}

		/// <summary>
		/// 同时移除标签和发射器
		/// </summary>
		/// <param name="index">所在的下标</param>
		/// <returns>是否成功</returns>
		private bool RemoveAt(int index)
		{
			if (index < 0)
			{
				return false;
			}

			emittersKey.RemoveAt(index);
			emittersList.RemoveAt(index);

			return true;
		}

        #endregion
    }
}