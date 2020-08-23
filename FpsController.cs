using UnityEngine;

public class FpsController : MonoBehaviour
{
    [SerializeField] Camera camera;

    [SerializeField] float MoveSpeed = 10;
    [SerializeField] float LookSpeed = 10; 

    float XRotation;
    float YRotation;

    [SerializeField] Rigidbody rigidbody;
    [SerializeField] int JumpForce = 10;

    bool Grounded;

    private void FixedUpdate()
    {
        WASD();
        PlayerRotation();
        CameraRotation();
        Jump();
    }
    void WASD()
    {
        gameObject.transform.position += gameObject.transform.forward * Input.GetAxis("W/S") * MoveSpeed * Time.deltaTime;
        gameObject.transform.position += gameObject.transform.right * Input.GetAxis("A/D") * MoveSpeed * Time.deltaTime;
    }
    void PlayerRotation()
    {
        YRotation += Input.GetAxis("Mouse X") * LookSpeed;
        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, YRotation, gameObject.transform.rotation.eulerAngles.z);
    }
    void CameraRotation()
    {
        XRotation += Input.GetAxis("Mouse Y") * LookSpeed;
        XRotation = Mathf.Clamp(XRotation, -80, 80);
        camera.transform.rotation = Quaternion.Euler(-XRotation, camera.transform.rotation.eulerAngles.y, camera.transform.rotation.eulerAngles.z);
    }
    void Jump()
    {
        if (Input.GetAxis("Jump") > 0 && Grounded)
        {
            rigidbody.velocity = gameObject.transform.up * JumpForce;
            Grounded = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            Grounded = true;
        }
    }
}