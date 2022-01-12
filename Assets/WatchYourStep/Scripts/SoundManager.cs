using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeBGMVolume(float value)
    {
        audioMixer.SetFloat("BGM", ConvertVolume2dB(value));
    }

    public void ChangeSEVolume(float value)
    {
        audioMixer.SetFloat("SE", ConvertVolume2dB(value));
    }

    // 0 ~ 1�̒l��dB( �f�V�x�� )�ɕϊ�.
    float ConvertVolume2dB(float volume) => Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);
}
