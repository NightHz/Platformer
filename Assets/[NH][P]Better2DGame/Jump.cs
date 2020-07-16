using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Better2DGame;
using UnityEngine.Events;

namespace Better2DGame
{
    public class Jump : MonoBehaviour
    {
        bool jumpStart = false;
        bool jumpKeep = false;
        bool fastFalling = false;
        void UpdateInput()
        {
            // jumpStart = GameInput.Jump;
            jumpStart = jumpStart || Input.GetKeyDown(KeyCode.X);
            jumpKeep = Input.GetKey(KeyCode.X);
            fastFalling = Input.GetKey(KeyCode.DownArrow);
        }

        [Header("Checker")]
        public Collider2D footTrigger;

        // timer
        float timerLeaveLand = 0;

        // state
        bool isStand = false;
        bool isJump = false;
        bool isForceJump = false;
        bool isSecondJump = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger && collision.gameObject != gameObject)
            {
                if (!isStand && timerLeaveLand > 0.12f)
                    // 着陆
                    OnLand?.Invoke();
                isStand = true;
                isJump = false;
                isForceJump = false;
                isSecondJump = false;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.isTrigger)
                return;

            // 检查是否还有正在接触的碰撞箱
            List<Collider2D> colliders = new List<Collider2D>();
            footTrigger.GetContacts(colliders);
            foreach(Collider2D collider in colliders)
            {
                if (!collider.isTrigger && collider.gameObject != gameObject)
                {
                    // 有
                    return;
                }
            }
            // 没有
            isStand = false;
            timerLeaveLand = 0;
        }

        [Header("Ability")]
        public bool doubleJump = true;

        [Header("Parameter")]
        [Range(0, 8)] public float jumpBasicHeight = 3.4f;
        [Range(0, 3)] public float jumpBasicTime = 1f;
        float jumpVelocity = 0;
        float gravity = 0;
        [Range(0, 10)] public float gravityScaleLowJump = 3f;
        [Range(0, 10)] public float gravityScaleDoubleJump = 1.4f;
        [Range(0, 10)] public float gravityScaleFalling = 1.3f;
        [Range(0, 10)] public float gravityScaleFastFalling = 2f;

        [Header("Event")]
        public UnityEvent OnLand;


        Rigidbody2D rb;
        Animator ani;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
        }

        private void Start()
        {
            // set global gravity
            Physics2D.gravity = new Vector2(0, -1);
            Debug.Log("Jump Script sets 2d gravity to (0,-1)");
            // set jump velocity and gravity
            jumpVelocity = 4 * jumpBasicHeight / jumpBasicTime;
            gravity = 8 * jumpBasicHeight / (jumpBasicTime * jumpBasicTime);
            rb.gravityScale = gravity;
        }
        private void Update()
        {
            UpdateInput();

            // animation
            ani?.SetBool("isJump", isJump);
        }
        private void FixedUpdate()
        {
#if UNITY_EDITOR
            // update jump velocity and gravity
            jumpVelocity = 4 * jumpBasicHeight / jumpBasicTime;
            gravity = 8 * jumpBasicHeight / (jumpBasicTime * jumpBasicTime);
#endif
            // timer
            timerLeaveLand += Time.fixedDeltaTime;

            // jump
            if (jumpStart)
            {
                // first jump
                if (isStand)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
                    isStand = false;
                    isJump = true;
                    isForceJump = false;
                    isSecondJump = false;
                    timerLeaveLand = 0;
                }
                // second jump
                else if (doubleJump && !isSecondJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
                    isJump = true;
                    isForceJump = false;
                    isSecondJump = true;
                }
                jumpStart = false;
            }

            // adjust gravity
            if (rb.velocity.y > 0)
            {
                // 上升中
                if (jumpKeep || isForceJump)
                {
                    // 保持摁下跳跃
                    if (!doubleJump || !isSecondJump)
                        // 第一次跳跃
                        rb.gravityScale = gravity;
                    else
                        // 第二次跳跃
                        rb.gravityScale = gravity * gravityScaleDoubleJump;
                }
                else
                    // 低高度跳跃
                    rb.gravityScale = gravity * gravityScaleLowJump;
            }
            else
            {
                // 下降中
                if (fastFalling)
                    // 加速下降
                    rb.gravityScale = gravity * gravityScaleFastFalling;
                else
                    // 正常下降
                    rb.gravityScale = gravity * gravityScaleFalling;
            }
        }

        private void OnDisable()
        {
            jumpStart = false;
        }

        public void ForceJump(float intensity = 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity * intensity);
            isStand = false;
            isJump = true;
            isForceJump = true;
            isSecondJump = false;
            timerLeaveLand = 0;
        }
    }
}