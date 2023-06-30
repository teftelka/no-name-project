using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu()]
    public class BackgroundMusicSO : ScriptableObject
    {
        public string songName;
        public AudioClip backgroundSong;
        public float BMP;
        public float startOffset;
        public int bit;
    }
}
