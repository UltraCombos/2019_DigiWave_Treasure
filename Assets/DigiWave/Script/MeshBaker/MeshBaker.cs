using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBaker : MonoBehaviour
{
    //
    [SerializeField]
    SkinnedMeshRenderer m_skinnedMesh;
    [SerializeField]
    RenderTexture m_positionRT;
    [SerializeField]
    RenderTexture m_velocityRT;
    [SerializeField]
    ComputeShader m_computeShader;

    //
    Mesh m_mesh;
    ComputeBuffer m_positionBuffer;
    ComputeBuffer m_prevPositionBuffer;
    RenderTexture m_tempPosRT;
    RenderTexture m_tempVelRT;
    List<Vector3> m_positionList = new List<Vector3>();
    Matrix4x4 m_prevTransformMatrix = Matrix4x4.identity;
    int m_kernel;

    void ReleaseBuffer(ComputeBuffer _buffer)
    {
         if (_buffer != null)
         {
            _buffer.Dispose();
            _buffer = null;
         }
    }

    void ReleaseRT(RenderTexture _rt)
    {
        if (_rt != null)
        {
            _rt.Release();
            _rt = null;
        }
    }

    void CheckInternalResources()
    {
        if(m_positionBuffer==null||m_positionBuffer.count != m_mesh.vertexCount)
        {
            ReleaseBuffer(m_positionBuffer);
            m_positionBuffer = new ComputeBuffer(m_mesh.vertexCount, 12);
        }

        if (m_prevPositionBuffer == null || m_prevPositionBuffer.count != m_mesh.vertexCount)
        {
            ReleaseBuffer(m_prevPositionBuffer);
            m_prevPositionBuffer = new ComputeBuffer(m_mesh.vertexCount, 12);
        }

        if (m_tempPosRT == null || m_tempPosRT.width != m_positionRT.width)
        {
            ReleaseRT(m_tempPosRT);
            m_tempPosRT = new RenderTexture(m_positionRT.descriptor);
            m_tempPosRT.enableRandomWrite = true;
            m_tempPosRT.Create();
        }

        if (m_tempVelRT == null || m_tempVelRT.width != m_velocityRT.width)
        {
            ReleaseRT(m_tempVelRT);
            m_tempVelRT = new RenderTexture(m_velocityRT.descriptor);
            m_tempVelRT.enableRandomWrite = true;
            m_tempVelRT.Create();
        }
    }

    void Awake()
    {
        m_mesh = new Mesh();

        if(m_computeShader!=null)
            m_kernel = m_computeShader.FindKernel("Main");
    }

    void Update()
    {
        if (m_skinnedMesh == null || m_computeShader == null || m_positionRT == null || m_velocityRT == null)
            return;
        
        m_skinnedMesh.BakeMesh(m_mesh);

        CheckInternalResources();

        m_mesh.GetVertices(m_positionList);
        m_positionBuffer.SetData(m_positionList);
 
        m_computeShader.SetInt("m_vertexCount", m_positionBuffer.count);
        m_computeShader.SetFloat("m_deltaTime", Time.deltaTime);
        //-
        m_computeShader.SetMatrix("m_transformMatrix", m_skinnedMesh.transform.localToWorldMatrix);
        m_computeShader.SetMatrix("m_prevTransformMatrix", m_prevTransformMatrix);
        //-
        m_computeShader.SetBuffer(m_kernel, "m_positionBuffer", m_positionBuffer);
        m_computeShader.SetBuffer(m_kernel, "m_prevPositionBuffer", m_prevPositionBuffer);
        //-
        m_computeShader.SetTexture(m_kernel, "m_positionRT", m_tempPosRT);
        m_computeShader.SetTexture(m_kernel, "m_velocityRT", m_tempVelRT);
        m_computeShader.Dispatch(m_kernel, m_positionRT.width / 8, m_positionRT.height / 8, 1);

        //Update Result
        Graphics.Blit(m_tempPosRT, m_positionRT);
        Graphics.Blit(m_tempVelRT, m_velocityRT);

        //Update Prev Data
        m_prevTransformMatrix = m_skinnedMesh.transform.localToWorldMatrix;
        m_prevPositionBuffer.SetData(m_positionList);
    }

    void OnDestroy()
    {
        ReleaseBuffer(m_positionBuffer);
        ReleaseBuffer(m_prevPositionBuffer);
        ReleaseRT(m_tempVelRT);
        ReleaseRT(m_tempPosRT);
    }
}
