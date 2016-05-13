using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class TrainJourneyManager : MonoBehaviour
{

    public Scene currentlyLoadedScene;

    [System.Serializable]
    public class TrainJourney
    {
        [System.Serializable]
        public enum SceneLocation
        {
            Station,
            Crossing,
            RockyDesert,
            SwitchBox,
            Canyon,
            Bridge,
            River,
            Tunel,
            SignalTower,
            CrashSite
        }

        public SceneLocation location;
        public string scene;
        public int distance;
        public bool othersideOfTrack = false;
        public bool isDayTime = true;
        public bool isPuzzle = false;
    }

    public TrainJourney[] trainJourney;

    public void GoToTrain()
    {
        if (currentlyLoadedScene.isLoaded)
        {
            SceneManager.UnloadScene(currentlyLoadedScene.name);
        }

        Scene mainScene = SceneManager.GetSceneByName("TrainScene");
        PositionPlayerInScene(mainScene);
    }


    public void GoToLocationOnJourney(TrainJourney tJ)
    {

        if (currentlyLoadedScene.isLoaded)
        {
            SceneManager.UnloadScene(currentlyLoadedScene.name);
        }

        SceneManager.LoadScene(tJ.scene, LoadSceneMode.Additive);
        currentlyLoadedScene = SceneManager.GetSceneByName(tJ.scene);

        PositionPlayerInScene(currentlyLoadedScene);

    }


    public void PositionPlayerInScene(Scene s)
    {
        GameObject[] rootObjects = s.GetRootGameObjects();
        for (int i = 0; i < rootObjects.Length; ++i)
        {
            if (rootObjects[i].name == "SpawnPoint")
            {
                Player.GetInstance().transform.position = rootObjects[i].transform.position;
            }
        }
    }
}
