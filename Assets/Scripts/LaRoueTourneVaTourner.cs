using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class LaRoueTourneVaTourner : MonoBehaviour
{

    //me jugez pas, le code va etre degueulasse mais on est dans la derniere ligne droite et j'ai pas la foi de faire des trucs compliqués

    [SerializeField]
    private float m_RotationSpeed = 1f;

    [SerializeField]
    private float m_CountdownSpeed;

    [SerializeField]
    private Text m_CountdownText;

    [SerializeField]
    private float m_Fade_Duration = 0.7f;

    private float m_Rotation;

    private float m_CountdownValue = 3f;

    private float m_Timer = 3;

    private bool m_Counting = false;
    [SerializeField]
    private UnityEvent m_TurnEvent = null;

    // Start is called before the first frame update
    void Start()
    {
        m_CountdownText.text = "" + 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Counting)
        {
            m_Timer -= Time.deltaTime;
            if (m_CountdownValue == 0 && m_Timer <= 0)
            {
                m_CountdownText.DOFade(1f, 0f);
                m_Counting = false;
                m_CountdownValue = 3;
                m_Timer = 3;
                m_CountdownText.text = "" + 0;
                m_TurnEvent.Invoke();
                transform.DORotate(transform.rotation.eulerAngles - new Vector3(0, 0, -m_Rotation), m_RotationSpeed, RotateMode.Fast);

                //ICI ROTATION ET CHANGEMENT INPUT
                FindObjectOfType<CharactersManager>().a_DirectionManager.ChangeInputs(m_Rotation);
            }
            if (m_Timer < m_CountdownValue && m_CountdownValue > 0)
            {
                Debug.Log(m_CountdownValue);
                m_CountdownText.text = "" + m_CountdownValue;
                m_CountdownValue -= 1;
                m_CountdownText.DOFade(1f, 0f);
                m_CountdownText.DOFade(0f, m_Fade_Duration);
            }
        }
    }

    //APPELER CA POUR CHANGER INPUT
    public void LaRoueTourne(float p_Rotation)
    {
        m_Counting = true;
        m_Rotation = p_Rotation;
    }

}
