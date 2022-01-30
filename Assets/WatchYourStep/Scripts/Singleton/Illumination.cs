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
            Debug.Log("‚·‚Å‚ÉIllumination‚Í‘¶İ‚µ‚Ü‚·");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //‰Šú‰»
        panel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// –¾“]
    /// </summary>
    /// <param name="act">–¾“]’¼Œã‚És‚¢‚½‚¢Action</param>
    /// <param name="time">–¾“]‚É‚©‚©‚éŠÔ</param>
    /// <returns>true : ¬Œ÷, false : ¸”s</returns>
    public static bool Open(Action act, float time = 1f) => Instance._Open(act, time);

    /// <summary>
    /// ˆÃ“]
    /// </summary>
    /// <param name="act">ˆÃ“]’¼Œã‚És‚¢‚½‚¢Action</param>
    /// <param name="time">ˆÃ“]‚É‚©‚©‚éŠÔ</param>
    /// <returns>true : ¬Œ÷, false : ¸”s</returns>
    public static bool Close(Action act, float time = 1f) => Instance._Close(act, time);

    /// <summary>
    /// –¾“]‰Â”\‚©
    /// </summary>
    public static bool CanOpen => Instance.State == IlluminationState.Closed;

    /// <summary>
    /// ˆÃ“]‰Â”\‚©
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
