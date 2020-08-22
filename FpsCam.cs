using UnityEngine;

public class FpsCam : MonoBehaviour
{
    float PanSpeed = 10;
    float PanLimit = 1200;

    float ScrollSpeed = 500;
    float ScrollMin = 5;
    float ScrollMax = 100;

    private void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(gameObject.transform.forward.x, 0, gameObject.transform.forward.z) * Input.GetAxis("W/S") * PanSpeed * Time.deltaTime;
        gameObject.transform.position += gameObject.transform.right * Input.GetAxis("A/D") * PanSpeed * Time.deltaTime;
        gameObject.transform.position += Vector3.up * Input.GetAxis("MouseWheel") * ScrollSpeed * Time.deltaTime;

        if (Input.GetAxis("LeftCtrl") != 0)
        {
            PanSpeed = 500;
            ScrollSpeed = 5000;
            gameObject.transform.position += new Vector3(gameObject.transform.forward.x, 0, gameObject.transform.forward.z) * -Input.GetAxis("Mouse Y") * PanSpeed * Time.deltaTime;
            gameObject.transform.position += gameObject.transform.right * -Input.GetAxis("Mouse X") * PanSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Fire3") != 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            float DirectionX = Input.GetAxis("Mouse X");
            float DirectionY = Input.GetAxis("Mouse Y");

            float rotAmountX = DirectionX * 1;
            float rotAmountY = DirectionY * 1;

            Vector3 CameraRotation = gameObject.transform.rotation.eulerAngles;
            CameraRotation.y += rotAmountX;
            CameraRotation.x -= rotAmountY;

            gameObject.transform.rotation = Quaternion.Euler(CameraRotation);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetAxis("X") > 0)
        {
            PanSpeed = 100;
            ScrollSpeed = 5000;
        }
        else
        {
            ScrollSpeed = 1000;
            PanSpeed = 10;
        }
    }
    private void Update()
    {
        if (Vector3.Distance(new Vector3(0, 0, 0), gameObject.transform.position) > PanLimit)
        {
            gameObject.transform.position = new Vector3(0, gameObject.transform.position.y, 0);
        }
        if (gameObject.transform.position.y < ScrollMin)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, ScrollMin, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.y > ScrollMax)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, ScrollMax, gameObject.transform.position.z);
        }

    }
}