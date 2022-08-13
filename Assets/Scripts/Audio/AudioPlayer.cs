using UnityEngine;
using Zenject;

namespace Scripts.Audio
{
    public class AudioPlayer
    {
        [Inject]private AudioSource source;
        [Inject(Id = "victory")] private AudioClip victory;
        [Inject(Id = "lose")] private AudioClip lose;
        [Inject(Id = "swap")] private AudioClip swap;
        [Inject(Id = "match")] private AudioClip match;

        public void PlayVictory()
        {
            source.PlayOneShot(victory);
        }

        public void PlayLose()
        {
            source.PlayOneShot(lose);
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
