using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Character : MonoBehaviour {

    [System.Serializable]
    public class CharacterStateData
    {
        public string[] dialogue;
        public int dialoguePos;
        public string ItemOfInterest;
    }
    public Text m_Text;
    public GameObject m_SpeechCanvas;
    public CharacterStateData[] m_CharacterStateData;
    public int m_CurrentState = 0;

    //Returns true when there was speech, false when finished
    public bool SpeakToNPC()
    {
        CharacterStateData stateData = m_CharacterStateData[m_CurrentState];
        if (stateData.dialoguePos >= stateData.dialogue.Length)
        {
            stateData.dialoguePos = 0;
            m_Text.text = "";
            m_SpeechCanvas.SetActive(false);
            return false;
        }
      
        string speech = stateData.dialogue[stateData.dialoguePos];
        stateData.dialoguePos++;
        m_Text.text = speech;
        m_SpeechCanvas.SetActive(true);
        return true;
    }

    public void GiveItem(string item)
    {
        if(item == m_CharacterStateData[m_CurrentState].ItemOfInterest)
        {
            MoveToNextState();
        }

    }

    public string GetItemOfInterest()
    {
        return m_CharacterStateData[m_CurrentState].ItemOfInterest;
    }

    private void MoveToNextState()
    {
        m_CurrentState++;
    }




}
