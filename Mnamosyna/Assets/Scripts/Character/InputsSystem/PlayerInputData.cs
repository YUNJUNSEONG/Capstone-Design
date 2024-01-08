using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerInputData : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool leftAttack;
    public bool rightAttack;
    public bool dash;

    [Header("Movement Setting")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    /* public method*/
    #region public method
    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    public void LeftAttackInput()
    {
        leftAttack = true;
    }

    public void RightAttackInput()
    {
        rightAttack = true;
    }

    public void DashInput()
    {
        dash = true;
    }

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
    }

    public void OnLeftAttack()
    {
        if (PlayableCharacterController.isAttack || PlayableCharacterController.isDash)
            return;

        LeftAttackInput();
    }

    public void OnRightAttack()
    {
        if (PlayableCharacterController.isAttack || PlayableCharacterController.isDash)
            return;

        RightAttackInput();
    }

    public void OnDash()
    {
        if (PlayableCharacterController.isAttack || PlayableCharacterController.isDash)
            return;

        DashInput();
    }
#endif
    #endregion

    /* private method */
    #region private method
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        // Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.lockState = CursorLockMode.None;
    }
    #endregion
}
