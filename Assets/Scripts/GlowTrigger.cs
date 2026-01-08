using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowTrigger : MonoBehaviour
{
    private InteractableGlow interactableGlow;

    void Start()
    {
        interactableGlow = GetComponent<InteractableGlow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactableGlow != null)
        {
            interactableGlow.StartGlow();
            Debug.Log("GLOW");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactableGlow != null)
        {
            interactableGlow.StopGlow();
        }
    }
}
