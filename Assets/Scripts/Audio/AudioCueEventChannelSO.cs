using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Audio
{
    /// <summary>
    /// AudioCue组件发送消息以播放音乐的事件。AudioManager会监听这些事件，并实际播放声音。
    /// </summary>
    [CreateAssetMenu(menuName = "Events/AudioCue Event Channel")]
    public class AudioCueEventChannelSO : ScriptableObject
    {
        #region 委托

        public AudioCuePlayAction OnAudioCuePlayRequested;
        public AudioCueStopAction OnAudioCueStopRequested;
        public AudioCueFinishAction OnAudioCueFinishRequested;

        #endregion

        #region 方法

        /// <summary>
        /// 引发播放事件
        /// </summary>
        /// <param name="audioCue">音频</param>
        /// <param name="audioConfiguration">设置</param>
        /// <param name="positionInSpace">位置，针对3D音频</param>
        /// <returns>音频标签</returns>
        public AudioCueKey RaisePlayEvent(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
        {
            AudioCueKey audioCueKey = AudioCueKey.Invalid;

            if (OnAudioCuePlayRequested != null)
            {
                audioCueKey = OnAudioCuePlayRequested.Invoke(audioCue, audioConfiguration, positionInSpace);
            }
            else
            {
                Debug.LogWarning("请求了AudioCue播放活动，但没有人回应。" +
                    "检查为什么尚未加载AudioManager，" +
                    "并确保它正在监听此AudioCue事件频道。");
            }

            return audioCueKey;
        }

        /// <summary>
        /// 引发暂停事件
        /// </summary>
        /// <param name="audioCueKey">音频标签</param>
        /// <returns>是否成功</returns>
        public bool RaiseStopEvent(AudioCueKey audioCueKey)
        {
            bool requestSucceed = false;

            if (OnAudioCueStopRequested != null)
            {
                requestSucceed = OnAudioCueStopRequested.Invoke(audioCueKey);
            }
            else
            {
                Debug.LogWarning("请求了AudioCue播放活动，但没有人回应。" +
                    "检查为什么尚未加载AudioManager，" +
                    "并确保它正在监听此AudioCue事件频道。");
            }

            return requestSucceed;
        }

        /// <summary>
        /// 引发结束事件
        /// </summary>
        /// <param name="audioCueKey">音频标签</param>
        /// <returns>是否成功</returns>
        public bool RaiseFinishEvent(AudioCueKey audioCueKey)
        {
            bool requestSucceed = false;

            if (OnAudioCueStopRequested != null)
            {
                requestSucceed = OnAudioCueFinishRequested.Invoke(audioCueKey);
            }
            else
            {
                Debug.LogWarning("请求了AudioCue播放活动，但没有人回应。" +
                    "检查为什么尚未加载AudioManager，" +
                    "并确保它正在监听此AudioCue事件频道。");
            }

            return requestSucceed;
        }

        #endregion
    }
    public delegate AudioCueKey AudioCuePlayAction(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace);
    public delegate bool AudioCueStopAction(AudioCueKey emitterKey);
    public delegate bool AudioCueFinishAction(AudioCueKey emitterKey);
}