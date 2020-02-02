using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] shootClips;
    [SerializeField] private AudioClip[] explodeClips;
    [SerializeField] private AudioClip[] attachClips;
    [SerializeField] private AudioClip[] joinClips;
    [SerializeField] private AudioClip[] winClips;
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private AudioClip[] startClips;

    private AudioSource source;
    private static SoundManager instance;

    void Start()
    {
        source = GetComponent<AudioSource>();
        instance = this;
    }

    public static void PlayShootClip()
    {
        PlayRandomClip(instance.shootClips);
    }
    public static void PlayExplodeClip()
    {
        PlayRandomClip(instance.explodeClips);
    }
    public static void PlayAttachClip()
    {
        PlayRandomClip(instance.attachClips);
    }
    public static void PlayJoinClip()
    {
        PlayRandomClip(instance.joinClips);
    }
    public static void PlayWinClip()
    {
        PlayRandomClip(instance.winClips);
    }
    public static void PlayDeathClip()
    {
        PlayRandomClip(instance.deathClips);
    }
    public static void PlayStartClip()
    {
        PlayRandomClip(instance.startClips);
    }

    private static void PlayRandomClip(AudioClip[] clips)
    {
        if (clips.Length > 0)
        {
            int index = Random.Range(0, clips.Length - 1);
            instance.source.PlayOneShot(clips[index], 1f);
        }
        else
        {
            Debug.Log("Clips missing");
        }
    }
}
