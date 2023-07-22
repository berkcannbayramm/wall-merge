using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovement
{
    [SerializeField] private VariableJoystick joystick;
    [Space, Space]
    [SerializeField] private Rigidbody rb;
    [Space, Space]
    [SerializeField] private float speed;
    bool IMovement.IsMoving { get; set; }

    public void Move()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 addedPos = new Vector3(horizontal, Physics.gravity.y * Time.deltaTime, vertical);
        Vector3 direction = Vector3.forward * vertical + Vector3.right * horizontal;

        rb.velocity = addedPos.normalized * speed;

        if (addedPos == Vector3.zero)
        {
            ((IMovement)this).IsMoving = false;
        }
        else if (direction != Vector3.zero)
        {
            ((IMovement)this).IsMoving = true;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            ((IMovement)this).IsMoving = false;
        }

    }

    public void Rotation()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = Vector3.forward * vertical + Vector3.right * horizontal;

        if(direction!=Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);
    }
    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }
}
