using UnityEngine;
using Zenject;

namespace Scripts.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        private AudioSource source;

        [Inject]
        private void Construct(AudioSource source)
        {
            this.source = source;
        }

        private void Start()
        {
            source.Play();
        }
    }
}