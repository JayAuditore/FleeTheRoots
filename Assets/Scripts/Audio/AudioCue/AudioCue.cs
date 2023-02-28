using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Audio
{
    /// <summary>
    /// MonoBehaviour的简单实现，可以请求AudioManager播放声音。
    /// 它在作为频道的AudioCueEventSO上触发事件，AudioManager将接收并播放该事件。
    /// </summary>
    public class AudioCue : MonoBehaviour
    {
        #region 字段

        [Header("Sound definition")]
        [SerializeField]
        private AudioCueSO audioCue = default;
        [SerializeField]
        private bool playOnStart = false;

        [Header("Configuration")]
        [SerializeField]
        private AudioCueEventChannelSO audioCueEventChannel = default;
        [SerializeField]
        private AudioConfigurationSO audioConfiguration = default;

        private AudioCueKey controlKey = AudioCueKey.Invalid;

        #endregion

        #region Unity回调

        private void Start()
        {
            if (playOnStart)
            {
                StartCoroutine(PlayDelayed());
            }
        }

        private void OnDisable()
        {
            playOnStart = false;
        }

        #endregion

        #region 方法

        private IEnumerator PlayDelayed()
        {
            //The wait allows the AudioManager to be ready for play requests
            yield return new WaitForSeconds(.1f);

            //This additional check prevents the AudioCue from playing if the object is disabled or the scene unloaded
            //This prevents playing a looping AudioCue which then would be never stopped
            if (playOnStart)
            {
                PlayAudioCue();
            }
        }

        public void PlayAudioCue()
        {
            controlKey = audioCueEventChannel.RaisePlayEvent(audioCue, audioConfiguration, transform.position);
        }

        public void StopAudioCue()
        {
            if (controlKey != AudioCueKey.Invalid)
            {
                if (!audioCueEventChannel.RaiseStopEvent(controlKey))
                {
                    controlKey = AudioCueKey.Invalid;
                }
            }
        }

        public void FinishAudioCue()
        {
            if (controlKey != AudioCueKey.Invalid)
            {
                if (!audioCueEventChannel.RaiseFinishEvent(controlKey))
                {
                    controlKey = AudioCueKey.Invalid;
                }
            }
        }
            
        #endregion
    }
}