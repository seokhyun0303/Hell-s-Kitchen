using UnityEngine;

public class InteractableGlow : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material[] objectMaterials; // Array to store all materials
    private Color[] originalEmissionColors; // Array to store original emission colors
    public Color glowColor = Color.yellow; // The color of the glow effect
    public float glowIntensity = 2f;      // Intensity multiplier for the glow effect
    private bool isGlowing = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            // Get all materials applied to the object
            objectMaterials = objectRenderer.materials;

            // Store original emission colors for all materials
            originalEmissionColors = new Color[objectMaterials.Length];
            for (int i = 0; i < objectMaterials.Length; i++)
            {
                if (objectMaterials[i].HasProperty("_EmissionColor"))
                {
                    originalEmissionColors[i] = objectMaterials[i].GetColor("_EmissionColor");
                }
                else
                {
                    originalEmissionColors[i] = Color.black; // Default to black if no emission
                }
            }
        }
    }

    public void StartGlow()
    {
        if (!isGlowing && objectMaterials != null)
        {
            isGlowing = true;

            // Apply glow to all materials
            foreach (Material material in objectMaterials)
            {
                if (material.HasProperty("_EmissionColor"))
                {
                    material.EnableKeyword("_EMISSION"); // Enable emission
                    material.SetColor("_EmissionColor", glowColor * glowIntensity);
                }
            }
        }
    }

    public void StopGlow()
    {
        if (isGlowing && objectMaterials != null)
        {
            isGlowing = false;

            // Restore original emission colors to all materials
            for (int i = 0; i < objectMaterials.Length; i++)
            {
                if (objectMaterials[i].HasProperty("_EmissionColor"))
                {
                    objectMaterials[i].SetColor("_EmissionColor", originalEmissionColors[i]);
                    objectMaterials[i].DisableKeyword("_EMISSION"); // Disable emission
                }
            }
        }
    }
}
