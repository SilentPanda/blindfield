using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

    public int xSize, ySize;
    int boundaries = 150;
    private Vector3[] vertices;
    private Mesh mesh;
    //private LineRenderer line;
    private Vector3 startPos;
    private Vector3 endPos;
    public Camera camera;

    private void Awake()
    {
        if (camera)
        {
            xSize = Screen.currentResolution.width - boundaries;
            ySize = Screen.currentResolution.height - boundaries;
        }
        
        //StartCoroutine(Generate());
    }

    // Use this for initialization
    void Start () {
       
        createLine("l1", camera.ScreenToWorldPoint( new Vector3(-xSize/2, ySize/2, 0)), camera.ScreenToWorldPoint(new Vector3(xSize/2, ySize/2, 0)) );
        createLine("l2", camera.ScreenToWorldPoint(new Vector3(xSize / 2, ySize / 2, 0)), camera.ScreenToWorldPoint( new Vector3(xSize / 2, -ySize / 2, 0)));
        createLine("l3", camera.ScreenToWorldPoint(new Vector3(xSize / 2, -ySize / 2, 0)), camera.ScreenToWorldPoint(new Vector3(-xSize / 2, -ySize / 2, 0)));
        createLine("l4", camera.ScreenToWorldPoint(new Vector3(-xSize / 2, -ySize / 2, 0)), camera.ScreenToWorldPoint(new Vector3(-xSize / 2, ySize / 2, 0)));
        //addColliderToLine();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    

    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = -ySize / 2; y <= ySize/2; y++)
        {
            for (int x = -xSize / 2; x <= xSize/2; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
            }
        }
        mesh.vertices = vertices;

       /* int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.triangles = triangles;*/
        yield return wait;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    private void createLine(string name, Vector3 start, Vector3 end)
    {
        LineRenderer line = new GameObject(name).AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Diffuse"));
        line.SetVertexCount(2);
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        line.SetWidth(0.5f, 0.5f);
        line.SetColors(Color.black, Color.black);
       // line.useWorldSpace = true;
    }

   /* private void addColliderToLine()
    {
        BoxCollider col = new GameObject("Collider").AddComponent<BoxCollider>();
        col.transform.parent = line.transform; // Collider is added as child object of line
        float lineLength = Vector3.Distance(startPos, endPos); // length of line
        col.size = new Vector3(lineLength, 0.1f, 1f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
        Vector3 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint; // setting position of collider object
        // Following lines calculate the angle between startPos and endPos
        float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));
        if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        {
            angle *= -1;
        }
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        col.transform.Rotate(0, 0, angle);
    }*/

}
