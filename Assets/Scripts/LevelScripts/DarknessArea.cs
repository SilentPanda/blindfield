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
        //if (renderer.enabled)
       // {
            if (Input.GetKeyDown("space"))
            {
                SwapColors();
            }
       // }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        r.enabled = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        r.enabled = false;
    }
    private void SwapColors()
    {
        Debug.Log("swapping");
        foreach(GameObject o in affectedObjects)
        {
            SpriteRenderer r = o.GetComponent<SpriteRenderer>();
            r.color = Color.white;
            r.sortingOrder = 2;
            
        }
    }
 }
