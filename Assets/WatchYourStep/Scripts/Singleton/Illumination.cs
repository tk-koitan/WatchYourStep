using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Illumination : MonoBehaviour
{
    public static Illumination Instance { get; private set; }
    public IlluminationState State { get; private set; } = IlluminationState.Closed;
    [SerializeField]
    Image panel;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("すでにIlluminationは存在します");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        panel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 明転
    /// </summary>
    /// <param name="act">明転直後に行いたいAction</param>
    /// <param name="time">明転にかかる時間</param>
    /// <returns>true : 成功, false : 失敗</returns>
    public static bool Open(Action act, float time = 1f) => Instance._Open(act, time);

    /// <summary>
    /// 暗転
    /// </summary>
    /// <param name="act">暗転直後に行いたいAction</param>
    /// <param name="time">暗転にかかる時間</param>
    /// <returns>true : 成功, false : 失敗</returns>
    public static bool Close(Action act, float time = 1f) => Instance._Close(act, time);

    /// <summary>
    /// 明転可能か
    /// </summary>
    public static bool CanOpen => Instance.State == IlluminationState.Closed;

    /// <summary>
    /// 暗転可能か
    /// </summary>
    public static bool CanClose => Instance.State == IlluminationState.Opened;


    bool _Open(Action act, float time = 1f)
    {
        if (State != IlluminationState.Closed)
        {
            return false;
        }
        Sequence seq = DOTween.Sequence()
            .SetUpdate(true)
            .OnStart(() =>
            {
                State = IlluminationState.Opening;
                panel.gameObject.SetActive(true);
                Color c = panel.color;
                c.a = 1;
                panel.color = c;
            })
            .Append(panel.DOFade(0f, time).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                State = IlluminationState.Opened;
                panel.gameObject.SetActive(false);
                act();
            });
        return true;
    }


    bool _Close(Action act, float time = 1f)
    {
        if (State != IlluminationState.Opened)
        {
            return false;
        }
        Sequence seq = DOTween.Sequence()
            .SetUpdate(true)
            .OnStart(() =>
            {
                State = IlluminationState.Closing;
                panel.gameObject.SetActive(true);
                Color c = panel.color;
                c.a = 0;
                panel.color = c;
            })
            .Append(panel.DOFade(1f, time).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                State = IlluminationState.Closed;
                act();
            });
        return true;
    }
}

public enum IlluminationState
{
    Closed,
    Opening,
    Opened,
    Closing
}
