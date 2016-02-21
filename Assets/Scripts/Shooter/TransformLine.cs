using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class TransformLine : MonoBehaviour
{
	public Transform[] transforms;

	LineRenderer line;

	void Awake()
	{
		line = GetComponent<LineRenderer> ();
		line.SetVertexCount (transforms.Length);
	}

	// Update is called once per frame
	void LateUpdate () 
	{
		for( int i = 0; i < transforms.Length; i++ )
		{
			line.SetPosition (i, transforms [i].position);
		}
	}
}