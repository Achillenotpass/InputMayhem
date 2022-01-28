using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum CaseState
{
    Empty,
    OrbeChaos,
    OrbeCalme,
    Player
}

public class GridManager : MonoBehaviour
{

    private CaseState[,] m_Grid;

    [SerializeField]
    private int m_GridSize = 10;

    [SerializeField]
    private int m_Grid_offset = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitGrid()
    {
        m_Grid = new CaseState[m_GridSize, m_GridSize];
        for (int i = 0; i < m_GridSize; i++)
        {
            for (int j = 0; j < m_GridSize; j++)
            {
                m_Grid[i, j] = CaseState.Empty;
            }
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

                }
            }
        }
    }
}
