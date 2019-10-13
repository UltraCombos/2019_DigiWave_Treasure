using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutHolder : MonoBehaviour
{
    [System.Serializable]
    public struct LayoutSource
    {
        public GameObject[] m_cameras;
        public CanvasGroup m_canvasGroup;
    }

    [SerializeField]
    GameObject m_cameraRoot;

    [SerializeField]
    GameObject m_layoutRoot;

    [SerializeField]
    List<LayoutSource> m_layoutData = new List<LayoutSource>();

    void SetupLayout(int _index)
    {
        if (_index >= m_layoutData.Count || m_layoutData == null)
            return;

        for(var i=0; i< m_cameraRoot.transform.childCount; i++)
            m_cameraRoot.transform.GetChild(i).gameObject.SetActive(false);

        for (var i = 0; i < m_layoutRoot.transform.childCount; i++)
            m_layoutRoot.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0;

        LayoutSource _s = m_layoutData[_index];
        _s.m_canvasGroup.alpha = 1;
        for (var i = 0; i < _s.m_cameras.Length; i++)
            _s.m_cameras[i].SetActive(true);
    }

    void Start()
    {
        SetupLayout(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetupLayout(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetupLayout(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetupLayout(2);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetupLayout(3);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            SetupLayout(4);
    }
}
