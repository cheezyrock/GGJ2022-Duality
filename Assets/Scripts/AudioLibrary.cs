using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioLibrary : MonoBehaviour
{
    public static AudioLibrary Instance { get; private set; }

    public List<Lib> library;

    void Awake ()
    {
        Instance = this;
    }

    [System.Serializable]
    public class Lib
    {
        public Sounds name;
        public AudioClip audioClip;
        public SoundType type;
    }
}

[System.Serializable]
public enum Sounds
{
    Sound1 = 0,

}

[System.Serializable]
public enum SoundType
{
    SoundType1 = 0,

}
