using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class DarknessArea : MonoBehaviour {

    public List<GameObject> affectedObjects;
    private SpriteRenderer r; 
    
    // Use this for initialization
	void Start () {
        r = GetComponent<SpriteRenderer>();
        r.enabled = false;
        
	}
	
	// Update is called once per frame
	void Update () {
            if (Input.GetKeyDown("joystick button 0")) //press A
            {
                foreach (GameObject o in affectedObjects)
                 {
                    SpriteRenderer s = o.GetComponent<SpriteRenderer>();
                    s.color = Color.white;
                    s.sortingOrder = 2;
                 }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        r.enabled = true;
        foreach (GameObject o in affectedObjects)
        {
            SpriteRenderer s = o.GetComponent<SpriteRenderer>();
            s.color = Color.black;
            s.sortingOrder = 2;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        r.enabled = false;
        foreach (GameObject o in affectedObjects)
        {
            SpriteRenderer s = o.GetComponent<SpriteRenderer>();
            s.color = Color.black;
            s.sortingOrder = 2;

        }
    }
    
 }
