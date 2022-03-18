using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    GameObject cam;
    [Range(0, 1)]
    public float speed;
    float startPosX, startPosY, tempX, distX, distY, length;

    public bool auto, createCopys, includeY;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>().gameObject;
        startPosX = cam.transform.position.x;
        startPosY = cam.transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        if (auto)
        {
            transform.position = new Vector3(speed, 0, 0);

            if (createCopys)
            {
                if (startPosX > length || startPosX < -length) transform.position = new Vector3(0, transform.position.y, transform.position.z);
            }
        }
        else
        {
            tempX = (cam.transform.position.x * (1 - speed));
            distX = (cam.transform.position.x * (speed));

            if (includeY)
            {
                distY = (cam.transform.position.y * speed);
            }

            transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

            if (createCopys)
            {
                if (tempX > startPosX + length) startPosX += length;
                else if (tempX < startPosX - length) startPosX -= length;
            }
        }

    }
}
