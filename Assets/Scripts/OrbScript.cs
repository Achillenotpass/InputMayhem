using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrbScript : MonoBehaviour
{
    private float m_Timer = 0;

    [SerializeField]
    private CaseState m_OrbType;

    [SerializeField]
    private GridManager m_GridManager;

    [SerializeField]
    private TimeManager m_TimeManager;

    [SerializeField]
    private UnityEvent m_CollectedEvent = null;
    public int m_X, m_Y;

    public float m_Lifespan;

    private bool m_IsDying = false;

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

        if (m_Timer >= m_Lifespan && !m_IsDying)
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
        m_CollectedEvent.Invoke();
        Death();
    }

    void Death()
    {
        m_IsDying = true;
        //temporaire
        m_GridManager.MakeEmpty(m_X, m_Y);
        
        m_GridManager.MakeOrbRandom(m_OrbType);

        transform.position = new Vector3(1000.0f, 1000.0f, 1000.0f);
        Invoke(nameof(DestroyObject), 2.0f);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
