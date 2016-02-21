using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class DarknessArea : MonoBehaviour {

    public List<GameObject> affectedObjects;
    private SpriteRenderer renderer; 
    
    // Use this for initialization
	void Start () {
        renderer = GetComponent<SpriteRenderer>();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
