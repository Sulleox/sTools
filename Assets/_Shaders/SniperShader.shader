Shader "sTools/SniperShader" 
{
	Properties 
	{
		_RenderTex ("Render Texture", 2D) = "white" {}
		_UVDeformMap ("UV Deform Map", 2D) = "yellow" {}
		_UVDeformation ("UV Deformation", Range(0,1)) = 0.5
		_VisorTex ("Visor Texture", 2D) = "black" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _RenderTex;
		sampler2D _UVDeformMap;
		sampler2D _VisorTex;

		float _UVDeformation;

		struct Input 
		{
			float2 uv_RenderTex;
			float2 uv_DeformMap;
		};

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float2 uvDeform = tex2D (_UVDeformMap, IN.uv_RenderTex);
			float2 uvDeformValue = (uvDeform * _UVDeformation) + (IN.uv_RenderTex * (1 - _UVDeformation));

			fixed4 albedo = tex2D (_RenderTex, uvDeformValue);
			fixed4 visor = tex2D (_VisorTex, IN.uv_RenderTex);
			o.Albedo = albedo.rgb * visor.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
