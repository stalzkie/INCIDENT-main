using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateControl : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isJumping = animator.GetBool(isJumpingHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        bool jumpPressed = Input.GetKeyDown("space");

        if (!isWalking && forwardPressed) {
            animator.SetBool(isWalkingHash, true);
        }

        if (isWalking && !forwardPressed) {
            animator.SetBool(isWalkingHash, false);
        }

        if (!isRunning && (forwardPressed && runPressed)) {
            animator.SetBool(isRunningHash, true);
        }

        if (isRunning && (!forwardPressed || !runPressed)) {
            animator.SetBool(isRunningHash, false);
        }

        if (!isJumping && jumpPressed) {
            animator.SetBool(isJumpingHash, true);
        }

        if (isJumping && !jumpPressed) {
            animator.SetBool(isJumpingHash, false);
        }
        
    }
}
