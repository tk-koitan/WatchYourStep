using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveY(30, 1).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RunManager.Instance.GameOver();
        }
    }
}
