using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    float speed = 6.0f;
    float rotSpeed = 100.0f;
    float jumpSpeed = 10f;
    float gravity = 20.0f;

    private Vector3 moveDir = Vector3.zero;
    private CharacterController cc;
    private GameObject playerCam;

    public GameObject mirror;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        mirror = GameObject.FindGameObjectWithTag("Mirror");
        mirror.SetActive(false);
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (cc.isGrounded)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);
            moveDir = moveDir * speed;

            if (Input.GetButton("Jump"))
            {
                moveDir.y = jumpSpeed;
            }
        }

        playerCam.transform.Rotate(Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime, 0, 0);
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, 0);

        //apply gravity
        moveDir.y = moveDir.y - (gravity * Time.deltaTime);
        cc.Move(moveDir * Time.deltaTime);
    }
}
