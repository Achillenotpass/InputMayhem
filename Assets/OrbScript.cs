using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : MonoBehaviour
{
    private float m_Timer = 0;

    [SerializeField]
    private CaseState m_OrbType;

    [SerializeField]
    private GridManager m_GridManager;

    [SerializeField]
    private TimeManager m_TimeManager;

    public int m_X, m_Y;

    public float m_Lifespan;

    // Start is called before the first frame update
    void Start()
    {
        m_GridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        m_TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Timer += Time.deltaTime;

        if (m_Timer >= m_Lifespan)
        {
            Death();
        }
    }

    public void Taken()
    {
        if (m_OrbType == CaseState.OrbeCalme)
        {
            m_TimeManager.addCalmOrb();
        } else
        {
            m_TimeManager.addChaosOrb();
        }
        Death();
    }

    void Death()
    {
        //temporaire
        m_GridManager.MakeEmpty(m_X, m_Y);
        
        m_GridManager.MakeOrbRandom(m_OrbType);
        Destroy(gameObject);
    }
}
