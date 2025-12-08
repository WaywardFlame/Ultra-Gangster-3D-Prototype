using UnityEngine;

public class MouseLook : MonoBehaviour {
    public float mouseSensitivity = 100f;
    public Transform playerBody; 
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } 

    void Update()
    {
        // camera moves faster at lower framerates, slower at higher framerates
        // I have no clue how to fix, nor do I have the time to fix this
        // Only solution I have mind is allowing the player to numerically change
        // the sensitivity of the camera
        // 50 for 30fps, 100 for 60fps, 150 for 120fps, 200 for 240fps
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);  
    } 
}
