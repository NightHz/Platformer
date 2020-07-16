using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Better2DGame;

namespace Better2DGame
{
    public class MoveSideView : MonoBehaviour
    {
        float moveInput = 0;
        void UpdateInput()
        {
            // moveInput = GameInput.Move;
            if (Input.GetKey(KeyCode.RightArrow))
                moveInput = 1;
            else if (Input.GetKey(KeyCode.LeftArrow))
                moveInput = -1;
            else
                moveInput = 0;
        }

        Rigidbody2D rb;
        SpriteRenderer sr;
        Animator ani;
        [Range(0, 20)] public float moveVelocity = 8f;
        [Range(0, 1)] public float speedUpTime = 0.35f;
        [Range(0, 1)] public float speedDownTime = 0.1f;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            ani = GetComponent<Animator>();
        }
        private void Update()
        {
            UpdateInput();

            // face
            if (moveInput > 0)
                sr.flipX = false;
            else if (moveInput < 0)
                sr.flipX = true;

            ani.SetBool("isStop", rb.velocity.x == 0);
        }
        private void FixedUpdate()
        {
            // move
            float deltaVel = moveVelocity * moveInput - rb.velocity.x;
            float change = moveVelocity / (deltaVel * rb.velocity.x > 0 ? speedUpTime : speedDownTime) * Time.deltaTime;
            change = Mathf.Min(Mathf.Abs(deltaVel), change);
            rb.velocity += new Vector2(change * Mathf.Sign(deltaVel), 0);

        }
    }
}