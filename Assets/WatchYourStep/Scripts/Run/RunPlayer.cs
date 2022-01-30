using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class RunPlayer : MonoBehaviour
{
    Rigidbody2D rb;
    GroundTrigger ground;
    AudioSource audioSource;
    [SerializeField]
    float jumpSpeed = 5f;
    [SerializeField]
    float gravity = 5f;
    [SerializeField]
    float adjustTime = 0.5f;
    [SerializeField]
    int airJumpNum;
    [SerializeField]
    float deathZoneY = -100;
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    Transform lightTransform;
    [SerializeField]
    Transform lightOrigin;
    [SerializeField]
    Transform playerImage;
    bool isTouching;
    Animator animator;
    bool isAirJumping;
    float restAdjustTime;
    int restAirJumpNum;
    [SerializeField]
    Light2D light2d;
    [SerializeField]
    float maxLightRadius = 500f;
    private void Awake()
    {
        TryGetComponent(out rb);
        ground = GetComponentInChildren<GroundTrigger>();
        TryGetComponent(out animator);
        TryGetComponent(out audioSource);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //左と右のタップを判別して保存
        Touch rightTouch = new Touch(), leftTouch = new Touch();
        bool isRightTouch = false, isLeftTouch = false;

#if UNITY_EDITOR
        //パソコン用
        isLeftTouch = true;
        leftTouch.position = Input.mousePosition;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rightTouch.phase = TouchPhase.Began;
        }
        else
        {
            rightTouch.phase = TouchPhase.Stationary;
        }
        isRightTouch = Input.GetKey(KeyCode.Space);
#endif

        foreach (Touch t in Input.touches)
        {
            float viewportPosX = t.position.x / Screen.width;
            if (viewportPosX < 0.5f)
            {
                leftTouch = t;
                isLeftTouch = true;
            }
            else
            {
                rightTouch = t;
                isRightTouch = true;
            }
        }

        //ジャンプ
        if (ground.IsGround)
        {
            //すり抜け床用
            if (rb.velocity.y <= 0)
            {
                restAirJumpNum = airJumpNum;
                animator.Play("Run");
                isAirJumping = false;
                playerImage.rotation = Quaternion.identity;
            }
            if (isRightTouch && rightTouch.phase == TouchPhase.Began)
            {
                rb.velocity = new Vector2(0, jumpSpeed);
                isTouching = true;
                restAdjustTime = adjustTime;
                animator.Play("Jump");
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
        else
        {
            if (isTouching && restAdjustTime > 0f && isRightTouch)
            {
                rb.velocity = new Vector2(0, jumpSpeed);
                restAdjustTime -= Time.deltaTime;
            }
            else
            {
                isTouching = false;
            }

            // 空中ジャンプが残っていた場合
            if (restAirJumpNum > 0 && isRightTouch && rightTouch.phase == TouchPhase.Began)
            {
                rb.velocity = new Vector2(0, jumpSpeed);
                isTouching = true;
                restAdjustTime = adjustTime;
                restAirJumpNum--;
                animator.Play("AirJump");
                isAirJumping = true;
                audioSource.PlayOneShot(audioSource.clip);
            }

            if (isAirJumping)
            {
                playerImage.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
            else
            {
                if (rb.velocity.y > 0)
                {
                    animator.Play("Jump");
                }
                else
                {
                    animator.Play("Fall");
                }
            }
        }

        //ライト
        if (isLeftTouch)
        {
            Vector3 leftTouchWorldPos;
            leftTouchWorldPos = leftTouch.position;
            leftTouchWorldPos.z = 10f;
            leftTouchWorldPos = Camera.main.ScreenToWorldPoint(leftTouchWorldPos);
            lightTransform.right = (leftTouchWorldPos - lightOrigin.position).normalized;
            lightOrigin.right = (leftTouchWorldPos - lightOrigin.position).normalized;
        }

        light2d.intensity = 1f * RunManager.LightAmount;
        /*
        // ライトの半径
        light2d.pointLightInnerRadius = maxLightRadius * 0.5f * RunManager.LightAmount;
        light2d.pointLightOuterRadius = maxLightRadius * RunManager.LightAmount;
        */

        //ゲームオーバー判定
        if (transform.position.y < deathZoneY)
        {
            RunManager.Instance.GameOver();
        }

        KoitanDebug.Display($"leftTouch.position = {leftTouch.position}\n");
        KoitanDebug.Display($"rightTouch.position = {rightTouch.position}\n");
        KoitanDebug.Display($"restAirJumpNum = {restAirJumpNum}\n");
    }

    private void FixedUpdate()
    {
        //重力
        rb.velocity += new Vector2(0, -gravity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.tag == "DeadZone")
        {
            RunManager.Instance.GameOver();
        }
        */
    }
}
