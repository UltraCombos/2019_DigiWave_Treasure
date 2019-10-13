using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class WhaleControl : MonoBehaviour
{
    [SerializeField]
    Animator m_animator;

    [SerializeField]
    float m_animationSpeed = 1;

    Material m_mat;

    [SerializeField]
    float m_time;

    void Awake()
    {
        m_mat = GetComponent<SkinnedMeshRenderer>().sharedMaterials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (m_animator != null)
            m_animator.SetFloat("Speed", m_animationSpeed);

        m_time += Time.deltaTime;
        m_mat.SetFloat("_fakeTime", m_time);
    }
}
