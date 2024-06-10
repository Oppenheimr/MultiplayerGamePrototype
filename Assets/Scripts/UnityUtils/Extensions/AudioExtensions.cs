using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class AudioExtensions
    {
        public static void SmoothPlay(this AudioSource source, float volume = 1, float duration = 1)
        {
            source.Play();
            source.volume = 0;
            source.DOFade(volume, duration);
        }
        
        public static TweenerCore<float, float, FloatOptions> SmoothStop(this AudioSource source, float duration = 1) => source.DOFade(0, duration).OnComplete(source.Stop);
    }
}