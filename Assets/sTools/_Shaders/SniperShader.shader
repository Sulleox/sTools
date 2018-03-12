Shader "sTools/SniperShader" 
{
	Properties 
	{
		_RenderTex ("Render Texture", 2D) = "white" {}
		_UVDeformMap ("UV Deform Map", 2D) = "yellow" {}
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

		float _Deformation;

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

			fixed4 albedo = tex2D (_RenderTex, uvDeform);
			fixed4 visor = tex2D (_VisorTex, IN.uv_RenderTex);
			o.Albedo = albedo.rgb + visor.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
