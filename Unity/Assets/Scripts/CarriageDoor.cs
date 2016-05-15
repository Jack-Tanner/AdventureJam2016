using UnityEngine;
using System.Collections;

public class CarriageDoor : Interactable
{
    public Transform    m_OwningTrain;
    public CarriageDoor m_TransitionToDoor;

    void Start()
    {
        if( m_OwningTrain == null )
        {
            Debug.LogError( "No owning train set on " + name );
        }

        if( m_TransitionToDoor == null )
        {
            Debug.LogError( "No transition to door set on " + name );
        }
    }

    public override void ThenDo()
    {
        if( m_TransitionToDoor != null )
        {
            Player.GetInstance().transform.parent = m_TransitionToDoor.m_OwningTrain.transform;
            Player.GetInstance().WalkToLocation( m_TransitionToDoor.GetWalkToLocation() );

            CameraController.GetInstance().MoveToPosition( m_TransitionToDoor.m_OwningTrain.transform.position );
        }
    }

    public override bool Highlightable()
    {
        return true;
    }
}
