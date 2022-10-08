using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class SoundTrigger : MonoBehaviour
{
    public AudioClip clipOverride;
    public bool allowRepeat = false;

    private ushort repeatCount = 0;
    private Collider trigger;
    private AudioSource audioSource;

    private void Awake()
    {
        trigger = GetComponent<Collider>();
        if (!trigger.isTrigger)
        {
            trigger.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "player")
            return;

        if (!audioSource.isPlaying)
        {
            if (allowRepeat == false && repeatCount < 1)
            {
                audioSource.Play();
                repeatCount++;
            }
            else if (allowRepeat == true)
            {
                audioSource.Play();
                repeatCount++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Do nothing
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
        //    audioSource.clip = clipOverride;
        }

        if (clipOverride != null)
        {
            audioSource.clip = clipOverride;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
