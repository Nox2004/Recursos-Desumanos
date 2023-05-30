using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MusicManager : MonoBehaviour
{
    [Serializable]
    struct RoomMusic
    {
        public string room_name;
        public AudioClip music;
    }

    [SerializeField] private RoomMusic[] musics;
    private AudioSource audio_source;

    private string last_scene = "";

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audio_source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != last_scene)
        {
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        last_scene = SceneManager.GetActiveScene().name;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("AAAAAAAAAAAAAAa");
        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].room_name == scene.name)
            {
                //Doesn't change music if it's already playing
                if (audio_source.clip == musics[i].music) return;

                //Changes music
                audio_source.clip = musics[i].music;
                audio_source.Play();
                return;
            }
        }
    }
}
