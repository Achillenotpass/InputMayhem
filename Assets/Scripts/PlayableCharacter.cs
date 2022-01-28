using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayableCharacter : MonoBehaviour
{
    //This is the main input of that character
    //The only keyboard key that will act on him
    private InputControl m_MainControl = null;
    public InputControl MainControl { set { m_MainControl = value; } get { return m_MainControl; } }
  

    public void InputPressed()
    {
        Destroy(this.gameObject);
    }

}
