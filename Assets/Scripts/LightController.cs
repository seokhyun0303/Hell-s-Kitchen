using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightController : MonoBehaviour
{
    public Color customAmbientColor = Color.black;
    public Cubemap customReflection;
    public Light directionalLight; // 기본 조명 
    public Light spotLight; // 비상등 
    private Light playerSpotLight; // 플레이어 조명
    public float timeRange_1 = 20f;
    public float timeRange_2 = 40f;
    private bool isBlackout = false;

    public AudioClip turnOnSound;
    private AudioSource audioSource;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // 플레이어의 자식 중에서 Spot Light 찾기
            Transform spotLightTransform = player.transform.Find("Spot Light");

            if (spotLightTransform != null)
            {
                playerSpotLight = spotLightTransform.GetComponent<Light>();

                if (playerSpotLight == null)
                {
                    Debug.LogWarning("Player Spot Light를 찾을 수 없음");
                }
            }
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트가 없습니다.");
        }
        GameObject audioObject = GameObject.Find("AudioSourceObject");
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();
        }
        switch(GameManager.instance.currentStage)
        {
            case 0:
                timeRange_1 = 117f;
                timeRange_2 = 117f;
                break;
            case 1:
                timeRange_1 = 90f;
                timeRange_2 = 100f;
                break;
            case 2:
                timeRange_1 = 30f;
                timeRange_2 = 45f;
                break;
            case 3:
                timeRange_1 = 20f;
                timeRange_2 = 30f;
                break;
        }
        
        SetSkyboxLighting();
        StartCoroutine(TriggerBlackout());
    }

    IEnumerator TriggerBlackout()
    {
        Debug.Log($"Stage: {GameManager.instance.currentStage}, LightControl: {timeRange_1} ~ {timeRange_2}");
        while (true)
        {
            float randomTime = Random.Range(timeRange_1, timeRange_2); 

            // 설정된 시간만큼 대기
            yield return new WaitForSeconds(randomTime);

            // 대기 후 실행할 함수 호출
            SetCustomLighting();
        }
    }
    // 정전일 때, Environment Lighting Source를 Color로, Reflection Source를 Custom으로 설정
    void SetCustomLighting()
    {
        directionalLight.enabled = false;
        spotLight.enabled = true;
        playerSpotLight.enabled = true;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = customAmbientColor; // 커스텀 컬러 설정

        // Reflection Source를 Custom으로 변경
        RenderSettings.customReflection = customReflection; // 커스텀 반사 소스 설정
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom; // Custom으로 설정

        isBlackout = true;
    }

    // 정전이 아닐 때, Environment Lighting Source와 Reflection Source를 다시 Skybox로 전환
    public void SetSkyboxLighting()
    {
        audioSource.PlayOneShot(turnOnSound);
        directionalLight.enabled = true;
        spotLight.enabled = false;
        playerSpotLight.enabled = false;

        RenderSettings.ambientMode = AmbientMode.Skybox;

        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;

        isBlackout = false;
    }
}
