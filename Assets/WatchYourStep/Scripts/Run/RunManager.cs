using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;
using TMPro;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }
    [SerializeField]
    Athletic[] athleticPrefabs;
    [SerializeField]
    Athletic firstAthletic;
    float restAthleticWidth;
    Athletic currentAthletic;
    [SerializeField]
    float standardSpeed = 5f;
    [SerializeField]
    float timeScaleDecreaseSpeed = 0.1f;
    float liveTime = 0;
    [SerializeField]
    TMP_Text scoreTextMesh;
    [SerializeField]
    TMP_Text coinTextMesh;
    float distance = 0;
    float athleticSpeed;
    [SerializeField]
    float lightAmountDecreaseSpeed = 0.05f;
    [SerializeField]
    float lightMinAmount = 0.25f;
    bool isTimeStop = false;
    public static float AthleticSpeed
    {
        get => Instance.athleticSpeed;
        set => Instance.athleticSpeed = value;
    }
    float lightAmount = 0;
    public static float LightAmount
    {
        get => Instance.lightAmount;
        set => Instance.lightAmount = Mathf.Clamp(value, Instance.lightMinAmount, 1);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            isTimeStop = true;
        }
        else
        {
            Debug.LogWarning("インスタンスが存在します");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    void Start()
    {
        currentAthletic = Instantiate(firstAthletic);
        currentAthletic.transform.position = new Vector3(-currentAthletic.athleticWidth / 2, 0);
        restAthleticWidth = currentAthletic.athleticWidth / 2;
        lightAmount = 1f;
        Illumination.Open(() => isTimeStop = false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeStop)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f + timeScaleDecreaseSpeed * liveTime;
        }
        liveTime += Time.deltaTime;
        LightAmount -= lightAmountDecreaseSpeed * Time.deltaTime;
#if UNITY_EDITOR
        // デバッグ用
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Time.timeScale = 10f;
        }
#endif
        AthleticSpeed = standardSpeed;
        restAthleticWidth -= AthleticSpeed * Time.deltaTime;
        if (restAthleticWidth < 300f)
        {
            Athletic nextAthletic = Instantiate(athleticPrefabs[Random.Range(0, athleticPrefabs.Length)]);
            nextAthletic.transform.position = new Vector3(currentAthletic.transform.position.x + currentAthletic.athleticWidth, 0);
            restAthleticWidth += nextAthletic.athleticWidth;
            currentAthletic = nextAthletic;
        }
        distance += standardSpeed * Time.deltaTime / 10;
        scoreTextMesh.text = $"{(int)distance} M";
        coinTextMesh.text = $"{GameManager.CoinNum} G";
        KoitanDebug.Display($"Time.timeScale = {Time.timeScale}\n");
    }

    public void GameOver()
    {
        //ハイスコアのときのみ更新
        if (distance > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", (int)distance);
        }
        PlayerPrefs.SetInt("CoinNum", GameManager.CoinNum);
        SceneManager.LoadScene("Title");
    }
}
