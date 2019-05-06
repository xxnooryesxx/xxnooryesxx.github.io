using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour {

    Dictionary<GameObject, int> boxes = new Dictionary<GameObject, int>();
    int id_counter = 1;

    GameObject working_box;
    bool going_back = true;

	// Use this for initialization
	void Start () {
		
	}

    //Set ID to each new box
    void SaveNewBox(GameObject box)
    {
        if (box.transform.position.y > 0.251f)
        {
            return;
        }
        if (boxes.ContainsKey(box))
        {
            return;
        }
        boxes[box] = id_counter;
        id_counter++;
    }

    // Set the target position of box
    bool CleanBox(GameObject box)
    {
        Vector3 clean_pos = BoxCleanPos(boxes[box]);
        if (Vector3.Distance(box.transform.position, clean_pos) > 0.05f)
        {
            //Calculate the vector from box position to target position
            Vector3 to = clean_pos - box.transform.position;
            //Move box
            box.transform.position += to.normalized * Mathf.Min(0.1f, Vector3.Distance(box.transform.position, clean_pos));
            transform.position = box.transform.position + to.normalized * -0.5f;
            return false;
        }

        return true;
    }

    //Calculate the location of boxes by id number
    Vector3 BoxCleanPos(int id)
    {
        int n = (id-1) % 5;
        int row = (id-1) / 5;
        Vector3 v = new Vector3(-5f + n * 1.0f, 0.25f, -5f + row * 1.0f);
        return v;
    }

    //Link the robot and the box which it should work with by ID order
    bool LockBox(GameObject box)
    {
        BoxData d = box.GetComponent<BoxData>();
        if (d == null)
        {
            return false;
        }
        if (d.working_robot == null)
        {
            d.working_robot = gameObject;
        }

        if (d.working_robot != gameObject)
        {
            return false;
        }
        return true;
    }

    // Unlink the robot and box, then robot start finding next box.
    bool FreeBoxLock(GameObject box)
    {
        BoxData d = box.GetComponent<BoxData>();
        if (d == null)
        {
            return false;
        }
        if (d.working_robot == null)
        {
            return true;
        }
        if (d.working_robot != gameObject)
        {
            return false;
        }
        d.working_robot = null;
        return true;
    }

	// Update is called once per frame
	void Update () {

        //detecting any new box
        GameObject[] all = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject box in all)
        {
            SaveNewBox(box);
        }

        //Search any new box, idle state
        if (working_box == null)
        {
            foreach (var pair in boxes)
            {
                if (!LockBox(pair.Key))
                {
                    continue;
                }
                Vector3 clean_pos = BoxCleanPos(boxes[pair.Key]);
                if (Vector3.Distance(pair.Key.transform.position, clean_pos) > 0.05f)
                {
                    working_box = pair.Key;
                    break;
                }
            }
        }

        if (working_box != null)
        {
            // moving box state and first moving state
            if (going_back == false)
            {
                if(CleanBox(working_box))
                {
                    FreeBoxLock(working_box);
                    working_box = null;
                    going_back = true;

                }
            }

            //going to new box state
            else
            {
                if (Vector3.Distance(working_box.transform.position, transform.position) > 0.05f)
                {
                    //MoveTowards(current: Vector3, target: Vector3, maxDistanceDelta: float) : Vector3
                    Vector3 to = working_box.transform.position - transform.position;
                    float f = to.magnitude / 0.1f;
                    to /= f;
                    transform.position += to;
                }
                else
                {
                    going_back = false;
                }
            }
        }
	}
}
