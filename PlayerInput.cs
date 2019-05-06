using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {

    public GameObject box_prefeb;

    [Range(0.0f, 10.0f)]
    public float times = 5;
    public Slider SL;

    // Use this for initialization
    void Start() {
    }

    void OnClickGround()
    {
        //For generate box by click on ground.
        Camera cam = Camera.main;       
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitt = new RaycastHit();
        Physics.Raycast(ray, out hitt, 100);
        //Debug.DrawLine(cam.transform.position, ray.direction, Color.red);

        if (hitt.transform!=null && hitt.transform.name=="Ground")
        {
            Vector3 p = new Vector3(hitt.point.x, 5, hitt.point.z);
            Instantiate(box_prefeb, p, Quaternion.Euler(0, 0, 0));
        }
    }

    // Update is called once per frame
    void Update() {

        //Update the slider value to time
        SL.onValueChanged.AddListener(delegate {
            times = SL.value;
        });

        //For generate box by automation
        times -= Time.deltaTime; 
        if (times < 0) 
        {
            float xRange = Random.Range(-5, 5);
            float yRange = Random.Range(-5, 5);

            Vector3 positionP = new Vector3(xRange, 5, yRange);

            Instantiate(box_prefeb, positionP, Quaternion.Euler(0, 0, 0));

            times = Random.Range(0, SL.value);

        }

        //For click on ground
        if (Input.GetMouseButtonDown(0))
        {
            OnClickGround();
        }
    }
    

}
