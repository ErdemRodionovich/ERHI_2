using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class ProtectedPlayList
    {
        private List<AudioClip> playList;
        
        public int Count
        {
            get { return playList.Count; }
        }

        public ProtectedPlayList(List<AudioClip> listOfAudio)
        {
            playList = listOfAudio;
        }

        public AudioClip GetAudio(int index)
        {
            return playList[index];
        }

        public string GetNameOfAudio(int index)
        {
            return GetAudio(index).name;
        }

        public AudioClip GetAudioByName(string name)
        {
            foreach(AudioClip clip in playList)
            {
                if (clip.name.Equals(name))
                {
                    return clip;
                }
            }

            Debug.LogError("[ProtectedPlayList] Cannot find audio by name '" + name + "'");

            return null;
        }

    }
}