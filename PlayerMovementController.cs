using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float SprintSpeed = 20;
    [SerializeField] float WalkSpeed = 10;
    [SerializeField] float SprintAcceleration = 10;
    [SerializeField] float SprintDecceleration = 10;
    float CurrentMoveSpeed = 0;

    // horizontal and vertical move directions
    float Horizontal = 0;
    float Vertical = 0;

    [SerializeField] float WalkAcceleration = 3;

    // when moving vertically and horizontally it doubles the speed of the player
    float DualMovementDivider = 1.6f; 

    // Mouse aim settings
    Vector2 MouseWorldPostion;

    public float GetCurrentMoveSpeed { get => CurrentMoveSpeed; }

    private void Start()
    {
        CurrentMoveSpeed = WalkSpeed;
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }
    void PlayerMovement()
    {
        gameObject.transform.rotation = PlayerFaceMouse();
        Vertical = MovementInput(PositiveKey: InputManager.isPressingUp(), NegativeKey : InputManager.isPressingDown(), Axis : Vertical);
        Horizontal = MovementInput(PositiveKey : InputManager.isPressingRight(), NegativeKey: InputManager.isPressingLeft(), Axis : Horizontal);
        CurrentMoveSpeed = Sprint(SprintButton: InputManager.isPressingSprint(), CurrentMoveSpeed : CurrentMoveSpeed);
        if (Horizontal != 0 && Vertical != 0)
        {
            gameObject.transform.position += HorizontalMovement() / DualMovementDivider;
            gameObject.transform.position += VerticalMovement() / DualMovementDivider;
        }
        else
        {
            gameObject.transform.position += HorizontalMovement();
            gameObject.transform.position += VerticalMovement();
        }
    }
    Vector3 HorizontalMovement() => Vector3.right * CurrentMoveSpeed * Horizontal * Time.fixedDeltaTime;
    Vector3 VerticalMovement() => Vector3.up* CurrentMoveSpeed * Vertical* Time.fixedDeltaTime;
    float MovementInput(bool PositiveKey, bool NegativeKey, float Axis)
    {
        float returnValue = Axis;
        if (NegativeKey)
        {
            if (Axis > -1)
            {
                returnValue -= WalkAcceleration * Time.fixedDeltaTime;
            }
        }
        else if (PositiveKey)
        {
            if (Axis < 1)
            {
                returnValue += WalkAcceleration * Time.fixedDeltaTime;
            }
        }
        else
        {
            if (Axis > 0.1f)
            {
                returnValue -= WalkAcceleration * Time.fixedDeltaTime;

            }
            else if (Axis < -0.1f)
            {
                returnValue += WalkAcceleration * Time.fixedDeltaTime;
            }
            else
            {
                returnValue = 0;
            }
        }
        return returnValue;
    }
    float Sprint(bool SprintButton, float CurrentMoveSpeed) 
    {
        float returnValue = CurrentMoveSpeed;
        if (SprintButton)
        {
            if (CurrentMoveSpeed < SprintSpeed)
            {
                returnValue += SprintAcceleration * Time.fixedDeltaTime;
            }
        }
        else
        {
            if (CurrentMoveSpeed > WalkSpeed)
            {
                returnValue -= SprintDecceleration * Time.fixedDeltaTime;
            }
        }
        return returnValue;
    }
    Quaternion PlayerFaceMouse()
    {
        MouseWorldPostion = GetMousePosition();
        if (InputManager.isPressingUp())
        {
            if (MouseIsRightSideofPlayer())
            {
                return Quaternion.Euler(FlipLeft());
            }
            else
            {
                return Quaternion.Euler(FlipRight());
            }
        }
        else if (!MouseIsRightSideofPlayer())
        {
            return Quaternion.Euler(FlipRight());
        }
        else
        {
            return Quaternion.Euler(FlipLeft());
        }
    }
    Vector2 GetMousePosition() => MouseWorldPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
    Vector3 FlipLeft() => new Vector3(0, 180, 0);
    Vector3 FlipRight() => new Vector3(0, 0, 0);
    bool MouseIsRightSideofPlayer()
    {
        if (gameObject.transform.position.x > MouseWorldPostion.x + gameObject.transform.position.x)
        {
            return true;
        }
        return false;
    }
}
