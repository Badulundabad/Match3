using UnityEngine;

namespace Scripts.Audio
{
    [CreateAssetMenu(fileName = "NewAudioList", menuName = "Audio List", order = 1)]
    public class AudioList : ScriptableObject
    {
        [SerializeField] private AudioClip victory;
        [SerializeField] private AudioClip swap;
        [SerializeField] private AudioClip match;
        [SerializeField] private AudioClip reject;

        public AudioClip Victory { get => victory; }
        public AudioClip Swap { get => swap; }
        public AudioClip Match { get => match; }
        public AudioClip Reject { get => reject; }
    }
}
