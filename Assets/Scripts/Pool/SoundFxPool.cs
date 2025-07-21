using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFxPool : PoolMember
{
    [SerializeField] private AudioSource audioSource;
    void OnEnable()
    {
        Invoke(nameof(DespawnEffect), audioSource.clip.length);
        if (SoundManager.Ins.IsMute) return;
        audioSource.Play();
    }

    void DespawnEffect()
    {
        PoolManager.Ins.Despawn(this);
    }
}