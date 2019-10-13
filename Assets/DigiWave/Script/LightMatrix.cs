using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class LightMatrix : MonoBehaviour
{

    class LightData
    {
        float m_delayTime = 0;
        float m_timeStamp = 0;
        float m_speed = 0;
        float m_lightIntensity = 0;
        bool m_bMoving = false;

        public float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public void Update(bool _active, Vector3 _areaSize, Vector3 _areaCenter, Light _light, AnimationCurve _lightIntensityCurve)
        {
            if(!m_bMoving)
            {
                if (!_active)
                    return;

                m_bMoving = true;
                _light.transform.position = _areaCenter + new Vector3(-_areaSize.x/2, UnityEngine.Random.Range(-_areaSize.y/2, _areaSize.y/2), UnityEngine.Random.Range(-_areaSize.z / 2, _areaSize.z / 2));
                m_timeStamp = Time.time;
                m_delayTime = UnityEngine.Random.Range(2,6);
                m_lightIntensity = UnityEngine.Random.Range(6, 8);
                m_speed = UnityEngine.Random.Range(6, 12);
                
            }
            else
            {
                _light.transform.Translate(m_speed*Time.deltaTime,0,0);
                if(_light.transform.position.x > _areaCenter.x + _areaSize.x / 2)
                {
                    m_bMoving = false;
                }
            }

            float _from = -_areaSize.x / 2 + _areaCenter.x;
            float _to = _areaSize.x / 2 + _areaCenter.x;
            _light.GetComponent<UnityEngine.Experimental.Rendering.HDPipeline.HDAdditionalLightData>().intensity = _lightIntensityCurve.Evaluate(Remap(_light.transform.position.x, _from, _to, 0 ,1))* m_lightIntensity;

        }
    }

    [SerializeField]
    Light[] m_lightList;

    [SerializeField]
    bool m_active = false;

    [SerializeField]
    Vector3 m_areaSize = new Vector3(5,10, 20);

    [SerializeField]
    Vector3 m_areaCenter = new Vector3(0, 0, 10);

    [SerializeField]
    AnimationCurve m_lightIntensityCurve;

    LightData[] m_lightDatas;

    // Start is called before the first frame update
    void Start()
    {
        m_lightDatas = new LightData[m_lightList.Length];
        for(var i=0; i< m_lightList.Length; i++)
        {
            LightData _d = new LightData();
            m_lightDatas[i] = _d;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < m_lightDatas.Length; i++)
        {
            m_lightDatas[i].Update(m_active, m_areaSize, m_areaCenter, m_lightList[i], m_lightIntensityCurve);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(m_areaCenter, m_areaSize);
    }
}
