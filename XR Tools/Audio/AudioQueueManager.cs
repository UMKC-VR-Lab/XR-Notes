using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class AudioQueueManager : MonoBehaviour
{
    public bool debug = true;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    private Queue<AudioClip> audioQueue = new();
    public List<AudioClip> startClips = new();
    private AudioSource currentSource;
    private AudioSource nextSource;
    public bool playStartClipsOnStart = true;
    public bool playStartClipsOnEnable = false;
    private bool isPlaying = false;

    public UnityEvent onClipFinished, onLastClipFinished;

    void Start()
    {
        if (audioSource1 == null || audioSource2 == null)
        {
            Debug.LogError("Audio sources are not assigned!");
            return;
        }

        // Initialize current and next audio sources
        currentSource = audioSource1;
        nextSource = audioSource2;

        if (playStartClipsOnStart)
        {
            PlayStartClips();
        }
    }

    void OnEnable()
    {
        if (audioSource1 == null || audioSource2 == null)
        {
            Debug.LogError("Audio sources are not assigned!");
            return;
        }

        if(audioSource1.isPlaying)
            audioSource1.Stop();
        if(audioSource2.isPlaying)
            audioSource2.Stop();

        // Initialize current and next audio sources
        currentSource = audioSource1;
        nextSource = audioSource2;

        if (playStartClipsOnEnable)
        {
            PlayStartClips();
        }
    }

    private void OnDisable()
    {
        if(audioSource1.isPlaying)
            audioSource1.Stop();
        if(audioSource2.isPlaying)
            audioSource2.Stop();
        
        currentSource = audioSource1;
        nextSource = audioSource2;

        audioQueue.Clear();
    }
    public void PlayStartClips()
    {
        if(debug) Debug.Log("Playing start clips");
        if (startClips.Count == 0)
        {
            Debug.LogWarning("Start clips list is empty.");
            return;
        }

        foreach (AudioClip clip in startClips)
        {
            EnqueueClip(clip);
        }
    }

    void Update()
    {
        if (!currentSource.isPlaying)
        {   
            if(audioQueue.Count > 0)
            {
                if(debug) Debug.Log("Play next clip in update");
                PlayNextClip();
                onClipFinished.Invoke();
            }
            else if(audioQueue.Count == 0 && isPlaying)
            {
                onClipFinished.Invoke();
                onLastClipFinished.Invoke();
                isPlaying = false;
            }
        }
    }

    public void EnqueueClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioQueue.Enqueue(clip);
        }
        else
        {
            Debug.LogWarning("Attempted to enqueue a null AudioClip.");
        }
    }

    private void PlayNextClip()
    {
        if (audioQueue.Count > 0)
        {
            AudioClip nextClip = audioQueue.Dequeue();
            
            nextSource.clip = nextClip;
            nextSource.Play();
            isPlaying = true;
            
            // Swap current and next audio source
            AudioSource temp = currentSource;
            currentSource = nextSource;
            nextSource = temp;
        }
        else
        {
            Debug.LogWarning("Audio queue is empty when attempting to play the next clip.");
        }
    }
}
