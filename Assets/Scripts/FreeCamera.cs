using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
	public bool active, allowMove;
	[SerializeField] float MoveSpeed = 1;
	[SerializeField] float MoveAccelerationSpeed = 1;

	[SerializeField] float RotateSpeed = 1;

	Vector3 Velocity;
	Vector3 MousePos;

	[HideInInspector] public bool hold;
	bool cc = true; //cursor control
    void Awake()
    {
		
    }

    void Update()
    {
		if (active)
		{
            if (allowMove)
            {
                Velocity.z = Mathf.MoveTowards(Velocity.z, Input.GetAxis("Vertical") * MoveSpeed, MoveAccelerationSpeed * Time.deltaTime);
                Velocity.x = Mathf.MoveTowards(Velocity.x, Input.GetAxis("Horizontal") * MoveSpeed, MoveAccelerationSpeed * Time.deltaTime);
                Velocity.y = Mathf.MoveTowards(Velocity.y, (Input.GetKey(KeyCode.E) ? 1 : Input.GetKey(KeyCode.Q) ? -1 : 0) * MoveSpeed * 0.3f, MoveAccelerationSpeed * Time.deltaTime);

                if (!hold) transform.position += transform.TransformDirection(Velocity);
            }

            var mousePosDelta = Input.mousePosition - MousePos;
            MousePos = Input.mousePosition;

            if (!hold) transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * RotateSpeed, Vector3.left);
            if (!hold) transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotateSpeed, Vector3.up);

            var eulerAngles = transform.eulerAngles;
            eulerAngles.z = 0;
            transform.eulerAngles = eulerAngles;

            if (!hold && cc)
            {
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView + Input.mouseScrollDelta.y, 50, 120);
        }
	}
}