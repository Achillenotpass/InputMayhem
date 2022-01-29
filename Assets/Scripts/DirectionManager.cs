using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDirectionManager", menuName = "Managers/NewDirectionManager")]
public class DirectionManager : ScriptableObject
{
    [SerializeField]
    private List<Direction> InputDirection = new List<Direction>();
    public Direction GetDirectionFromInput(int p_InputCount)
    {
        if (p_InputCount >= InputDirection.Count)
        {
            p_InputCount = InputDirection.Count;
        }
        return InputDirection[p_InputCount - 1];
    }

    //This function move all element in array to the next MoveAmount index
    //Try with List<T> to make the function hyper-generic
    public void ArrayMoveAllElement(int MoveAmount, List<Direction> ListToChange)
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
}


public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}
