using System.Collections.Generic;
using UnityEditor.VFX;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using System.Collections;

public class VFXHolder : MonoBehaviour
{
    [SerializeField]
    VisualEffect[] m_visualEffects;

    float m_bypass;
    public float bypass { set { m_bypass = value; } get { return m_bypass; } }

    float m_lowpass;
    public float lowpass { set { m_lowpass = value; } get { return m_lowpass; } }

    float m_bandpass;
    public float bandpass { set { m_bandpass = value; } get { return m_bandpass; } }

    float m_highpass;
    public float highpass { set { m_highpass = value; } get { return m_highpass; } }

    // Update is called once per frame
    void Update()
    {
        foreach (VisualEffect _v in m_visualEffects)
        {
            if (_v.enabled)
            {
                _v.SetFloat("ByPass", m_bypass);
                _v.SetFloat("LowPass", lowpass);
                _v.SetFloat("BandPass", bandpass);
                _v.SetFloat("HighPass", highpass);
            }
        }
    }
}
