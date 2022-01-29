using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

public class CharactersManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_GameOverUI = null;
    private GameState m_CurrentGameState = GameState.WaitingForPlayers;
    public GameState a_CurrentGameState 
    { set 
        { 
            m_CurrentGameState = value;
            if (m_CurrentGameState != GameState.Playing)
            {
                if (m_CurrentGameState == GameState.GameLost)
                {
                    m_GameOverUI.SetActive(true);
                }
                m_GridManager.StopGame();
            }
            else
            {
                m_GameOverUI.SetActive(false);
            }
        } 
        get { return m_CurrentGameState; } 
    }
    [SerializeField]
    private GameManager m_GameManager = null;
    [SerializeField]
    private DirectionManager m_DirectionManager = null;
    [SerializeField]
    private GridManager m_GridManager = null;
    private Dictionary<InputAction, PlayableCharacter> m_CharactersList = new Dictionary<InputAction, PlayableCharacter>();
    [SerializeField]
    private List<GameObject> m_CharactersPrefabs = null;
    private int m_CurrentPlayerCount = 0;


    //Called when any binded key is pressed
    //public void CreateCharacter(InputAction.CallbackContext p_Context)
    //{
    //    if (p_Context.started)
    //    {
    //        //If a PlayableCharacter with that input already exists
    //        if (m_CharactersList.TryGetValue(p_Context.control, out PlayableCharacter l_PlayableCharacter))
    //        {
    //            //We tell the PlayableCharacter his input has been pressed
    //            //l_PlayableCharacter.InputPressed();
    //        }
    //        else if(m_CurrentGameState == GameState.WaitingForPlayers)
    //        {
    //            if (m_CurrentPlayerCount != m_CharactersPrefabs.Count)
    //            {
    //                //Else we spawn a new PlayableCharacter
    //                PlayerInput l_NewPlayer = PlayerInput.Instantiate(m_CharactersPrefabs[m_CurrentPlayerCount]);
                    
    //                PlayableCharacter l_NewCharacter = l_NewPlayer.GetComponent<PlayableCharacter>();
    //                //Assign it his input (which the one that was just pressed)
    //                l_NewCharacter.a_MainControl = p_Context.control;
    //                //Give it a ref to the DirectionManager asset
    //                l_NewCharacter.a_DirectionManager = m_DirectionManager;
    //                //Give it a ref to the GridManager
    //                l_NewCharacter.a_GridManager = m_GridManager;
    //                //And add it to the list of existing PlayableCharacters
    //                m_CharactersList.Add(p_Context.control, l_NewCharacter);

    //                //SPAWNING CHARACTER
    //                bool l_Spawning = true;
    //                int l_WatchDog = 0;
    //                while (l_Spawning)
    //                {
    //                    Vector2Int l_GridPosition = new Vector2Int(Random.Range(0, m_GridManager.a_Grid.GetLength(0)), Random.Range(0, m_GridManager.a_Grid.GetLength(1)));
    //                    if (m_GridManager.a_Grid[l_GridPosition.x, l_GridPosition.y] == CaseState.Empty)
    //                    {
    //                        m_GridManager.MakePlayer(l_GridPosition.x, l_GridPosition.y);
    //                        l_NewCharacter.transform.position = m_GridManager.m_GridOffset * new Vector3(l_GridPosition.x, 0, l_GridPosition.y);
    //                        l_Spawning = false;
    //                    }
    //                    l_WatchDog = l_WatchDog + 1;
    //                    if (l_WatchDog >= 100)
    //                    {
    //                        l_Spawning = false;
    //                    }
    //                }


    //                //Then we call the function for newly joined players
    //                NewPlayerJoined(l_NewCharacter);
    //            }
    //            else
    //            {
    //                Debug.Log("Max number of players reached");
    //            }
    //        }
    //    }
    //}

    public void PlayerInputPressed(InputAction.CallbackContext p_Context)
    {
        if (p_Context.started)
        {
            if (m_CharactersList.TryGetValue(p_Context.action, out PlayableCharacter l_PlayableCharacter))
            {
                l_PlayableCharacter.InputPressed();
            }
            else if (m_CurrentPlayerCount != m_CharactersPrefabs.Count)
            {
                //Else we spawn a new PlayableCharacter
                GameObject l_NewPlayer = Instantiate(m_CharactersPrefabs[m_CurrentPlayerCount]);
                PlayableCharacter l_NewCharacter = l_NewPlayer.GetComponent<PlayableCharacter>();

                //Assign it his input (which the one that was just pressed)
                l_NewCharacter.a_MainControl = p_Context.control;
                //Give it a ref to the DirectionManager asset
                l_NewCharacter.a_DirectionManager = m_DirectionManager;
                //Give it a ref to the GridManager
                l_NewCharacter.a_GridManager = m_GridManager;
                //And add it to the list of existing PlayableCharacters
                m_CharactersList.Add(p_Context.action, l_NewCharacter);

                //SPAWNING CHARACTER
                bool l_Spawning = true;
                int l_WatchDog = 0;
                while (l_Spawning)
                {
                    Vector2Int l_GridPosition = new Vector2Int(Random.Range(0, m_GridManager.a_Grid.GetLength(0)), Random.Range(0, m_GridManager.a_Grid.GetLength(1)));
                    if (m_GridManager.a_Grid[l_GridPosition.x, l_GridPosition.y] == CaseState.Empty)
                    {
                        m_GridManager.MakePlayer(l_GridPosition.x, l_GridPosition.y);
                        l_NewCharacter.transform.position = m_GridManager.m_GridOffset * new Vector3(l_GridPosition.x, 0, l_GridPosition.y);
                        l_Spawning = false;
                    }
                    l_WatchDog = l_WatchDog + 1;
                    if (l_WatchDog >= 100)
                    {
                        l_Spawning = false;
                    }
                }
            }
        }
    }


    public void StartGame(InputAction.CallbackContext p_Context)
    {
        if (p_Context.started && m_CurrentGameState == GameState.WaitingForPlayers)
        {
            Debug.Log("START PLAYING DUDES! ");
            m_CurrentGameState = GameState.Playing;
            m_GridManager.StartGame();
        }
        else if (p_Context.started && m_CurrentGameState == GameState.GameLost)
        {
            SceneManager.LoadScene("TestTheo");
        }
    }

    private void NewPlayerJoined(PlayableCharacter p_NewPlayer)
    {
        m_CurrentPlayerCount = m_CurrentPlayerCount + 1;
        Debug.Log(p_NewPlayer + " JOINED !");
    }
    public enum GameState
    {
        WaitingForPlayers,
        Playing,
        GameLost,
        GameWon,
    }
}
