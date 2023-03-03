using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_SoundManager : MonoBehaviour
{
    private static SP_SoundManager _instance;
    public static SP_SoundManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    AudioSource[] aSources;
    public Dictionary<string, AudioSource> Sounds = new Dictionary<string, AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
        aSources = GetComponentsInChildren<AudioSource>();
        for(int i=0; i< aSources.Length; i++)
        {
            Sounds.Add(aSources[i].clip.name, aSources[i]);
        }
    }

    public void PlaySound(string name)
    {
        if(Sounds.ContainsKey(name))
            Sounds[name].Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
