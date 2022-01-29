using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    private CharactersManager m_CharactersManager = null;
    private float m_Timer;

    private int m_CalmOrbs, m_ChaosOrbs = 0;

    private bool ChaosGrowing = false;

    [SerializeField]
    private Text m_TimerText;

    [SerializeField]
    private Slider m_slider;

    [SerializeField]
    private Vector2 m_GaugeMovingValue;

    [SerializeField]
    private float m_GaugeValueIncrease = 10;

    [SerializeField]
    private int m_orbsDifference = 2;

    [Range(0, 100)]
    private float m_GaugeValue = 50;

    //placeholder
    public bool addChaos = false;

    public bool addCalme = false;

    public float addValue = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_CharactersManager.a_CurrentGameState)
        {
            case CharactersManager.GameState.WaitingForPlayers:
                break;
            case CharactersManager.GameState.Playing:
                m_Timer += Time.deltaTime;
                m_TimerText.text = m_Timer.ToString("F2");

                m_GaugeValue += (m_GaugeMovingValue.x / m_GaugeMovingValue.y * Time.deltaTime);

                if (m_GaugeMovingValue.x > 0)
                {
                    m_GaugeMovingValue.x += (m_GaugeValueIncrease / m_GaugeMovingValue.y * Time.deltaTime);
                }
                else
                {
                    m_GaugeMovingValue.x -= (m_GaugeValueIncrease / m_GaugeMovingValue.y * Time.deltaTime);
                }

                m_slider.value = m_GaugeValue / 100f;

                if (m_GaugeValue >= 100.0f || m_GaugeValue <= 0.0f)
                {
                    LoseGame();
                }
                break;
            case CharactersManager.GameState.GameLost:
                break;
            case CharactersManager.GameState.GameWon:
            default:
                break;
        }
    }

    public void addToGaugeValue(float p_value)
    {
        if (p_value > 0)
        {
            m_CalmOrbs += 1;
        } else
        {
            m_ChaosOrbs += 1;
        }
        m_GaugeValue += p_value;
        if (m_GaugeValue > 100)
        {
            m_GaugeValue = 100;
            LoseGame();
        }
        else if (m_GaugeValue < 0)
        {
            m_GaugeValue = 0;
            LoseGame();
        }

        if (ChaosGrowing && (m_CalmOrbs - m_ChaosOrbs >= m_orbsDifference)) 
        {
            ChaosGrowing = false;
            m_GaugeMovingValue.x = -m_GaugeMovingValue.x;
        }

        if (!ChaosGrowing && (m_ChaosOrbs - m_CalmOrbs >= m_orbsDifference))
        {
            ChaosGrowing = true;
            m_GaugeMovingValue.x = -m_GaugeMovingValue.x;
        }
    }

    public void addCalmOrb()
    {
        addToGaugeValue(addValue);
    }

    public void addChaosOrb()
    {
        addToGaugeValue(-addValue);
    }

    private void LoseGame()
    {
        m_CharactersManager.a_CurrentGameState = CharactersManager.GameState.GameLost;
    }

    //placeholder
    void OnValidate()
    {
        if (addCalme)
        {
            addToGaugeValue(addValue);
            addCalme = false;
        }

        if (addChaos)
        {
            addToGaugeValue(-addValue);
            addChaos = false;
        }
    }

}
