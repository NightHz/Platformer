using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Better2DGame;

namespace Better2DGame
{
    public class Jump : MonoBehaviour
    {
        bool jumpStart = false;
        bool jumpKeep = false;
        bool standInFloor = false;
        bool secondJump = true;
        void UpdateInput()
        {
            // jumpStart = GameInput.Jump;
            jumpStart = Input.GetKeyDown(KeyCode.X);
            jumpKeep = Input.GetKey(KeyCode.X);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger)
            {
                standInFloor = true;
                secondJump = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.isTrigger)
                standInFloor = false;
        }

        [Range(0, 8)] public float jumpBasicHeight = 3.4f;
        [Range(0, 3)] public float jumpBasicTime = 1f;
        float jumpVelocity = 0;
        float gravity = 0;
        public bool doubleJump = true;
        [Range(0, 10)] public float gravityScaleLowJump = 3f;
        [Range(0, 10)] public float gravityScaleDoubleJump = 1.4f;
        [Range(0, 10)] public float gravityScaleFastFalling = 1.3f;

        Rigidbody2D rb;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
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
#if UNITY_EDITOR
            // update jump velocity and gravity
            jumpVelocity = 4 * jumpBasicHeight / jumpBasicTime;
            gravity = 8 * jumpBasicHeight / (jumpBasicTime * jumpBasicTime);
#endif

            UpdateInput();

            // jump
            if (jumpStart)
            {
                // first jump
                if (standInFloor)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
                    standInFloor = false;
                }
                // second jump
                else if(doubleJump && secondJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
                    secondJump = false;
                }
            }
            // adjust gravity
            if (rb.velocity.y > 0)
            {
                // 上升中
                if (jumpKeep)
                {
                    // 保持摁下跳跃
                    if (!doubleJump || secondJump)
                        // 第一次跳跃
                        rb.gravityScale = gravity;
                    else
                        // 第二次跳跃
                        rb.gravityScale = gravity * gravityScaleDoubleJump;
                }
                else
                    // 停止摁下条约——低高度跳跃
                    rb.gravityScale = gravity * gravityScaleLowJump;
            }
            else
            {
                // 下降中
                rb.gravityScale = gravity * gravityScaleFastFalling;
            }
        }
    }
}