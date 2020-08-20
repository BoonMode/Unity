using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager gameManager;
    public GameObject CameraRig;
    public GameObject ActualCamera;
    float PanSpeed = 20;
    float PanLimit = 5000;

    float ScrollSpeed = 500;
    float ScrollMin = 25;
    float ScrollMax = 200;

    float CurrentDistance = 0;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void FixedUpdate()
    {
        CenterCameraOnCameraRig();
        WASD();
        MouseWheel();
        DragCamera();
        RotateCamera();
        ScrollSpeed();
    }
    void CenterCameraOnCameraRig()
    {
        ActualCamera.transform.LookAt(CameraRig.transform.position);
    }
    void WASD()
    {
        gameObject.transform.position += new Vector3(gameObject.transform.forward.x, 0, gameObject.transform.forward.z) * Input.GetAxis("W/S") * PanSpeed * Time.deltaTime / Time.timeScale;
        gameObject.transform.position += gameObject.transform.right * Input.GetAxis("A/D") * PanSpeed * Time.deltaTime / Time.timeScale;
    }
    void MouseWheel()
    {
        if (Input.GetAxis("MouseWheel") != 0 && gameManager.buildingPlacementManager.IsPlacing == false)
        {
            CurrentDistance = Vector3.Distance(CameraRig.transform.position, ActualCamera.transform.position);
            if (CurrentDistance > ScrollMin)
            {
                if (Input.GetAxis("MouseWheel") > 0)
                {
                    ActualCamera.transform.position += ActualCamera.transform.forward * Input.GetAxis("MouseWheel") * ScrollSpeed * Time.deltaTime / Time.timeScale;
                }
            }
            if (CurrentDistance < ScrollMax)
            {
                if (Input.GetAxis("MouseWheel") < 0)
                {
                    ActualCamera.transform.position += ActualCamera.transform.forward * Input.GetAxis("MouseWheel") * ScrollSpeed * Time.deltaTime / Time.timeScale;
                }
            }
        }
    }
    void DragCamera()
    {
        if (Input.GetAxis("LeftCtrl") != 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            PanSpeed = 500;
            ScrollSpeed = 5000;
            gameObject.transform.position += new Vector3(gameObject.transform.forward.x, 0, gameObject.transform.forward.z) * -Input.GetAxis("Mouse Y") * PanSpeed * Time.deltaTime;
            gameObject.transform.position += gameObject.transform.right * -Input.GetAxis("Mouse X") * PanSpeed * Time.deltaTime;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    void RotateCamera()
    {
        if (Input.GetAxis("Fire3") != 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            float DirectionX = Input.GetAxis("Mouse X");
            float DirectionY = Input.GetAxis("Mouse Y");

            float rotAmountX = DirectionX / Time.timeScale;
            float rotAmountY = DirectionY / Time.timeScale;

            Vector3 CameraRotation = gameObject.transform.rotation.eulerAngles;
            CameraRotation.y += rotAmountX;
            CameraRotation.x -= rotAmountY;
            if (CameraRotation.x < 0)
            {
                CameraRotation.x = 0;
            }
            if (CameraRotation.x > 60)
            {
                CameraRotation.x = 60;
            }

            gameObject.transform.rotation = Quaternion.Euler(CameraRotation);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    void ScrollSpeed()
    {
        if (Input.GetAxis("LeftShift") > 0)
        {
            PanSpeed = 100;
            ScrollSpeed = 5000;
        }
        else
        {
            ScrollSpeed = 1000;
            PanSpeed = 20;
        }
    }
}
