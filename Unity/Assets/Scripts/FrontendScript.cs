using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FrontendScript : MonoBehaviour {

    public GameObject Logo = null;
    public GameObject Intro = null;
    public GameObject HowToPlay = null;

    enum FrontendState
    {
        kLogo,
        kIntro,
        kHowToPlay,
        kFadeOut
    }

    private FrontendState eState;
    private bool bLoadTriggered = false;

    // Use this for initialization
    void Start () {
        eState = FrontendState.kLogo;
    }
	
	// Update is called once per frame
	void Update () {

        if( Input.GetMouseButtonDown(0))
        { 
            switch( eState )
            {
                case FrontendState.kLogo:
                    Logo.SetActive(false);
                    Intro.SetActive(true);
                    eState = FrontendState.kIntro;
                    break;
                case FrontendState.kIntro:
                    Intro.SetActive(false);
                    HowToPlay.SetActive(true);
                    eState = FrontendState.kHowToPlay;
                    break;
                case FrontendState.kHowToPlay:
                    eState = FrontendState.kFadeOut;
                    Fade.GetInstance().FadeOn();
                    GetComponent<FadeOutAudio>().bTrigger = true;
                    break;
                case FrontendState.kFadeOut:
                    break;
                default:
                    break;
            }
        }

        if (eState == FrontendState.kFadeOut)
        {
            if( Fade.GetInstance().IsFading() == false && !bLoadTriggered )
            {
                bLoadTriggered = true;
                SceneManager.LoadScene("TrainScene");
            }
        }
	
	}
}
