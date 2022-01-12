using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public class GroundTrigger : MonoBehaviour
{
    public bool IsGround { get; private set; }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        KoitanDebug.Display($"IsGround = {IsGround}\n");
    }

    private void FixedUpdate()
    {
        IsGround = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGround = true;
        }
    }
}
