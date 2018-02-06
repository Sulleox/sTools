using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent( typeof (Camera) )]
public class CameraDepthModeActivator : MonoBehaviour 
{

public Camera m_Camera = null; 
public Material m_Material = null;
   void Start () 
   {
      m_Camera.depthTextureMode = DepthTextureMode.Depth;
   }

   void OnRenderImage (RenderTexture source, RenderTexture destination)
   {
      Graphics.Blit( source, destination, m_Material);
   }
}
