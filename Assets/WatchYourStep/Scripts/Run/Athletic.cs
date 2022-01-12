using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Athletic : MonoBehaviour
{
    public float athleticWidth;
    public float noneRate;
    public float yellowRate;
    public float redRate;
    public float blueRate;
    [SerializeField]
    private float expectedValue;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Coin c in GetComponentsInChildren<Coin>())
        {
            c.SetCoinColor(GetCoinColor());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x + athleticWidth < -300f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(-RunManager.AthleticSpeed * Time.fixedDeltaTime, 0);
    }

    private void OnValidate()
    {
        float sum = noneRate + yellowRate + redRate + blueRate;
        if (sum <= 0) return;
        int coinCnt = GetComponentsInChildren<Coin>().Length;
        expectedValue = coinCnt * (yellowRate / sum + 5 * redRate / sum + 10 * blueRate / sum);
    }

    CoinColor GetCoinColor()
    {
        float sum = noneRate + yellowRate + redRate + blueRate;
        float r = Random.Range(0f, sum);
        if (r <= noneRate)
        {
            return CoinColor.None;
        }
        r -= noneRate;
        if (r <= yellowRate)
        {
            return CoinColor.Yellow;
        }
        r -= yellowRate;
        if (r <= redRate)
        {
            return CoinColor.Red;
        }
        r -= redRate;
        if (r <= blueRate)
        {
            return CoinColor.Blue;
        }
        return CoinColor.None;
    }
}
