using System;
using UnityEngine;

namespace Survivor.Audio
{
    public enum SequenceMode
    {
        Random,
        RandomNoImmediateRepeat,
        Sequential,
    }

    [Serializable]
    public class AudioClipsGroup
    {
        #region 字段

        public SequenceMode Mode = SequenceMode.RandomNoImmediateRepeat;
        public AudioClip[] AudioClips;

        private int nextClipToPlay = -1;
        private int lastClipPlayed = -1;

        #endregion

        #region 方法

        /// <summary>
        /// 选择队列里的下一段音乐，有序或随机
        /// </summary>
        /// <returns>音乐的引用</returns>
        public AudioClip GetNextClip()
        {
            if (AudioClips.Length == 1)
            {
                return AudioClips[0];
            }
            if (nextClipToPlay == -1)
            {
                nextClipToPlay = (Mode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, AudioClips.Length);
            }
            else
            {
                switch (Mode)
                {
                    case SequenceMode.Random:
                        {
                            nextClipToPlay = UnityEngine.Random.Range(0, AudioClips.Length);
                            break;
                        }

                    case SequenceMode.RandomNoImmediateRepeat:
                        {
                            while (nextClipToPlay == lastClipPlayed)
                            {
                                nextClipToPlay = UnityEngine.Random.Range(0, AudioClips.Length);
                            }
                            break;
                        }

                    case SequenceMode.Sequential:
                        {
                            nextClipToPlay = (int)Mathf.Repeat(++nextClipToPlay, AudioClips.Length);
                            break;
                        }
                }
            }
            lastClipPlayed = nextClipToPlay;
            return AudioClips[nextClipToPlay];
        }

        #endregion
    }

    [CreateAssetMenu(fileName = "newAudioCue", menuName = "Audio/Audio Cue")]
    public class AudioCueSO : ScriptableObject
    {
        public bool Looping = false;
        [SerializeField]
        private AudioClipsGroup[] audioClipGroups = default;

        /// <summary>
        /// 获取播放列表
        /// </summary>
        /// <returns>播放列表</returns>
        public AudioClip[] GetClips()
        {
            int numberOfClips = audioClipGroups.Length;
            AudioClip[] resultingClips = new AudioClip[numberOfClips];

            for (int i = 0; i < numberOfClips; i++)
            {
                resultingClips[i] = audioClipGroups[i].GetNextClip();
            }

            return resultingClips;
        }
    }
}