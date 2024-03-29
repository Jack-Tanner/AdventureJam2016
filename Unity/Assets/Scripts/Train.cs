﻿using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour
{
    Vector3 startingPoint;
    static float bobScale = 0.001f;
    static float fBobUpperRange = 0.2f;
    // Use this for initialization
    void Start()
    {
        startingPoint = transform.position;
        StartCoroutine( "BobTrain" );
    }

    void Update()
    {
        if( TrainJourneyManager.GetInstance().m_fTrainPosition > 30.0f )
        {
            bobScale = 0.03f;
            fBobUpperRange = 0.1f;
        }
        else
        {
            bobScale = 0.001f;
            fBobUpperRange = 0.2f;
        }
    }

    /// <summary>
    /// Sets how much the train bobs by.
    /// </summary>
    /// <param name="amount"></param>
    static public void SetBobAmount( float amount )
    {
        bobScale = amount * 0.1f;
    }

    IEnumerator BobTrain()
    {
        while( true )
        {
            for( int i = 0; i < 3; ++i )
            {
                transform.position = new Vector3( startingPoint.x, startingPoint.y + ((float)i * bobScale), startingPoint.z );
                yield return new WaitForSeconds( Random.Range( 0.03f, fBobUpperRange ) );
            }
        }
    }
}
