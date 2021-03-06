using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    private float m_DirectionChangeChance = 5.0f;
    [SerializeField]
    private CharactersManager m_CharactersManager = null;
    //timer global, permet de stocker le score
    private float m_Timer;

    //on stocke le nombre d'orbes prises
    private int m_CalmOrbs, m_ChaosOrbs = 0;

    //permet de savoir si on doit additioner ou soustraire a la valeur de la jauge
    private bool ChaosGrowing = false;

    [SerializeField]
    private Text m_TimerText;

    [SerializeField]
    private Slider m_slider;

    //la premi?re valeur repr?sente l'augmentation, la deuxi?me en combien de temps cette augmentation prend place
    [SerializeField]
    private Vector2 m_GaugeMovingValue;

    //la valeur d'augmentation augmente elle meme avec le temps
    [SerializeField]
    private float m_GaugeValueIncrease = 10;

    //diff?rence entre les orbes collect?es avant que la jauge change de sens
    [SerializeField]
    private int m_orbsDifference = 2;

    [SerializeField]
    private Light m_DirectionalLight;

    [Range(0, 100)]
    private float m_GaugeValue = 50;

    private Gradient m_Gradient;

    [SerializeField]
    private Color m_ChaosColor;

    [SerializeField]
    private Color m_CalmColor;

    [SerializeField]
    private float m_LerpValue = 0.5f;

    private GradientColorKey[] m_ColorKey;

    private GradientAlphaKey[] m_AlphaKey;

    //placeholder

    public bool addChaos = false;

    public bool addCalme = false;

    public float addValue = 30;

    // Start is called before the first frame update
    void Start()
    {
        m_CharactersManager.a_DirectionManager.Resetgame();

        StartCoroutine(ChangeInputDirections());

        m_Gradient = new Gradient();
        m_ColorKey = new GradientColorKey[2];
        m_ColorKey[0].color = m_ChaosColor;
        m_ColorKey[0].time = 0f;
        m_ColorKey[1].color = m_CalmColor;
        m_ColorKey[1].time = 1f;

        m_AlphaKey = new GradientAlphaKey[2];
        m_AlphaKey[0].alpha = 1.0f;
        m_AlphaKey[0].time = 1.0f;
        m_AlphaKey[1].alpha = 1.0f;
        m_AlphaKey[1].time = 1.0f;

        m_Gradient.SetKeys(m_ColorKey, m_AlphaKey);

        m_DirectionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        m_DirectionalLight.color = m_Gradient.Evaluate(m_GaugeValue / 100f);

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
                if (m_slider.value != m_GaugeValue / 100f)
                {
                    m_slider.value = Mathf.Lerp(m_slider.value, m_GaugeValue / 100f, m_LerpValue);
                }
                //m_slider.value = m_GaugeValue / 100f;

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

    private IEnumerator ChangeInputDirections()
    {
        while (true)
        {
            if (m_CharactersManager.a_CurrentGameState == CharactersManager.GameState.Playing)
            {
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(10.0f);
        while (true)
        {
            if (m_CharactersManager.a_CurrentGameState == CharactersManager.GameState.GameLost)
            {
                break;
            }
            yield return new WaitForSeconds(1.0f);
            if (Random.Range(0.0f, 100.0f) <= m_DirectionChangeChance)
            {
                int l_RandomAngle = Random.Range(1, 4) * 90;
                FindObjectOfType<LaRoueTourneVaTourner>().LaRoueTourne((float)l_RandomAngle);
                yield return new WaitForSeconds(5.0f);
            }

            yield return null;
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

        /*
        m_ColorKey[0].color = m_ChaosColor;

        m_ColorKey[1].color = m_CalmColor;

        m_Gradient.SetKeys(m_ColorKey, m_AlphaKey);
        */
    }

}
