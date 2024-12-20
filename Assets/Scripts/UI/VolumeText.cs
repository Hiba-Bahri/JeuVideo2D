using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    private Text txt;
    [SerializeField] private String volumeName;
    [SerializeField] private String textIntro;
    private void Awake()
    {
        txt = GetComponent<Text>();
    }
    private void Update()
    {
        UpdateVolume();
    }
    private void UpdateVolume()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumeName) * 100;
        txt.text = textIntro + volumeValue.ToString();
    }
}
