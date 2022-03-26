/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    [SerializeField]private Transform previousRoom;
    [SerializeField]private Transform nextRoom;
    [SerializeField] private camera_controler cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
                cam.movetonewroom (nextRoom);
            else
                cam.movetonewroom (previousRoom);
        }
    }
}*/
