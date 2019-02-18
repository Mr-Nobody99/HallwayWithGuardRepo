using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror_Pickup : MonoBehaviour
{

    private GameObject playerRef;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            print("Git mirror");
            gameObject.SetActive(false);
            playerRef.GetComponent<PlayerControl>().mirror.SetActive(true);

        }
    }
}
