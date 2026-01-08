//using System.Collections.Generic;
//using UnityEngine;

//public class ConveyorBelt : MonoBehaviour
//{
//    public float speed;
//    public Vector3 direction;
//    public List<GameObject> onBelt;
//    private Renderer rend;

//    // Start is called before the first frame update
//    void Start()
//    {
//        rend = GetComponent<Renderer>();
//        direction = direction.normalized; // Ensure direction is normalized
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // Move objects on the conveyor belt
//        for (int i = 0; i < onBelt.Count; i++)
//        {
//            GameObject item = onBelt[i];

//            // Ensure the object has a Collider and disable the isTrigger property
//            Collider col = item.GetComponent<Collider>();
//            if (col != null && col.isTrigger)
//            {
//                col.isTrigger = false; // Disable trigger mode to allow physics interaction
//            }

//            // Ensure the object has a Rigidbody
//            Rigidbody rb = item.GetComponent<Rigidbody>();
//            if (rb == null)
//            {
//                rb = item.AddComponent<Rigidbody>();
//                rb.useGravity = true; // Enable gravity
//                rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent tipping
//                rb.interpolation = RigidbodyInterpolation.Interpolate;
//                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
//            }


//            // Apply force to move the object along the conveyor
//            //rb.AddForce(direction.normalized * speed, ForceMode.Force);
//            onBelt[i].GetComponent<Rigidbody>().velocity = speed * direction;
//        }
//        //for (int i = 0; i < onBelt.Count; i++)
//        //{

//        //}

//        // Move the texture in the specified direction
//        float offsetX = direction.x * Time.time * speed / 2;
//        float offsetY = direction.z * Time.time * speed / 3.5f;
//        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
//    }

//    // When something collides with the belt
//    private void OnCollisionEnter(Collision collision)
//    {
//        onBelt.Add(collision.gameObject);
//        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
//        if (rb == null)
//        {
//            rb = collision.gameObject.AddComponent<Rigidbody>();
//        }

//        // Freeze rotation on all axes
//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//    }

//    // When something leaves the belt
//    private void OnCollisionExit(Collision collision)
//    {
//        onBelt.Remove(collision.gameObject);
//        Destroy(collision.gameObject);
//    }
//}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ConveyorBelt : MonoBehaviour
//{
//    public float speed = 1f;
//    public Vector3 direction = Vector3.forward;
//    public List<GameObject> onBelt = new List<GameObject>();
//    private Renderer rend;
//    private float beltBoundary = 10f; // Distance threshold for removing objects

//    void Start()
//    {
//        rend = GetComponent<Renderer>();
//        direction = direction.normalized; // Ensure direction is normalized

//    }

//    void Update()
//    {
//        // Move objects on the conveyor belt
//        for (int i = 0; i < onBelt.Count; i++)
//        {
//            GameObject item = onBelt[i];

//            // Check distance from conveyor and remove if too far
//            if (Vector3.Distance(item.transform.position, transform.position) > beltBoundary)
//            {
//                RemoveAndDestroy(item);
//                i--; // Adjust index after removing item from the list
//                continue;
//            }

//            // Ensure the object has a Collider and disable the isTrigger property
//            Collider col = item.GetComponent<Collider>();
//            if (col != null && col.isTrigger)
//            {
//                col.isTrigger = false; // Disable trigger mode to allow physics interaction
//            }

//            // Ensure the object has a Rigidbody
//            Rigidbody rb = item.GetComponent<Rigidbody>();
//            if (rb == null)
//            {
//                rb = item.AddComponent<Rigidbody>();
//                rb.useGravity = true; // Enable gravity
//                rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent tipping
//                rb.interpolation = RigidbodyInterpolation.Interpolate;
//                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
//            }

//            // Apply constant velocity to move the object along the conveyor
//            rb.velocity = speed * direction;
//        }

//        // Move the conveyor texture in the specified direction
//        float offsetX = direction.x * Time.time * speed / 2;
//        float offsetY = direction.z * Time.time * speed / 3.5f;
//        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (!onBelt.Contains(collision.gameObject))
//        {
//            onBelt.Add(collision.gameObject);

//            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
//            if (rb == null)
//            {
//                rb = collision.gameObject.AddComponent<Rigidbody>();
//                rb.useGravity = true; // Ensure gravity is enabled
//            }

//            // Freeze rotation on all axes to stabilize objects on the conveyor

//            rb.constraints = RigidbodyConstraints.FreezeRotation;

//            //GameManager.instance.CompleteOrder();
//            // If the object is a dish, call CompleteOrder()
//            //if (collision.gameObject.CompareTag("Dish"))
//            //{

//            //}

//            // Start coroutine to destroy the last object after 3 seconds
//            StartCoroutine(DestroyLastObjectAfterDelay(3f));
//        }
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        // Remove object on collision exit as a fallback
//        RemoveAndDestroy(collision.gameObject);
//    }

//    private IEnumerator DestroyLastObjectAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);

//        if (onBelt.Count > 0)
//        {
//            // Destroy the last object in the list and remove it from the list
//            GameObject lastObject = onBelt[onBelt.Count - 1];
//            RemoveAndDestroy(lastObject);
//        }
//    }

//    private void RemoveAndDestroy(GameObject item)
//    {
//        if (onBelt.Contains(item))
//        {
//            onBelt.Remove(item);
//            Destroy(item);
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 direction = Vector3.forward;
    public List<GameObject> onBelt = new List<GameObject>();
    private Renderer rend;
    private float beltBoundary = 10f; // Distance threshold for removing objects

    void Start()
    {
        rend = GetComponent<Renderer>();
        direction = direction.normalized; // Ensure direction is normalized
    }

    void FixedUpdate()
    {
        // Move objects on the conveyor belt
        for (int i = 0; i < onBelt.Count; i++)
        {
            GameObject item = onBelt[i];

            // Check distance from conveyor and remove if too far
            if (Vector3.Distance(item.transform.position, transform.position) > beltBoundary)
            {
                RemoveAndDestroy(item);
                i--; // Adjust index after removing item from the list
                continue;
            }

            // Ensure the object has a Collider and disable isTrigger property
            Collider col = item.GetComponent<Collider>();
            if (col != null && col.isTrigger)
            {
                col.isTrigger = false; // Disable trigger mode to allow physics interaction
            }

            // Ensure the object has a Rigidbody
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = item.AddComponent<Rigidbody>();
                rb.useGravity = false; // Disable gravity on the conveyor
                rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent tipping
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            // Move the item manually
            item.transform.position += direction * speed * Time.fixedDeltaTime;
        }

        // Move the conveyor texture in the specified direction
        float offsetX = direction.x * Time.time * speed / 2;
        float offsetY = direction.z * Time.time * speed / 3.5f;
        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!onBelt.Contains(collision.gameObject))
        {
            onBelt.Add(collision.gameObject);

            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = collision.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false; // Disable gravity on the conveyor
            }

            // Freeze rotation on all axes to stabilize objects on the conveyor
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            Debug.Log($"Object {collision.gameObject.name} entered conveyor belt.");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Remove object on collision exit
        RemoveAndDestroy(collision.gameObject);
    }

    private void RemoveAndDestroy(GameObject item)
    {
        if (onBelt.Contains(item))
        {
            onBelt.Remove(item);
            Destroy(item);
        }
    }
}



