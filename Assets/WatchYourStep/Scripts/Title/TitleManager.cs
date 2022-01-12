using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text highSocreTextMesh;
    [SerializeField]
    TMP_Text coinTextMesh;
    [SerializeField]
    TMP_Text staminaTextMesh;
    // Start is called before the first frame update
    void Start()
    {
        highSocreTextMesh.text = $"{PlayerPrefs.GetInt("HighScore", 0)} M";
        coinTextMesh.text = $"{PlayerPrefs.GetInt("CoinNum", 0)} G";
    }

    // Update is called once per frame
    void Update()
    {
        int rs = GameManager.Instance.restSeconds();
        int mm = rs / 60;
        int ss = rs % 60;
        if (GameManager.Instance.IsStaminaMax())
        {
            staminaTextMesh.text = $"S {GameManager.Instance.GetStamina(),2}/{GameManager.Instance.maxStamina,2}";
        }
        else
        {
            staminaTextMesh.text = $"S {GameManager.Instance.GetStamina(),2}/{GameManager.Instance.maxStamina,2}\n [{mm:D2}:{ss:D2}]";
        }
    }

    public void OnClickStartButton()
    {
        if (GameManager.Instance.UseStamina(1))
        {
            SceneManager.LoadScene("Run");
        }
    }

    public void OnClickSutaminaButton(int useAmount)
    {
        if (GameManager.Instance.UseStamina(useAmount))
        {
            Debug.Log($"スタミナ消費{useAmount}");
        }
    }
}
