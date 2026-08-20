using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    //Variables for animator functionality 
    public Animator animator;
    int horizontal;
    int vertical;
    public bool dialogueLocked = false;

    //Calling variables on wake for setup
    private void Awake()
    {
        animator = GetComponent<Animator>();

        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    //Play specific animation to play
    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    //Play animation for character movements
    public void PlayAnimation(string animationName)
    {
        // 0 = Base Layer
        // 1 = Override Layer
        animator.CrossFade(animationName, 0.2f, 1);
    }

    public void UpdateAnimatorValues(float horizontalMovement, float veritcalMovement, bool isSprinting)
    {
        //Check for dialogue
        if (dialogueLocked)
        {
            ForceIdle();
            return;
        }

        //Animation snapping
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f) 
        {
            snappedHorizontal = 0.5f;
        }
        else if(horizontalMovement > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            snappedHorizontal = -1;
        }
        else 
        { 
            snappedHorizontal = 0;
        }
        #endregion
        #region Snapped Vertical
        if (veritcalMovement > 0 && veritcalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (veritcalMovement > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (veritcalMovement < 0 && veritcalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (veritcalMovement < -0.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if (isSprinting) 
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    //Preventing movement during dialogue with the animation
    public void SetDialogueLocked(bool locked)
    {
        dialogueLocked = locked;

        if (locked)
        {
            ForceIdle();
        }
    }
    //Can only be in idle when in dialogue scene
    private void ForceIdle()
    {
        //Set animator state
        animator.SetFloat(horizontal, 0f);
        animator.SetFloat(vertical, 0f);

        //Check player state
        animator.SetBool("isJumping", false);
        animator.SetBool("isInteracting", false);
    }
}
