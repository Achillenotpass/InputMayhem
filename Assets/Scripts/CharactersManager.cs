using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharactersManager : MonoBehaviour
{
    [SerializeField]
    private DirectionManager m_DirectionManager = null;
    [SerializeField]
    private GridManager m_GridManager = null;
    private Dictionary<InputControl, PlayableCharacter> m_CharactersList = new Dictionary<InputControl, PlayableCharacter>();
    [SerializeField]
    private List<GameObject> m_CharactersPrefabs = null;
    private int m_CurrentPlayerCount = 0;

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
                if (m_CurrentPlayerCount != m_CharactersPrefabs.Count)
                {
                    //Else we spawn a new PlayableCharacter
                    PlayerInput l_NewPlayer = PlayerInput.Instantiate(m_CharactersPrefabs[m_CurrentPlayerCount]);
                    PlayableCharacter l_NewCharacter = l_NewPlayer.GetComponent<PlayableCharacter>();
                    //Assign it his input (which the one that was just pressed)
                    l_NewCharacter.a_MainControl = p_Context.control;
                    //Give it a ref to the DirectionManager asset
                    l_NewCharacter.a_DirectionManager = m_DirectionManager;
                    //Give it a ref to the GridManager
                    l_NewCharacter.a_GridManager = m_GridManager;
                    //And add it to the list of existing PlayableCharacters
                    m_CharactersList.Add(p_Context.control, l_NewCharacter);

                    //SPAWNING CHARACTER
                    l_NewCharacter.a_GridManager.MakePlayer(5, 5);
                    l_NewCharacter.transform.position = l_NewCharacter.a_GridManager.m_GridOffset * new Vector3(5, 0, 5);


                    //Then we call the function for newly joined players
                    NewPlayerJoined(l_NewCharacter);
                }
                else
                {
                    Debug.Log("Max number of players reached");
                }
            }
        }
    }


    private void NewPlayerJoined(PlayableCharacter p_NewPlayer)
    {
        m_CurrentPlayerCount = m_CurrentPlayerCount + 1;
        Debug.Log(p_NewPlayer + " JOINED !");
    }
}
