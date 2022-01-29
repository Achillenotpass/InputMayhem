using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum CaseState
{
    Empty,
    OrbeChaos,
    OrbeCalme,
    Player
}

public class GridManager : MonoBehaviour
{

    private CaseState[,] m_Grid;
    public CaseState[,] a_Grid { get { return m_Grid; } }

    private GameObject[,] m_ObjGrid;

    [SerializeField]
    private int m_GridSize = 10;

    public int m_GridOffset = 1;

    [SerializeField]
    private GameObject m_GridCase;

    [SerializeField]
    private Material m_Mat_Player;

    [SerializeField]
    private Material m_Mat_Chaos;

    [SerializeField]
    private Material m_Mat_Calme;

    [SerializeField]
    private Material m_Mat_Empty;

    [SerializeField]
    private GameObject m_ChaosOrb;

    [SerializeField]
    private GameObject m_CalmOrb;

    [SerializeField]
    private float m_OrbLifeSpan = 3;

    [SerializeField]
    private float m_OrbLifeSpanRand = 1;

    //placeholder

    public bool addPlayer = false;

    public bool CaseFree = false;

    public int X, Y = 5;



    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitGrid()
    {
        m_Grid = new CaseState[m_GridSize, m_GridSize];
        m_ObjGrid = new GameObject[m_GridSize, m_GridSize];
        for (int i = 0; i < m_GridSize; i++)
        {
            for (int j = 0; j < m_GridSize; j++)
            {
                m_Grid[i, j] = CaseState.Empty;
                m_ObjGrid[i, j] = Instantiate(m_GridCase, new Vector3(i * m_GridOffset, 0, j * m_GridOffset), Quaternion.identity, transform);
            }
        }
    }
    public void StartGame()
    {
        for (int k = 0; k < 6; k++)
        {
            if (k < 3)
            {
                MakeOrbRandom(CaseState.OrbeCalme);
            }
            else
            {
                MakeOrbRandom(CaseState.OrbeChaos);
            }
        }
    }
    public void StopGame()
    {
        foreach (OrbScript l_Orb in FindObjectsOfType<OrbScript>())
        {
            MakeEmpty((int)l_Orb.transform.position.x / m_GridOffset, (int)l_Orb.transform.position.z / m_GridOffset);
            Destroy(l_Orb.gameObject);
        }
    }

    void UpdateGrid()
    {
        for (int i = 0; i < m_GridSize; i++)
        {
            for (int j = 0; j < m_GridSize; j++)
            {
                if (m_Grid[i, j] == CaseState.Empty)
                {
                    m_ObjGrid[i, j].gameObject.GetComponent<Renderer>().material = m_Mat_Empty;
                }
                else if (m_Grid[i, j] == CaseState.Player)
                {
                    m_ObjGrid[i, j].gameObject.GetComponent<Renderer>().material = m_Mat_Player;
                }
                else if (m_Grid[i, j] == CaseState.OrbeCalme)
                {
                    m_ObjGrid[i, j].gameObject.GetComponent<Renderer>().material = m_Mat_Calme;
                }
                else if (m_Grid[i, j] == CaseState.OrbeChaos)
                {
                    m_ObjGrid[i, j].gameObject.GetComponent<Renderer>().material = m_Mat_Chaos;
                }
            }
        }
    }

    public void MakePlayer(int p_PlayerX, int p_PlayerY)
    {
        m_Grid[p_PlayerX, p_PlayerY] = CaseState.Player;
        m_ObjGrid[p_PlayerX, p_PlayerY].gameObject.GetComponent<Renderer>().material = m_Mat_Player;
    }

