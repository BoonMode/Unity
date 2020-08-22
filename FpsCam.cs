using UnityEngine;

public class FpsCam : MonoBehaviour
{
    [SerializeField] float LookSpeed = 10;

    private void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(gameObject.transform.forward.x, gameObject.transform.forward.y, gameObject.transform.forward.z) * Input.GetAxis("W/S") * LookSpeed * Time.deltaTime;
        gameObject.transform.position += gameObject.transform.right * Input.GetAxis("A/D") * LookSpeed * Time.deltaTime;

        float DirectionX = Input.GetAxis("Mouse X");
        float DirectionY = Input.GetAxis("Mouse Y");

        float rotAmountX = DirectionX * 1;
        float rotAmountY = DirectionY * 1;

        Vector3 CameraRotation = gameObject.transform.rotation.eulerAngles;
        CameraRotation.y += rotAmountX;
        CameraRotation.x -= rotAmountY;

        gameObject.transform.rotation = Quaternion.Euler(CameraRotation);
        
    }
}