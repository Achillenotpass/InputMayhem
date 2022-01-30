using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayableCharacter : MonoBehaviour
{
    //This is the character's name that will be used for, like, uh, I don't know
    [SerializeField]
    private string m_CharacterName = string.Empty;
    [SerializeField]
    private List<AudioClip> m_JumpSound = null;

    private DirectionManager m_DirectionManager = null;
    public DirectionManager a_DirectionManager { set { m_DirectionManager = value; } get { return m_DirectionManager; } }
    private GridManager m_GridManager = null;
    public GridManager a_GridManager { set { m_GridManager = value; } get { return m_GridManager; } }
    private Animator m_Animator = null;
    private AudioSource m_AudioSource = null;
    //This is the main input of that character
    //The only keyboard key that will act on him
    private InputControl m_MainControl = null;
    public InputControl a_MainControl { set { m_MainControl = value; } get { return m_MainControl; } }
    private float m_MaxTimeBetweenInputs = 0.2f;
    private int m_CurrentInputCount = 0;
    private bool m_IsMoving = false;

    private List<Coroutine> m_RunningCoroutines = new List<Coroutine>();

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_AudioSource = GetComponentInChildren<AudioSource>();
    }

    public void InputPressed()
    {
        if (!m_IsMoving)
        {
            foreach (Coroutine Coroutine in m_RunningCoroutines)
            {
                StopCoroutine(Coroutine);
            }
            m_CurrentInputCount = m_CurrentInputCount + 1;
            m_RunningCoroutines.Add(StartCoroutine(CheckInputDelay(m_MaxTimeBetweenInputs)));
        }
    }

    private IEnumerator CheckInputDelay(float p_MaxTimeBetweenInputs)
    {
        yield return new WaitForSeconds(p_MaxTimeBetweenInputs);
        Move(m_CurrentInputCount);
    }

    void UpdatePlayerOnGrid(int p_X, int p_Y)
    {
        m_GridManager.MakeEmpty((int)(transform.position.x / m_GridManager.m_GridOffset), (int)(transform.position.z / m_GridManager.m_GridOffset));
        int l_nextCaseX = (int)(transform.position.x / m_GridManager.m_GridOffset) + p_X;
        int l_nextCaseY = (int)(transform.position.z / m_GridManager.m_GridOffset) + p_Y;
        CaseState l_NextCase = m_GridManager.getCaseState(l_nextCaseX, l_nextCaseY);
        if (l_NextCase == CaseState.OrbeCalme || l_NextCase == CaseState.OrbeChaos)
        {
            OrbScript[] l_Orbs = GameObject.FindObjectsOfType<OrbScript>();
            foreach (OrbScript l_OrbScript in l_Orbs)
            {
                if (l_OrbScript.m_X == l_nextCaseX && l_OrbScript.m_Y == l_nextCaseY)
                {
                    l_OrbScript.Taken();
                    break;
                }
            }
        }
        m_GridManager.MakePlayer(l_nextCaseX, l_nextCaseY);
    }

    bool caseNotPlayer(int p_X, int p_Y)
    {
        int l_X = (int)(transform.position.x / m_GridManager.m_GridOffset) + p_X;
        int l_Y = (int)(transform.position.z / m_GridManager.m_GridOffset) + p_Y;
        if (m_GridManager.getCaseState(l_X, l_Y) == CaseState.Player)
        {
            return false;
        }
        return true;
    }

    private void Move(int p_InputCount)
    {
        Direction l_MoveDirection = m_DirectionManager.GetDirectionFromInput(p_InputCount);
        m_CurrentInputCount = 0;
        switch (l_MoveDirection)
        {
            case Direction.Up:
                Debug.Log((int)(transform.position.z / m_GridManager.m_GridOffset));
                if ((int)(transform.position.z / m_GridManager.m_GridOffset) < m_GridManager.m_GridSize - 1)
                {
                    if (caseNotPlayer(Vector3Int.forward.x, Vector3Int.forward.z))
                    {
                        StartCoroutine(MovementAnimation(Vector3Int.forward));
                        break;
                    }
                }
                break;
            case Direction.Down:
                Debug.Log((int)(transform.position.z / m_GridManager.m_GridOffset));
                if ((int)(transform.position.z / m_GridManager.m_GridOffset) > 0)
                {
                    if (caseNotPlayer(Vector3Int.back.x, Vector3Int.back.z))
                    {
                        StartCoroutine(MovementAnimation(Vector3Int.back));
                        break;
                    } 
                }
                break;
            case Direction.Left:
                if ((int)(transform.position.x / m_GridManager.m_GridOffset) > 0)
                {
                    transform.GetChild(0).GetChild(0).localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    if (caseNotPlayer(Vector3Int.left.x, Vector3Int.left.z))
                    {
                        StartCoroutine(MovementAnimation(Vector3Int.left));
                        break;
                    }
                }
                break;
            case Direction.Right:
                if ((int)(transform.position.x / m_GridManager.m_GridOffset) < m_GridManager.m_GridSize - 1)
                {
                    transform.GetChild(0).GetChild(0).localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    if (caseNotPlayer(Vector3Int.right.x, Vector3Int.right.z))
                    {
                        StartCoroutine(MovementAnimation(Vector3Int.right));
                        break;
                    }
                } 
                break;
            default:
                break;
        }
    }
    private IEnumerator MovementAnimation(Vector3Int p_Direction)
    {
        m_IsMoving = true;

        float l_AnimationTime = 0.3f;
        float l_Timer = 0.0f;
        m_Animator.SetTrigger("Jump");
        m_AudioSource.pitch = Random.Range(0.9f, 1.1f);
        m_AudioSource.PlayOneShot(m_JumpSound[Random.Range(0, m_JumpSound.Count)]);
        yield return new WaitForSeconds(0.15f);
        UpdatePlayerOnGrid(p_Direction.x, p_Direction.z);
        
        while (l_Timer < l_AnimationTime)
        {
            transform.Translate((Vector3)p_Direction * m_GridManager.m_GridOffset * Time.deltaTime / l_AnimationTime);
            l_Timer = l_Timer + Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(Mathf.Round(transform.position.x), 0.0f, Mathf.Round(transform.position.z));
        m_IsMoving = false;
    }

}
