using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip music;

    private void Start()
    {
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
    }
}
