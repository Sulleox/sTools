using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DepthBufferCamera : MonoBehaviour 
{
	void Awake()
	{
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
	}
}