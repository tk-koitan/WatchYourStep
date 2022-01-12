using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Animator animator;
    Collider2D col;
    AudioSource audioSource;
    SpriteRenderer sr;
    int coinNum = 1;
    string getAnimName;

    //マグネット用
    Vector3 velocity;
    Vector3 position;
    Transform target;
    float period;
    bool isStart = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStart) return;
        var acceleration = Vector3.zero;
        var diff = target.position - position;
        acceleration += (diff - velocity * period) * 2f / (period * period);
        period -= Time.deltaTime;
        if (period < 0f)
        {
            transform.position = target.position;
            isStart = false;
            return;
        }
        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    public void Launch(Transform target, Vector3 velocity, float period)
    {
        if (isStart) return;
        this.target = target;
        this.velocity = velocity;
        this.period = period;
        position = transform.position;
        isStart = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // コイン取得
            animator.Play(getAnimName);
            col.enabled = false;
            audioSource.Play();
            GameManager.CoinNum += coinNum;
        }
    }

    public void SetCoinColor(CoinColor coinColor)
    {
        switch (coinColor)
        {
            case CoinColor.None:
                gameObject.SetActive(false);
                break;
            case CoinColor.Yellow:
                animator.Play("Idle");
                getAnimName = "Get";
                coinNum = 1;
                break;
            case CoinColor.Red:
                animator.Play("IdleRed");
                getAnimName = "GetRed";
                coinNum = 5;
                break;
            case CoinColor.Blue:
                animator.Play("IdleBlue");
                getAnimName = "GetBlue";
                coinNum = 10;
                break;
        }
        if (coinColor == CoinColor.None) return;
        // 自分の薄い絵の複製
        var copy = Instantiate(sr, sr.transform.position, Quaternion.identity);
        copy.transform.SetParent(transform.parent);
        Color c = copy.color;
        c.a = 0.2f;
        copy.color = c;
    }
}

public enum CoinColor
{
    None,
    Yellow,
    Red,
    Blue
}
