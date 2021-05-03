
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public RectTransform m_outline;
    public RectTransform m_progressbar;
    [Space]

    public Button m_playbutton;
    public Button m_taptostop;
    public Button m_replaybutton;
    [Space]
    public TextMeshProUGUI m_percentagetocomplete;
    public TextMeshProUGUI m_failedtext;
    public TextMeshProUGUI m_barpercentage;

    [Space]
    public RectTransform m_imdictor;
    [Space]
    public TextMeshProUGUI m_instruction;
    [Space]
    public float m_speed;


    public bool m_isgamestarted;

    private Image m_progressbarimage;

    private int m_randmonpercentage;
    private float m_fillamount;


    private float m_width;

    private void Start()
    {
        m_progressbarimage = m_progressbar.GetComponent<Image>();


        m_width = m_outline.sizeDelta.x;

        _Reset();
        _AddButtonListeners();
    }

    void _AddButtonListeners()
    {
        m_playbutton.onClick.AddListener(_PlayGame);
        m_taptostop.onClick.AddListener(_TapToStopGame);
        m_replaybutton.onClick.AddListener(_ResplayGame);
    }

    private void _ResplayGame()
    {
        _Reset();
        m_replaybutton.gameObject.SetActive(false);
        m_playbutton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (m_isgamestarted)
        {
            _StartFillingUpProcess();
        }
    }


    void _StartFillingUpProcess()
    {
        m_fillamount += Time.deltaTime * m_speed;
        m_progressbarimage.fillAmount = m_fillamount;
    }



    void _CountThePercentageAtStoped()
    {
        Debug.Log(m_fillamount);
        float m_progress = m_fillamount * 100f;
        Debug.Log(m_progress);

        m_barpercentage.text = ((int)m_progress).ToString()+"%";
        float m_pos = (m_progress)*(m_width) / (100f);
        Debug.Log(m_pos);
        m_imdictor.gameObject.SetActive(true);
        m_imdictor.anchoredPosition = new Vector2(m_pos, m_imdictor.anchoredPosition.y);

        int m_missedbypoints = m_randmonpercentage - (int)m_progress;
        m_missedbypoints = Mathf.Abs(m_missedbypoints);

        m_instruction.gameObject.SetActive(false);
        m_failedtext.gameObject.SetActive(true);


        if (m_missedbypoints>5)
        {
            m_failedtext.text = "YOU MISSED BY " + m_missedbypoints.ToString();
        }
        else if (m_missedbypoints >=3)
        {
            m_failedtext.text = "VERY CLOSE";
        }
        else if (m_missedbypoints >0)
        {
            m_failedtext.text ="ALMOST";
        }



        Debug.Log(m_missedbypoints);

    }

    void _SetRandomPercentageValue()
    {
        m_randmonpercentage = Random.Range(0,100);
    }


    void _Reset()
    {
        m_fillamount = 0f;
        m_progressbarimage.fillAmount = m_fillamount;
        m_failedtext.gameObject.SetActive(false);
        m_instruction.gameObject.SetActive(true);
        m_percentagetocomplete.gameObject.SetActive(false);
        m_imdictor.gameObject.SetActive(false);
        _SetRandomPercentageValue();
    }


    void _PlayGame()
    {
        m_playbutton.gameObject.SetActive(false);
        StartCoroutine(_WaitAndStartGame());
    }

    IEnumerator _WaitAndStartGame()
    {
        m_percentagetocomplete.text = m_randmonpercentage.ToString() + "%";
        m_percentagetocomplete.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        m_taptostop.gameObject.SetActive(true);
        m_isgamestarted = true;
    }


    void _TapToStopGame()
    {
        m_isgamestarted = false;
        m_taptostop.gameObject.SetActive(false);
        m_replaybutton.gameObject.SetActive(true);
        _CountThePercentageAtStoped();
    }
}
