using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class Mine : MonoBehaviour
{
    public float ExplosionTime = 3f;
    public AudioClip explosion;

    Animation anim;
    int count = 0;
    float timer = 0;
    bool exploded = false;
    
    void Awake()
    {
        anim = GetComponent<Animation>();
    }   
     
	void OnTriggerEnter2D( Collider2D other )
    {
        if ( other.CompareTag( "Player" ) )
        {
            count++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            count--;
        }
    }

    void Update()
    {
        if (exploded) return;
        if ( count > 0 )
        {
            anim["mine_blink"].speed = 5f;
            timer += Time.deltaTime;
            if ( timer > ExplosionTime )
            {
                //DIE
                AudioSource.PlayClipAtPoint(explosion, Camera.main.transform.position);
                exploded = true;
                Conductor.Stop();
            }
        }
        else
        {
            anim["mine_blink"].speed = 1f;
            timer = 0;
        }
    }
}
