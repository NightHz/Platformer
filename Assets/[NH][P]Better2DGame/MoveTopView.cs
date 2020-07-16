using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Better2DGame;

namespace Better2DGame
{
    public class MoveTopView : MonoBehaviour
    {
        public bool xCanMove = true;
        public bool yCanMove = true;
        Vector2 moveInput = Vector2.zero;
        void UpdateInput()
        {
            // moveInput = GameInput.Move;
            if (xCanMove)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                    moveInput.x = 1;
                else if (Input.GetKey(KeyCode.LeftArrow))
                    moveInput.x = -1;
                else
                    moveInput.x = 0;
            }
            if (yCanMove)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    moveInput.y = 1;
                else if (Input.GetKey(KeyCode.DownArrow))
                    moveInput.y = -1;
                else
                    moveInput.y = 0;
            }
            if (moveInput != Vector2.zero)
                moveInput.Normalize();
        }

        Rigidbody2D rb;
        [Range(0, 20)] public float moveVelocity = 8f;
        [Range(0, 1)] public float speedUpTime = 0.15f;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            UpdateInput();

            // move
            Vector2 deltaVel = Vector2.zero;
            if (xCanMove && yCanMove)
            {
                deltaVel = moveVelocity * moveInput - rb.velocity;
            }
            else if (xCanMove)
            {
                deltaVel = new Vector2(moveVelocity * moveInput.x - rb.velocity.x, 0);
            }
            else if (yCanMove)
            {
                deltaVel = new Vector2(0, moveVelocity * moveInput.y - rb.velocity.y);
            }
            if (deltaVel != Vector2.zero)
            {
                float change = moveVelocity / speedUpTime * Time.deltaTime;
                change = Mathf.Min(deltaVel.magnitude, change);
                rb.velocity += change * deltaVel.normalized;
            }
        }
    }
}