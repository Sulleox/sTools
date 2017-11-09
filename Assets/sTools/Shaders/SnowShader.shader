Shader "sTools/SnowShader"
{
	Properties
	{
		//Shader Variables
		_SnowVector("SnowVector", vector) = (0,1,0,0)
		_SnowThreshold("Threshold", Range(0,1)) = 0.0
		_SnowAmount("Amount", Range(0,0.2)) = 0.0

		//Snow Texture
		_snowColor("Color", Color) = (1,1,1,1)
		_snowTex("Snow Texture", 2D) = "white" {}
		_snowGlossiness("Snow Smoothness", Range(0,1)) = 0.5
		_snowMetallic("Snow Metallic", Range(0,1)) = 0.0
		_snowMet("Snow Metallic Map", 2D) = "white" {}
		_snowBumpMap("Snow Normal Map", 2D) = "bump" {}

		//Other Texture
		_otherColor("Color", Color) = (1,1,1,1)
		_otherTex("Other Texture", 2D) = "white" {}
		_otherGlossiness("Other Smoothness", Range(0,1)) = 0.5
		_otherMetallic("Other Metallic", Range(0,1)) = 0.0
		_otherMet("Other Metallic Map", 2D) = "white" {}
		_otherBumpMap("Other Normal Map", 2D) = "bump" {}
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert addshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input
		{
			float2 uv_snowTex;
			float2 uv_otherTex;
			float _SnowMask;
		};

		float3 _SnowVector;
		float _SnowAmount;
		float _SnowThreshold;

		//Snow
		float4 _snowColor;
		sampler2D _snowTex;
		sampler2D _snowMet;
		sampler2D _snowBumpMap;
		half _snowGlossiness;
		half _snowMetallic;

		//Other
		float4 _otherColor;
		sampler2D _otherTex;
		sampler2D _otherMet;
		sampler2D _otherBumpMap;
		half _otherGlossiness;
		half _otherMetallic;

		float _SnowMask;

		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

			void vert(inout appdata_full v, out Input o)
		{
			//Enable input save in Vertex Edit.
			UNITY_INITIALIZE_OUTPUT(Input, o);
			//Generate worldNormal & apply it to the vertex position
			float3 worldNormal = UnityObjectToWorldNormal(v.normal);
			_SnowMask = saturate(saturate(dot(_SnowVector, worldNormal)) / _SnowThreshold);
			v.vertex.xyz += v.normal * _SnowAmount * _SnowMask;
			o._SnowMask = _SnowMask;

		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//Albedo
			fixed4 snowAlbedo = tex2D(_snowTex, IN.uv_snowTex) *_snowColor;
			fixed4 otherAlbedo = tex2D(_otherTex, IN.uv_otherTex) *_otherColor;
			o.Albedo = lerp(otherAlbedo, snowAlbedo, IN._SnowMask);

			//Metallic && Glossiness
			fixed4 snowMetallic = tex2D(_snowMet, IN.uv_snowTex) * _snowMetallic;
			fixed4 otherMetallic = tex2D(_otherMet, IN.uv_otherTex) * _otherMetallic;

			o.Metallic = lerp(otherMetallic.r, snowMetallic.r, IN._SnowMask);
			o.Smoothness = lerp(otherMetallic.a, otherMetallic.a, IN._SnowMask);
			//o.Normal = UnpackNormal();

			o.Alpha = _snowColor.a;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
