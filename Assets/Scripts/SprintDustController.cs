using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintDustController : MonoBehaviour
{
    private ParticleSystem dustEffect;

    void Start()
    {
        dustEffect = GetComponent<ParticleSystem>();
    }

    public void PlayDustOnce()
    {
        if (dustEffect != null && !dustEffect.isPlaying)
        {
            dustEffect.Play(); // Play the particle effect
        }
    }
    public void StopDust()
    {
        if (dustEffect != null && dustEffect.isPlaying)
        {
            dustEffect.Stop(); // Stop the particle effect
        }
    }
}
