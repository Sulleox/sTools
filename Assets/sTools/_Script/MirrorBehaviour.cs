using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBehaviour : MonoBehaviour
{
    private Camera m_MainCamera = null;
    private Camera m_MirrorCamera = null;

    private void Awake()
    {
        m_MainCamera = Camera.main;
        m_MirrorCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPositon = new Vector3(m_MainCamera.transform.position.x, -m_MainCamera.transform.position.y, m_MainCamera.transform.position.z);
        m_MirrorCamera.transform.position = newPositon;
        m_MirrorCamera.transform.LookAt(transform.position, m_MirrorCamera.transform.up);
    }
}
