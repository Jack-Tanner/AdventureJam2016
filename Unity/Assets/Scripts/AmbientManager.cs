using UnityEngine;
using System.Collections;

public class AmbientManager : MonoBehaviour {

    public AudioSource DaySound;
    public AudioSource NightSound;

    public float VolumeSpeed = 0.05f;
    private float VolumeThreshold = 0.05f;


    bool bDay = true;
    bool bGoingToNight = false;
    bool bGoingToDay = true;

	// Use this for initialization
	void Start () {
        DaySound.Play();
	}
	
	// Update is called once per frame
	void Update () {

        if( bGoingToDay )
        {
            if( DaySound.isPlaying )
            {
                if( DaySound.volume < 1.0f )
                {
                    float newVolume = DaySound.volume + (VolumeSpeed * Time.deltaTime);
                    newVolume = Mathf.Clamp(newVolume, 0.0f, 1.0f);
                    DaySound.volume = newVolume;
                }
            }
            else
            {
                DaySound.Play();
                DaySound.volume = VolumeThreshold;
            }

            if( NightSound.isPlaying )
            {
                if( NightSound.volume > VolumeThreshold )
                {
                    float newVolume = NightSound.volume - (VolumeSpeed * Time.deltaTime);
                    newVolume = Mathf.Clamp(newVolume, 0.0f, 1.0f);
                    NightSound.volume = newVolume;
                }
                else
                {
                    NightSound.Stop();
                }
            }
        }
        else if( bGoingToNight )
        {
            if (NightSound.isPlaying)
            {
                if (NightSound.volume < 1.0f)
                {
                    float newVolume = NightSound.volume + (VolumeSpeed * Time.deltaTime);
                    newVolume = Mathf.Clamp(newVolume, 0.0f, 1.0f);
                    NightSound.volume = newVolume;
                }
            }
            else
            {
                NightSound.Play();
                NightSound.volume = VolumeThreshold;
            }

            if (DaySound.isPlaying)
            {
                if (DaySound.volume > VolumeThreshold)
                {
                    float newVolume = DaySound.volume - (VolumeSpeed * Time.deltaTime);
                    newVolume = Mathf.Clamp(newVolume, 0.0f, 1.0f);
                    DaySound.volume = newVolume;
                }
                else
                {
                    DaySound.Stop();
                }
            }
        }

        TrainJourneyManager journeyManager = TrainJourneyManager.GetInstance();
        if (journeyManager)
        {
            if (bDay != journeyManager.IsDayTime())
            {
                if (bDay)
                {
                    bGoingToNight = true;
                    bGoingToDay = false;
                }
                else
                {
                    bGoingToDay = true;
                    bGoingToNight = false;
                }
                bDay = journeyManager.IsDayTime();
            }
        }
	    
	}
}
