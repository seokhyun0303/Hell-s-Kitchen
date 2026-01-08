using UnityEngine;

public class SparkleController : MonoBehaviour
{
    public GameObject sparklePrefab; // Reference to the sparkle particle system prefab
    public Transform plate; // Reference to the plate where the dish will be placed
    public float effectDuration = 1f; // Duration of the sparkle effect

    private void OnDishPlaced()
    {
        // Instantiate the sparkle effect at the plate position
        GameObject sparkle = Instantiate(sparklePrefab, plate.position, Quaternion.identity);

        // Optionally destroy the effect after a duration
        Destroy(sparkle, effectDuration);
    }
}
