/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralaxControler : MonoBehaviour
{
    GameObject pref;
    Parallax paralax;

    void Start()
    {
        for(int i = 0; i <transform.childCount; i++)
        {
            paralax.transform.GetChild(i).GetComponent<Parallax>(); 
            if(paralax != null && paralax.createCopys)
            {
                pref = paralax.gameObject;
                paralax.gameObject.GetComponent <Parallax>().enabled = false;

                GameObject childL = Instantiate(pref, paralax.gameObject.transform);
                childL.name = "L";

                GameObject childR = Instantiate(pref, paralax.gameObject.transform);
                childL.name = "R";

                paralax.gameObject.GetComponent<Parallax>().enabled = true; 

                childL.transform.localScale = new Vector3(1, 1, 1);
                childR.transform.localScale = new Vector3(1, 1, 1);

                childL.transform.localPosition = new Vector3(-(paralax.gameObject.GetComponent<SpriteRenderer>().bounds.size.x) / paralax.transform.localScale.x,0,0);
                childR.transform.localPosition = new Vector3(-(paralax.gameObject.GetComponent<SpriteRenderer>().bounds.size.x) / paralax.transform.localScale.x, 0, 0);
            }
            pref = null;
            paralax = null;
            
        }
    }

    
}*/
