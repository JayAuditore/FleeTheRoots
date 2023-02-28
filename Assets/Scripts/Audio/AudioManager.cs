using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

namespace Survivor.Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region 字段

        [Header("SoundEmitters pool")]
        [SerializeField]
        private SoundEmitterFactorySO factory = default;
        [SerializeField]
        private SoundEmitterPoolSO pool = default;
        [SerializeField]
        [Tooltip("对象池大小")]
        private int initialSize = 10;

        [Header("Listening on channels")]
        [Tooltip("SoundManager监听由任何场景中的物体触发的此事件，以播放音效")]
        [SerializeField]
        private AudioCueEventChannelSO SFXEventChannel = default;
        [Tooltip("SoundManager监听由任何场景中的对象触发的此事件，以播放音乐")]
        [SerializeField]
        private AudioCueEventChannelSO musicEventChannel = default;

        [Header("Audio control")]
        [SerializeField]
        private AudioMixer audioMixer = default;
        [Range(0f, 1f)]
        [SerializeField]
        private float masterVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField]
        private float musicVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField]
        private float sfxVolume = 1f;

        private SoundEmitterVault soundEmitterVault;
        private SoundEmitter musicSoundEmitter;

        #endregion

        #region Unity回调

        private void Awake()
        {
            //TODO: Get the initial volume levels from the settings
            soundEmitterVault = new SoundEmitterVault();

            pool.Prewarm(initialSize);
            pool.SetParent(this.transform);
        }

        private void OnEnable()
        {
            SFXEventChannel.OnAudioCuePlayRequested += PlayAudioCue;
            SFXEventChannel.OnAudioCueStopRequested += StopAudioCue;
            SFXEventChannel.OnAudioCueFinishRequested += FinishAudioCue;

            musicEventChannel.OnAudioCuePlayRequested += PlayMusicTrack;
            musicEventChannel.OnAudioCueStopRequested += StopMusic;
        }

        private void OnDestroy()
        {
            SFXEventChannel.OnAudioCuePlayRequested -= PlayAudioCue;
            SFXEventChannel.OnAudioCueStopRequested -= StopAudioCue;
            SFXEventChannel.OnAudioCueFinishRequested -= FinishAudioCue;

            musicEventChannel.OnAudioCuePlayRequested -= PlayMusicTrack;
        }

        /// <summary>
        /// 这仅在编辑器中用于调试音量
        /// 当任何变量发生变化时都会调用它，它将直接改变AudioMixer上的音量值
        /// </summary>
        void OnValidate()
        {
            if (Application.isPlaying)
            {
                SetGroupVolume("Master", masterVolume);
                SetGroupVolume("Music", musicVolume);
                SetGroupVolume("SFX", sfxVolume);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="parameterName">参数的名字</param>
        /// <param name="normalizedVolume">标准化后的大小</param>
        public void SetGroupVolume(string parameterName, float normalizedVolume)
        {
            bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
            if (!volumeSet)
            {
                Debug.LogError("没找到AudioMixer");
            }
        }

        /// <summary>
        /// 获取音量
        /// </summary>
        /// <param name="parameterName">参数名字</param>
        /// <returns>音量大小</returns>
        public float GetGroupVolume(string parameterName)
        {
            if (audioMixer.GetFloat(parameterName, out float rawVolume))
            {
                return MixerValueToNormalized(rawVolume);
            }
            else
            {
                Debug.LogError("没找到AudioMixer");
                return 0f;
            }
        }

        // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
        /// when using UI sliders normalized format
        private float MixerValueToNormalized(float mixerValue)
        {
            // We're assuming the range [-80dB to 0dB] becomes [0 to 1]
            return 1f + (mixerValue / 80f);
        }
        private float NormalizedToMixerValue(float normalizedValue)
        {
            // We're assuming the range [0 to 1] becomes [-80dB to 0dB]
            // This doesn't allow values over 0dB
            return (normalizedValue - 1f) * 80f;
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="audioCue">音频容器</param>
        /// <param name="audioConfiguration">音频设置</param>
        /// <param name="positionInSpace">音频位置，主要针对3D音频</param>
        /// <returns>音频标签</returns>
        private AudioCueKey PlayMusicTrack(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
        {
            float fadeDuration = 2f;
            float startTime = 0f;

            if (musicSoundEmitter != null && musicSoundEmitter.IsPlaying())
            {
                AudioClip songToPlay = audioCue.GetClips()[0];
                if (musicSoundEmitter.GetClip() == songToPlay)
                {
                    return AudioCueKey.Invalid;
                }

                //音乐已经在播放，需要淡出
                startTime = musicSoundEmitter.FadeMusicOut(fadeDuration);
            }

            musicSoundEmitter = pool.Request();
            musicSoundEmitter.FadeMusicIn(audioCue.GetClips()[0], audioConfiguration, 1f, startTime);
            musicSoundEmitter.OnSoundFinishedPlaying += StopMusicEmitter;

            return AudioCueKey.Invalid; //No need to return a valid key for music
        }

        /// <summary>
        /// 停止播放音乐
        /// </summary>
        /// <param name="key">音乐标签</param>
        /// <returns>是否成功</returns>
        private bool StopMusic(AudioCueKey key)
        {
            if (musicSoundEmitter != null && musicSoundEmitter.IsPlaying())
            {
                musicSoundEmitter.Stop();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 通过从对象池中请求适当数量的SoundEmitter来播放AudioCue
        /// </summary>
        /// <param name="audioCue">音频容器</param>
        /// <param name="settings">音频设置</param>
        /// <param name="position">音频位置，主要针对3D音频</param>
        /// <returns>音乐标签</returns>
        public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
        {
            AudioClip[] clipsToPlay = audioCue.GetClips();
            SoundEmitter[] soundEmitterArray = new SoundEmitter[clipsToPlay.Length];

            int nOfClips = clipsToPlay.Length;
            for (int i = 0; i < nOfClips; i++)
            {
                soundEmitterArray[i] = pool.Request();
                if (soundEmitterArray[i] != null)
                {
                    soundEmitterArray[i].PlayAudioClip(clipsToPlay[i], settings, audioCue.Looping, position);
                    if (!audioCue.Looping)
                    {
                        soundEmitterArray[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                    }
                }
            }

            return soundEmitterVault.Add(audioCue, soundEmitterArray);
        }

        /// <summary>
        /// 结束播放音乐，不保留进度
        /// </summary>
        /// <param name="audioCueKey">音频标签</param>
        /// <returns>是否成功</returns>
        public bool FinishAudioCue(AudioCueKey audioCueKey)
        {
            bool isFound = soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

            if (isFound)
            {
                for (int i = 0; i < soundEmitters.Length; i++)
                {
                    soundEmitters[i].Finish();
                    soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
            }
            else
            {
                Debug.LogWarning("请求结束AudioCue，但未找到AudioCue");
            }

            return isFound;
        }

        /// <summary>
        /// 暂停播放音乐，保留进度
        /// </summary>
        /// <param name="audioCueKey">音频标签</param>
        /// <returns>是否成功</returns>
        public bool StopAudioCue(AudioCueKey audioCueKey)
        {
            bool isFound = soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

            if (isFound)
            {
                for (int i = 0; i < soundEmitters.Length; i++)
                {
                    StopAndCleanEmitter(soundEmitters[i]);
                }

                soundEmitterVault.Remove(audioCueKey);
            }

            return isFound;
        }

        /// <summary>
        /// 结束播放时调用的回调
        /// </summary>
        /// <param name="soundEmitter">声音发射器</param>
        private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
        {
            StopAndCleanEmitter(soundEmitter);
        }

        /// <summary>
        /// 实际回调
        /// </summary>
        /// <param name="soundEmitter">声音发射器</param>
        private void StopAndCleanEmitter(SoundEmitter soundEmitter)
        {
            if (!soundEmitter.IsLooping())
            {
                soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
            }

            soundEmitter.Stop();
            pool.Return(soundEmitter);

            //TODO: is the above enough?
            //_soundEmitterVault.Remove(audioCueKey); is never called if StopAndClean is called after a Finish event
            //How is the key removed from the vault?
        }

        /// <summary>
        /// 音乐播完后放回对象池
        /// </summary>
        /// <param name="soundEmitter">声音发射器</param>
        private void StopMusicEmitter(SoundEmitter soundEmitter)
        {
            soundEmitter.OnSoundFinishedPlaying -= StopMusicEmitter;
            pool.Return(soundEmitter);
        }

        #endregion
    }
}