using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemdelete : MonoBehaviour
{
    public float lifetime = 5f; // ������Ʈ�� ������������ �ð� (�⺻�� 5��)
    private AudioSource audioSource;
    public AudioClip coinSound;
    public AudioClip itemSound;

    void Start()
    {
        // ������ �ð� �� ������Ʈ�� �ı�
        GameObject audioObject = GameObject.Find("AudioSourceObject");
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();
        }
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("speedup"))
            {
                audioSource.PlayOneShot(itemSound);
                GameManager.instance.ActivateSpeedUp();
            }
            else if (gameObject.CompareTag("cookup"))
            {
                audioSource.PlayOneShot(itemSound);
                GameManager.instance.ActivateCookUp();
            }
            else if (gameObject.CompareTag("addmoney"))
            {
                audioSource.PlayOneShot(coinSound);
                GameManager.instance.ActivateAddMoney();
            }
            Destroy(gameObject);
        }
    }
}
