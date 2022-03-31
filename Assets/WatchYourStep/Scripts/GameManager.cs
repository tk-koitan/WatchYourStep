using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static int CoinNum
    {
        get => Instance.coinNum;
        set => Instance.coinNum = value;
    }
    private int coinNum;
    private DateTime lastDateTime;
    public int maxStamina;
    public int secondsPerStamina;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //������
            coinNum = PlayerPrefs.GetInt("CoinNum", 0);
            lastDateTime = LoadLastDateTime();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //KoitanDebug.Display($"Coin = {coinNum}\n");
        KoitanDebug.Display($"DateTime.Now = {DateTime.Now}\n");
        KoitanDebug.Display($"lastDateTime = {lastDateTime}\n");
        /*
        KoitanDebug.Display($"Stamina = {GetStamina()}\n");
        int rs = restSeconds();
        int mm = rs / 60;
        int ss = rs % 60;
        KoitanDebug.Display($"�X�^�~�i�񕜂̂��� = {mm:D2}:{ss:D2}\n");
        */
    }

    private void OnApplicationPause(bool pauseStatus)
    {

        //�ꎞ��~
        if (pauseStatus)
        {
            PlayerPrefs.SetInt("CoinNum", coinNum);
            SaveLastDateTime();
        }
        //�ĊJ��
        else
        {

        }

    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("CoinNum", coinNum);
        SaveLastDateTime();
    }

    public int GetStamina()
    {
        return Mathf.Min((int)(DateTime.Now - lastDateTime).TotalSeconds / secondsPerStamina, maxStamina);
    }

    public bool IsStaminaMax()
    {
        return GetStamina() == maxStamina;
    }

    /// <summary>
    /// �X�^�~�i�������
    /// </summary>
    /// <returns>
    /// true : ����
    /// false : ����Ȃ�
    /// </returns>
    public bool UseStamina(int requestStamina)
    {
        int tmpStamina = GetStamina();
        tmpStamina -= requestStamina;
        if (tmpStamina >= 0)
        {
            if (IsStaminaMax())
            {
                lastDateTime = DateTime.Now.AddSeconds((-maxStamina + requestStamina) * secondsPerStamina);
            }
            else
            {
                lastDateTime = lastDateTime.AddSeconds(requestStamina * secondsPerStamina);
            }
            return true;
        }
        return false;
    }

    public int RestSeconds()
    {
        if (IsStaminaMax())
        {
            return 0;
        }
        else
        {
            return secondsPerStamina - (int)(DateTime.Now - lastDateTime).TotalSeconds % secondsPerStamina;
        }
    }

    void SaveLastDateTime()
    {
        PlayerPrefs.SetString("LastDateTime", lastDateTime.ToBinary().ToString());
    }

    DateTime LoadLastDateTime()
    {
        string binaryDateTime = PlayerPrefs.GetString("LastDateTime", "");
        if (binaryDateTime == "")
        {
            return DateTime.Now.AddSeconds(-maxStamina * secondsPerStamina);
        }
        else
        {
            return DateTime.FromBinary(Convert.ToInt64(binaryDateTime));
        }

    }
}
