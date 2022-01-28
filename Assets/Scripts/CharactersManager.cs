using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharactersManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PlayerPrefab = null;
    private Dictionary<InputControl, PlayableCharacter> m_CharactersList = new Dictionary<InputControl, PlayableCharacter>();

    //Called when any binded key is pressed
    public void CreateCharacter(InputAction.CallbackContext p_Context)
    {
        if (p_Context.started)
        {
            //If a PlayableCharacter with that input already exists
            if (m_CharactersList.TryGetValue(p_Context.control, out PlayableCharacter l_PlayableCharacter))
            {
                //We tell the PlayableCharacter his input has been pressed
                l_PlayableCharacter.InputPressed();
            }
            else
            {
                //Else we spawn a new PlayableCharacter
                PlayerInput l_NewPlayer = PlayerInput.Instantiate(m_PlayerPrefab);
                PlayableCharacter l_NewCharacter = l_NewPlayer.GetComponent<PlayableCharacter>();
                //Assign it his input (which the one that was just pressed)
                l_NewCharacter.MainControl = p_Context.control;
                //And add it to the list of existing PlayableCharacters
                m_CharactersList.Add(p_Context.control, l_NewCharacter);

                //Then we call the function for newly joined players
                NewPlayerJoined(l_NewCharacter);
            }
        }
    }


    private void NewPlayerJoined(PlayableCharacter p_NewPlayer)
    {
        Debug.Log(p_NewPlayer + " JOINED !");
    }
}
