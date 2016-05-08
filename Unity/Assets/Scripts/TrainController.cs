using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainController : MonoBehaviour
{
    List<Train> m_trainList;

    Train m_playersTrain = null;

    // Use this for initialization
    void Start()
    {
        m_trainList = new List<Train>();

        // Find all trains and sort them by x position.
        GameObject[] trains = GameObject.FindGameObjectsWithTag( "Train" );
        if( trains != null && trains.Length > 0 )
        {
            for( int i = 0; i < trains.Length; ++i )
            {
                Train train = trains[i].GetComponent<Train>();
                if( train != null )
                {
                    m_trainList.Add( train );
                }
                else
                {
                    Debug.LogWarning( "TrainController:: GameObject with tag 'Train' but no Train script." );
                }
            }

            m_trainList.Sort( ( p1, p2 ) => p1.transform.position.x.CompareTo( p2.transform.position.x ) );
        }
        else
        {
            Debug.LogWarning( "TrainController:: I can't find any trains." );
        }

        // Find where the player is and what train he is in.

        for( int i = 0; i < m_trainList.Count; ++i )
        {
            Player player = m_trainList[i].gameObject.GetComponentInChildren<Player>();
            if( player != null )
            {
                m_playersTrain = m_trainList[i];
                break;
            }
        }
    }
}
