using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public LightController lightController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider other)
    {
        // 플레이어가 트리거 범위 안에 있는지 확인
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Space))
            lightController.SetSkyboxLighting();
            Debug.Log("Player is in the interaction range.");
        }
    }
}
