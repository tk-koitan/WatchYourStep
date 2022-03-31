using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWider : MonoBehaviour
{
    Animator animator;
    Collider2D col;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // ÉâÉCÉgÇçLÇ∞ÇÈ
            animator.Play("Get");
            col.enabled = false;
            audioSource.Play();
            RunManager.LightAngle += 20f;
        }
    }
}
