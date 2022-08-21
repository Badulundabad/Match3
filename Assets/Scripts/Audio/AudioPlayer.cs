using UnityEngine;
using Zenject;

namespace Match3.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource source;
        private AudioList audioList;

        [Inject]
        private void Construct(AudioSource source, AudioList audioList)
        {
            this.source = source;
            this.audioList = audioList;
        }

        public void PlayVictory()
        {
            source.PlayOneShot(audioList.Victory);
        }

        public void PlayReject()
        {
            source.PlayOneShot(audioList.Reject);
        }

        public void PlayCrystalSwap()
        {
            source.PlayOneShot(audioList.Swap);
        }

        public void PlayCrystalMatch()
        {
            source.PlayOneShot(audioList.Match);
        }
    }
}
