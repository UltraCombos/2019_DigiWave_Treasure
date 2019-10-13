using UnityEngine;

public class CityControl : MonoBehaviour
{
    float m_lowPassValue = 0;
    public float LowPassValue { set { m_lowPassValue = value; } }

    [SerializeField]
    int m_lowPassPowLevel = 3;

    [SerializeField]
    float m_lowPassThreshold = 0.7f;

    [Space]
    [SerializeField]
    float m_valueTweenSpeed = 1.5f;

    [SerializeField]
    float m_baseScrollSpeed = 0.5f;

    [SerializeField]
    float m_additionalScrollSpeed = 0.5f;

    [Space]
    [SerializeField, ColorUsage(false, true)]
    Color m_emissionColor;
    [SerializeField,ColorUsage(false,true)]
    Color m_secondaryEmissionColor;

    [Space]
    [SerializeField]
    Material m_mat;

    float m_time = 0;
    float m_targetOffset = 0;
    float m_currentOffset = 0;

    public float Tween(float _current, float _target, float _speed)
    {
        var exp = Mathf.Exp(-_speed * Time.deltaTime);
        return Mathf.Lerp(_target, _current, exp);
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;

        float _passValue = Mathf.Pow(m_lowPassValue, m_lowPassPowLevel) > m_lowPassThreshold ? 1 : 0;

        m_targetOffset += Time.deltaTime * (m_baseScrollSpeed + m_additionalScrollSpeed * _passValue);
        m_currentOffset = Tween(m_currentOffset, m_targetOffset, m_valueTweenSpeed);

        if (m_mat!=null)
        {
            m_mat.SetFloat("_patternOffset", m_currentOffset * -1);
            m_mat.SetFloat("_fakeTime", m_time);
            m_mat.SetColor("_emissionColor", m_emissionColor);
            m_mat.SetColor("_secondaryEmissionColor", m_secondaryEmissionColor);
        }
    }
}
