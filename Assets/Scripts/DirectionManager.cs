using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDirectionManager", menuName = "Managers/NewDirectionManager")]
public class DirectionManager : ScriptableObject
{
    [SerializeField]
    private List<Direction> m_InputDirection = new List<Direction>();
    private List<Direction> m_GameInputDirection = null;
    public Direction GetDirectionFromInput(int p_InputCount)
    {
        if (m_GameInputDirection.Count == 0)
        {
            m_GameInputDirection = new List<Direction>(m_InputDirection);
        }
        if (p_InputCount >= m_GameInputDirection.Count)
        {
            p_InputCount = m_GameInputDirection.Count;
        }
        return m_GameInputDirection[p_InputCount - 1];
    }

    public void ChangeInputs(float p_RotationAngle)
    {
        if (m_GameInputDirection.Count == 0)
        {
            m_GameInputDirection = new List<Direction>(m_InputDirection);
        }
        ArrayMoveAllElement((int)(p_RotationAngle / 90.0f), m_GameInputDirection);
    }

    //This function move all element in array to the next MoveAmount index
    //Try with List<T> to make the function hyper-generic
    private void ArrayMoveAllElement(int MoveAmount, List<Direction> ListToChange)
    {
        List<Direction> l_TempInputDirection = new List<Direction>(ListToChange);
        for (int i = 0; i < l_TempInputDirection.Count; i++)
        {
            int l_TempInt = i + MoveAmount;

            if (l_TempInt >= l_TempInputDirection.Count)
            {
                l_TempInt = l_TempInt % ListToChange.Count;
            }

            ListToChange[l_TempInt] = l_TempInputDirection[i];
        }
    }

    public void Resetgame()
    {
        m_GameInputDirection = new List<Direction>(m_InputDirection);
    }
}


public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}
