using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicThroughScenes : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
