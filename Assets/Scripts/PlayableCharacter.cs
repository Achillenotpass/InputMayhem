using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayableCharacter : MonoBehaviour
{
    //This is the character's name that will be used for, like, uh, I don't know
    [SerializeField]
    private string m_CharacterName = string.Empty;
    private DirectionManager m_DirectionManager = null;
    public DirectionManager a_DirectionManager { set { m_DirectionManager = value; } get { return m_DirectionManager; } }
    //This is the main input of that character
    //The only keyboard key that will act on him
    private InputControl m_MainControl = null;
    public InputControl a_MainControl { set { m_MainControl = value; } get { return m_MainControl; } }
    private float m_MaxTimeBetweenInputs = 0.2f;
    private int m_CurrentInputCount = 0;

    private List<Coroutine> m_RunningCoroutines = new List<Coroutine>();
  

    public void InputPressed()
    {
        foreach (Coroutine Coroutine in m_RunningCoroutines)
        {
            StopCoroutine(Coroutine);
        }
        m_CurrentInputCount = m_CurrentInputCount + 1;
        m_RunningCoroutines.Add(StartCoroutine(CheckInputDelay(m_MaxTimeBetweenInputs)));
    }

    private IEnumerator CheckInputDelay(float p_MaxTimeBetweenInputs)
    {
        yield return new WaitForSeconds(p_MaxTimeBetweenInputs);
        Move(m_CurrentInputCount);
    }

    private void Move(int p_InputCount)
    {
        Direction l_MoveDirection = m_DirectionManager.GetDirectionFromInput(p_InputCount);
        m_CurrentInputCount = 0;
        switch (l_MoveDirection)
        {
            case Direction.Up:
                transform.Translate(Vector3.up);
                break;
            case Direction.Down:
                transform.Translate(Vector3.down);
                break;
            case Direction.Left:
                transform.Translate(Vector3.left);
                break;
            case Direction.Right:
                transform.Translate(Vector3.right);
                break;
            default:
                break;
        }
        Debug.Log(l_MoveDirection);
    }

}
