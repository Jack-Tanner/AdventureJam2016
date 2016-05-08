using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour
{
    Vector3 startingPoint;
    public float bobScale;

    // Use this for initialization
    void Start()
    {
        startingPoint = transform.position;
        StartCoroutine( "BobTrain" );
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BobTrain()
    {
        while( true )
        {
            for( int i = 0; i < 3; ++i )
            {
                transform.position = new Vector3( startingPoint.x, startingPoint.y + ((float)i * bobScale), startingPoint.z );
                yield return new WaitForSeconds( Random.Range( 0.01f, 0.2f ) );
            }
        }
    }
}
