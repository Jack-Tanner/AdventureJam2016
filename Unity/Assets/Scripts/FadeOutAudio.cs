using UnityEngine;
using System.Collections;

public class FadeOutAudio : MonoBehaviour {

    public AudioSource FadeOutSource;
    public bool bTrigger = false;

    private bool bFadingOut = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (bTrigger)
            bFadingOut = true;

        if( bFadingOut )
        {
            if (FadeOutSource.isPlaying)
            {
                if (FadeOutSource.volume > 0.05f)
                {
                    float fNewVolume = FadeOutSource.volume - (0.05f * Time.deltaTime);
                    FadeOutSource.volume = fNewVolume;
                }
                else
                    FadeOutSource.Stop();
            }
        }

	}
}
