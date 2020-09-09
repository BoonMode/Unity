using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerMovementController Player;
    [SerializeField] float CameraDampening = 2; 
    float CameraPlayerXDifference, CameraPlayerYDifference;

    // when moving vertically and horizontally it doubles the speed of the camera
    [SerializeField] float DualMovementDivider = 1.6f;

    private void FixedUpdate()
    {
        if (Player != null)
        {
            GetCameraPlayerPositionDifference();
            CameraChasePlayer();
        }
        else if (GameManager.Instance.GetCurrentPlayerObject != null)
        {
            Player = GameManager.Instance.GetCurrentPlayerObject.GetComponent<PlayerMovementController>();
        }
    }
    void GetCameraPlayerPositionDifference()
    {
        CameraPlayerXDifference = gameObject.transform.position.x - Player.transform.position.x;
        CameraPlayerYDifference = gameObject.transform.position.y - Player.transform.position.y;
    }
    void CameraChasePlayer()
    {
        if (DualInput())
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
    Vector3 HorizontalMovement()
    {
        return Vector3.right * -CameraPlayerXDifference * Player.GetCurrentMoveSpeed * Time.fixedDeltaTime / CameraDampening;
    }
    Vector3 VerticalMovement()
    {
        return Vector3.up * -CameraPlayerYDifference * Player.GetCurrentMoveSpeed * Time.fixedDeltaTime / CameraDampening;
    }
    bool DualInput()
    {
        if (CameraPlayerXDifference > 1 && CameraPlayerYDifference > 1 || CameraPlayerXDifference < -1 && CameraPlayerYDifference < -1 || CameraPlayerXDifference < -1 && CameraPlayerYDifference > 1 || CameraPlayerXDifference > 1 && CameraPlayerYDifference < -1)
        {
            return true;
        }
        return false;
    }
}
