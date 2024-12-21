using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorKey : MonoBehaviour
{
    public static bool gotKey = false;
    [SerializeField] private AudioClip keySound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(keySound);
            gotKey = true;
            Destroy(gameObject);
        }
    }
}
