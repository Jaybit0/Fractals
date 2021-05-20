using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Range(0.01f, 1f)]
    public float movementSpeed = 0.05f;

    [Range(0f, 10f)]
    public float mouseSensitivity = 1;

    private Camera cam;
    private Vector2 turn;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position += gameObject.transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position -= gameObject.transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position -= gameObject.transform.right * movementSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position += gameObject.transform.right * movementSpeed;
        }

        turn.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        turn.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
