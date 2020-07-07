using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Better2DGame;

namespace Better2DGame
{
    public class MoveX : MonoBehaviour
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
        [Range(0, 20)] public float moveVelocity = 8f;
        [Range(0, 1)] public float speedUpTime = 0.35f;
        [Range(0, 1)] public float speedDownTime = 0.1f;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            UpdateInput();

            // move
            float deltaVel = moveVelocity * moveInput - rb.velocity.x;
            float change = moveVelocity / (deltaVel * rb.velocity.x > 0 ? speedUpTime : speedDownTime) * Time.deltaTime;
            change = Mathf.Min(Mathf.Abs(deltaVel), change);
            rb.velocity += new Vector2(change * Mathf.Sign(deltaVel), 0);
        }
    }
}