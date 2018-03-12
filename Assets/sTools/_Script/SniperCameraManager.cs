using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SniperCameraManager : MonoBehaviour 
{

	public Camera m_Camera = null;
	public Material m_Material = null;
	public float m_ScopeDistance = 100f;
	public bool DebugRay = false;
	[Range(0,1)] public float m_Deformation = 0.5f;

	void Update()
	{
		if(DebugRay) Debug.DrawRay(transform.position, -transform.forward * m_ScopeDistance, Color.red, 0.01f);
		if(m_Camera != null)
		{
			m_Camera.farClipPlane = m_ScopeDistance;
		}
		if(m_Material != null)
		{
			m_Material.SetFloat("_UVDeformation", m_Deformation);
		}
	}
}