    public void MakeOrbWithCoord(CaseState p_OrbType, int p_X, int p_Y)
    {
        m_Grid[p_X, p_Y] = p_OrbType;
        GameObject l_NewOrb;
        if (p_OrbType == CaseState.OrbeCalme)
        {
            l_NewOrb = m_CalmOrb;
            m_ObjGrid[p_X, p_Y].gameObject.GetComponent<Renderer>().material = m_Mat_Calme;
        } else
        {
            l_NewOrb = m_ChaosOrb;
            m_ObjGrid[p_X, p_Y].gameObject.GetComponent<Renderer>().material = m_Mat_Chaos;
        }
        l_NewOrb = Instantiate(l_NewOrb, new Vector3(p_X * m_GridOffset, 2, p_Y * m_GridOffset), Quaternion.identity, transform);
        l_NewOrb.GetComponent<OrbScript>().m_X = p_X;
        l_NewOrb.GetComponent<OrbScript>().m_Y = p_Y;
        l_NewOrb.GetComponent<OrbScript>().m_Lifespan = m_OrbLifeSpan + Random.Range(-m_OrbLifeSpanRand, m_OrbLifeSpanRand);
    }

    public void MakeOrbRandom(CaseState p_OrbType)
    {
        List<Vector2> l_Liste = GetAllFreeCases();
        Vector2 l_Coord = l_Liste[Random.Range(0, l_Liste.Count - 1)];
        MakeOrbWithCoord(p_OrbType, (int)l_Coord.x, (int)l_Coord.y);
    }

    public void MakeEmpty(int p_X, int p_Y)
    {
        m_Grid[p_X, p_Y] = CaseState.Empty;
        m_ObjGrid[p_X, p_Y].gameObject.GetComponent<Renderer>().material = m_Mat_Empty;
    }

    public CaseState getCaseState(int p_X, int p_Y)
    {
        return m_Grid[p_X, p_Y];
    }

    private bool CaseIsFree(int p_X, int p_Y)
    {
        bool l_result = true;
        if (m_Grid[p_X, p_Y] == CaseState.Player || m_Grid[p_X, p_Y] == CaseState.OrbeCalme || m_Grid[p_X, p_Y] == CaseState.OrbeChaos)
        {
            l_result = false;
        }
        if (p_X < m_GridSize - 1)
        {
            if (m_Grid[p_X + 1, p_Y] == CaseState.Player || m_Grid[p_X + 1, p_Y] == CaseState.OrbeCalme || m_Grid[p_X + 1, p_Y] == CaseState.OrbeChaos)
            {
                l_result = false;
            }
        }
        if (p_X > 0)
        {
            if (m_Grid[p_X - 1, p_Y] == CaseState.Player || m_Grid[p_X - 1, p_Y] == CaseState.OrbeCalme || m_Grid[p_X - 1, p_Y] == CaseState.OrbeChaos)
            {
                l_result = false;
            }
        }
        if (p_Y < m_GridSize - 1)
        {
            if (m_Grid[p_X, p_Y + 1] == CaseState.Player || m_Grid[p_X, p_Y + 1] == CaseState.OrbeCalme || m_Grid[p_X, p_Y + 1] == CaseState.OrbeChaos)
            {
                l_result = false;
            }
        }
        if (p_Y > 0)
        {
            if (m_Grid[p_X, p_Y - 1] == CaseState.Player || m_Grid[p_X, p_Y - 1] == CaseState.OrbeCalme || m_Grid[p_X, p_Y - 1] == CaseState.OrbeChaos)
            {
                l_result = false;
            }
        }
        return l_result;
    }

    public List<Vector2> GetAllFreeCases()
    {
        List<Vector2> l_liste = new List<Vector2>();
        for (int i = 0; i < m_GridSize; i++)
        {
            for (int j = 0; j < m_GridSize; j++)
            {
                if (CaseIsFree(i, j))
                {
                    l_liste.Add(new Vector2(i, j));
                }
            }
        }
        return l_liste;
    }

    //placeholder
    public void tamerInput(InputAction.CallbackContext p_context)
    {
        Debug.Log("tamer");
    }

    void OnValidate()
    {
        if (addPlayer)
        {
            addPlayer = false;
            MakePlayer(X, Y);
        }

        if (CaseFree)
        {
            CaseFree = false;
            Debug.Log(CaseIsFree(X, Y));
        }
    }
}
