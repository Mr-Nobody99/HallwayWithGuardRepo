using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    float speed = 6.0f;
    float rotSpeed = 100.0f;
    float jumpSpeed = 10f;
    float gravity = 20.0f;

    //private GameObject guardRef;
   // private GameObject guardRef2;
    //private bool guardIsStopped = false;
   // private bool guard2IsStopped = false;
    private Vector3 moveDir = Vector3.zero;
   // private Vector3 toEnemyDir1;
    //private Vector3 toEnemyDir2;
   // private float toEnemyDot1;
   // private float toEnemyDot2;
    //private Ray enemyRay1;
    //private Ray enemyRay2;
    //private Ray lookRay;
    private CharacterController cc;
    private GameObject playerCam;
    // Start is called before the first frame update
    void Start()
    {
        //guardRef = GameObject.FindGameObjectWithTag("Guard1");
       // guardRef2 = GameObject.FindGameObjectWithTag("Guard2");
        cc = GetComponent<CharacterController>();
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
        //gameObject.transform.position = new Vector3(0, 5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //toEnemyDir1 = guardRef.transform.position - transform.position;
        //toEnemyDir2 = guardRef2.transform.position - transform.position;
        //set look ray direction
        //lookRay.origin = transform.position;
        //lookRay.direction = transform.forward;
        //create ray to enemy1
        //enemyRay1.origin = transform.position;
        //enemyRay1.direction = (guardRef.transform.position - transform.position);
        //create ray to enemy2
        //enemyRay2.origin = transform.position;
        //enemyRay2.direction = (guardRef2.transform.position - transform.position);

        //Debug.DrawRay(lookRay.origin, lookRay.direction*100, Color.blue);
        //Debug.DrawRay(enemyRay1.origin, enemyRay1.direction *100 , Color.red);
        //Debug.DrawRay(enemyRay2.origin, enemyRay2.direction * 100, Color.green);

        //toEnemyDot1 = Vector3.Dot(transform.forward, toEnemyDir1);
        //toEnemyDot2 = Vector3.Dot(transform.forward, toEnemyDir2);
        //print(toEnemyDot1);
        //print(toEnemyDot2);

        //if(toEnemyDot1 < 0 && !guardIsStopped)
        //{
        //   guardIsStopped = guardRef.GetComponent<AI_Controller>().Freeze(true);
        //}
        //else if(toEnemyDot1 >= 0 && guardIsStopped)
        //{
        //    guardIsStopped = guardRef.GetComponent<AI_Controller>().Freeze(false);
        //}

        //if (toEnemyDot2 < 0 && !guard2IsStopped)
        //{
        //    guard2IsStopped = guardRef2.GetComponent<AI_Controller>().Freeze(true);
        //}
        //else if (toEnemyDot2 >= 0 && guard2IsStopped)
        //{
        //    guard2IsStopped = guardRef2.GetComponent<AI_Controller>().Freeze(false);
        //}

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
