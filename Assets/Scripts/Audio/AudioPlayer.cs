using UnityEngine;
using Zenject;

namespace Scripts.Audio
{
    public class AudioPlayer
    {
        [Inject]private AudioSource source;
        [Inject(Id = "victory")] private AudioClip victory;
        [Inject(Id = "reject")] private AudioClip reject;
        [Inject(Id = "swap")] private AudioClip swap;
        [Inject(Id = "match")] private AudioClip match;

        public void PlayVictory()
        {
            source.PlayOneShot(victory);
        }

        public void PlayReject()
        {
            source.PlayOneShot(reject);
        }

        public void PlayCrystalSwap()
        {
            source.PlayOneShot(swap);
        }

        public void PlayCrystalMatch()
        {
            source.PlayOneShot(match);
        }
    }
}
