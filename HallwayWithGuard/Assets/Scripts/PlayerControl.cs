using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    float speed = 6.0f;
    float rotSpeed = 100.0f;
    float jumpSpeed = 10f;
    float gravity = 20.0f;

    private GameObject guardRef;
    private bool guardIsStopped = false;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 toEnemyDir;
    private float toEnemyDot;
    private Ray lookRay;
    private Ray enemyRay;
    private CharacterController cc;
    private Camera playerCam;
    // Start is called before the first frame update
    void Start()
    {
        guardRef = GameObject.FindGameObjectWithTag("Guard");
        cc = GetComponent<CharacterController>();
        playerCam = FindObjectOfType<Camera>();
        gameObject.transform.position = new Vector3(0, 5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        toEnemyDir = guardRef.transform.position - transform.position;
        lookRay.origin = transform.position;
        lookRay.direction = transform.forward;
        enemyRay.origin = transform.position;
        enemyRay.direction = (guardRef.transform.position -transform.position);
        Debug.DrawRay(lookRay.origin, lookRay.direction*100, Color.blue);
        Debug.DrawRay(enemyRay.origin, enemyRay.direction *100 , Color.red);
        toEnemyDot = Vector3.Dot(transform.forward, toEnemyDir);
        print(toEnemyDot);

        if(toEnemyDot < 0 && !guardIsStopped)
        {
           guardIsStopped = guardRef.GetComponent<AI_Controller>().Freeze(true);
        }
        else if(toEnemyDot >= 0 && guardIsStopped)
        {
            guardIsStopped = guardRef.GetComponent<AI_Controller>().Freeze(false);
        }

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

        playerCam.transform.Rotate(Input.GetAxis("Mouse Y")*rotSpeed*Time.deltaTime, 0, 0);
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, 0);

        //apply gravity
        moveDir.y = moveDir.y - (gravity * Time.deltaTime);
        cc.Move(moveDir * Time.deltaTime);
    }
}
