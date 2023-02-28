using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Survivor.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        #region 字段

        private AudioSource audioSource;

        public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

        #endregion

        #region Unity回调

        private void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 让音频在给定的位置播放，可以选择是否循环
        /// </summary>
        /// <param name="clip">音频</param>
        /// <param name="settings">inspector面板上的参数</param>
        /// <param name="hasToLoop">是否循环播放</param>
        /// <param name="position">生成位置，主要针对3D音频</param>
        public void PlayAudioClip(AudioClip clip, AudioConfigurationSO settings, bool hasToLoop, Vector3 position = default)
        {
            audioSource.clip = clip;
            settings.ApplyTo(audioSource);
            audioSource.transform.position = position;
            audioSource.loop = hasToLoop;
            audioSource.time = 0f; //Reset in case this AudioSource is being reused for a short SFX after being used for a long music track
            audioSource.Play();

            if (!hasToLoop)
            {
                StartCoroutine(FinishedPlaying(clip.length));
            }
        }

        /// <summary>
        /// 将音乐缓慢出现
        /// </summary>
        /// <param name="musicClip">当前播放的音乐</param>
        /// <param name="settings">设置</param>
        /// <param name="duration">出现的时间</param>
        /// <param name="startTime">音乐时间调到<paramref name="startTime"/></param>
        public void FadeMusicIn(AudioClip musicClip, AudioConfigurationSO settings, float duration, float startTime = 0f)
        {
            PlayAudioClip(musicClip, settings, true);
            audioSource.volume = 0f;

            //如果长度允许，则在前一个片段结束的同时开始该片段
            //TODO: find a better way to sync fading songs
            if (startTime <= audioSource.clip.length)
            {
                audioSource.time = startTime;
            }
            audioSource.DOFade(1f, duration);
        }

        /// <summary>
        /// 将音乐淡出
        /// </summary>
        /// <param name="duration">淡出时间</param>
        /// <returns>音乐结束的时间</returns>
        public float FadeMusicOut(float duration)
        {
            audioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;

            return audioSource.time;
        }

        private void OnFadeOutComplete()
        {
            NotifyBeingDone();
        }

        /// <summary>
        /// 获取正在播放的音频
        /// </summary>
        public AudioClip GetClip()
        {
            return audioSource.clip;
        }

        /// <summary>
        /// 从暂停的地方继续播放音频
        /// </summary>
        public void Resume()
        {
            audioSource.Play();
        }

        /// <summary>
        /// 暂停音频
        /// </summary>
        public void Pause()
        {
            audioSource.Pause();
        }

        /// <summary>
        /// 停止音频（不保留播放进度）
        /// </summary>
        public void Stop()
        {
            audioSource.Stop();
        }

        /// <summary>
        /// 播完时检查是否重复播放
        /// </summary>
        public void Finish()
        {
            if (audioSource.loop)
            {
                audioSource.loop = false;
                float timeRemaining = audioSource.clip.length - audioSource.time;
                StartCoroutine(FinishedPlaying(timeRemaining));
            }
        }

        /// <summary>
        /// 检查是否在播放
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        /// <summary>
        /// 检查是否循环
        /// </summary>
        /// <returns></returns>
        public bool IsLooping()
        {
            return audioSource.loop;
        }

        IEnumerator FinishedPlaying(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);

            NotifyBeingDone();
        }

        private void NotifyBeingDone()
        {
            OnSoundFinishedPlaying.Invoke(this); // The AudioManager will pick this up
        }

        #endregion
    }
}