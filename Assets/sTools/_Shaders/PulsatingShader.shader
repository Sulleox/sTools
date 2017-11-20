Shader "sTools/PulseShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MetallicMap("Metallic Map", 2D) = "black" {} 
		_GlossinessMap("Glossiness Map", 2D) = "black" {}
		_GlossinessRange ("Smoothness", Range(0,1)) = 0.5
		_MetallicRange ("Metallic", Range(0,1)) = 0.0
		_BumpMap ("Normal Map", 2D) = "bump" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		struct Input 
		{
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		sampler2D _MetallicMap;
		sampler2D _GlossinessMap;
		sampler2D _BumpMap;

		half _GlossinessRange;
		half _MetallicRange;
		fixed4 _Color;

		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Metallic = tex2D (_MetallicMap, IN.uv_MainTex) * _Metallic;
			o.Smoothness = tex2D (_GlossinessMap, IN.uv_MainTex) * _Glossiness;
			o.Normal = tex2D (_BumpMap, IN.uv_MainTex);
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
