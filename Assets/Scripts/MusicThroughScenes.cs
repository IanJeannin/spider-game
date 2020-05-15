using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicThroughScenes : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] audioSources = GameObject.FindGameObjectsWithTag("Music");
        if(audioSources.Length>1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }
}
