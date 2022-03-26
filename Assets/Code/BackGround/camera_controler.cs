using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controler : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;


    private void Update()
    { //room camera
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed );

        //Player camera
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    //public void movetonewroom(Transform _newRoom)
    //{
    //    currentPosX = _newRoom.position.x;
    //
}
