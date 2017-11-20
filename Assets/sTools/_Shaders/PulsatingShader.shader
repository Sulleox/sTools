Shader "sTools/PulseShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MetallicMap("Metallic Map", 2D) = "black" {} 
		_GlossinessRange ("Smoothness", Range(0,1)) = 0.5
		_MetallicRange ("Metallic", Range(0,1)) = 0.0
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_AO ("AO Map", 2D) = "white" {}
		_PulseAmount("Pulse Amount", Range(0,1)) = 0.5
		_PulseSpeed("Pulse Speed", float) = 1
		_PulseMask ("Pulse Mask", 2D) = "white" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM

		#pragma surface surf Standard vertex:vert addshadow
		#pragma target 3.0

		struct Input 
		{
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		sampler2D _MetallicMap;
		sampler2D _BumpMap;
		sampler2D _AO;
		sampler2D _PulseMask;

		half _GlossinessRange;
		half _MetallicRange;
		half _PulseAmount;

		fixed4 _Color;
		float _PulseSpeed;

		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void vert(inout appdata_full v)
		{
			//Able to keep the tiling and offset. 
			float4 Mask = tex2Dlod(_PulseMask, float4(v.texcoord.xy, 0, 0));
			v.vertex.xyz += v.normal * _PulseAmount * Mask * (1+sin(_Time.x * _PulseSpeed));
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Metallic = tex2D (_MetallicMap, IN.uv_MainTex).r * _MetallicRange;
			o.Smoothness = tex2D (_MetallicMap, IN.uv_MainTex).a * _GlossinessRange;
			o.Occlusion = tex2D (_AO, IN.uv_MainTex);
			o.Normal = tex2D (_BumpMap, IN.uv_MainTex);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
