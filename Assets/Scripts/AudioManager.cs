using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Spawner;
using UnityEngine.SceneManagement;
using UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public SoundVariables[] sounds;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        EnemySpawner.AllWavesCleared += StopBackgroundMusic;
        PauseMenuController.ReturnToMM += StopBackgroundMusic;
    }

    private void OnDestroy()
    {
        EnemySpawner.AllWavesCleared -= StopBackgroundMusic;
        PauseMenuController.ReturnToMM -= StopBackgroundMusic;
    }

    private void StopBackgroundMusic()
    {
        Stop("Background");
    }
    public void Play(string name)
    {
        SoundVariables s = Array.Find(sounds, SoundVariables => SoundVariables.name == name);
        GameObject tempObject = new GameObject("TempSourceAudio");
        tempObject.AddComponent<AudioSource>();
        AudioSource source = tempObject.GetComponent<AudioSource>();
        s.source = source;
        source.volume = s.volume;
        source.pitch = s.pitch;
        source.loop = s.loop;
        source.playOnAwake = s.playOnAwake;
        source.clip = s.clips[UnityEngine.Random.Range(0,s.clips.Length)];
        if (s == null)
        {
            Debug.Log(name + " Not Found");
            return;
        }
        source.Play();
        if(s.loop) { return; }
        Destroy(tempObject, source.clip.length);
    }


    public void Stop(string name)
    {
        SoundVariables s = Array.Find(sounds, SoundVariables => SoundVariables.name == name);
        if (s == null)
        {
            Debug.Log(name + " Not Found");
            return;
        }
        //Stops music from playing
        s.source.Stop();
    }
}

    [Serializable]
    public class SoundVariables
    {
        //audio settings that can be changed in the array
        public string name;

        public AudioClip[] clips;

        [Range(0f, 1f)]
        public float volume;

        [Range(0.1f, 3f)]
        public float pitch;

        public bool loop;

        public bool playOnAwake;

        [HideInInspector]
        public AudioSource source;


    }


