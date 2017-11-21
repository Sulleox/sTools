Shader "sTools/TextureBlend" 
{
	Properties 
	{
		//Blend Properties
		_blendAmount("Blend Amount", Range(0,1)) = 0.5
		_blendMask("Blend Mask", 2D) = "white" {}
			
		//First Texture
		_FirstTexture ("First Texture Map", 2D) = "white" {}
		_FirstMetallicMap("First Metallic Map", 2D) = "black" {}
		_FirstGlossinessRange ("First Smoothness Range", Range(0,1)) = 0.5
		_FirstMetallicRange ("First Metallic Range", Range(0,1)) = 0.0
		_FirstAO("First AO Map", 2D) = "white" {}
		_FirstBumpMap("First Texture Normal Map", 2D) = "white"{}

		//Second Texture
		_SecondTexture ("Second Texture Map", 2D) = "white" {}
		_SecondMetallicMap("Second Metallic Map", 2D) = "black" {}
		_SecondGlossinessRange ("Second Smoothness Range", Range(0,1)) = 0.5
		_SecondMetallicRange ("Second Metallic Range", Range(0,1)) = 0.0
		_SecondAO("Second AO Map", 2D) = "white" {}
		_SecondBumpMap("Second Texture Normal Map", 2D) = "white"{}

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
			float2 uv_FirstTexture;
			float2 uv_SecondTexture;
		};

		sampler2D _blendMask;

		sampler2D _FirstTexture;
		sampler2D _FirstMetallicMap;
		sampler2D _FirstAO;
		sampler2D _FirstBumpMap;

		sampler2D _SecondTexture;
		sampler2D _SecondMetallicMap;
		sampler2D _SecondAO;
		sampler2D _SecondBumpMap;

		half _FirstGlossinessRange;
		half _FirstMetallicRange;
		half _SecondGlossinessRange;
		half _SecondMetallicRange;

		half _blendAmount;

		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float4 blendMask = tex2D(_blendMask, IN.uv_FirstTexture);
			//Albedo
			float4 firstTexture = tex2D(_FirstTexture, IN.uv_FirstTexture);
			float4 secondTexture = tex2D(_SecondTexture, IN.uv_SecondTexture);
			o.Albedo = lerp(firstTexture, secondTexture, (_blendAmount * blendMask));
			//Metallic & Smoothness
			float4 firstMetallic = tex2D(_FirstMetallicMap, IN.uv_FirstTexture);
			float4 secondMetallic = tex2D(_SecondMetallicMap, IN.uv_SecondTexture).r;
			o.Metallic = lerp(firstMetallic.r, secondMetallic.r, (_blendAmount * blendMask));
			o.Smoothness = lerp(firstMetallic.a, secondMetallic.a, (_blendAmount * blendMask));
			//Ambient Occlusion
			float4 firstAO = tex2D(_FirstAO, IN.uv_FirstTexture);
			float4 secondAO = tex2D(_SecondAO, IN.uv_FirstTexture);
			o.Occlusion = lerp(firstAO, secondAO, (_blendAmount * blendMask));
			//Normal
			float4 firstNormal = tex2D(_FirstBumpMap, IN.uv_FirstTexture);
			float4 secondNormal = tex2D(_SecondBumpMap, IN.uv_SecondTexture);
			o.Normal = lerp(firstNormal, secondNormal, (_blendAmount * blendMask));
		}
		ENDCG
	}
	FallBack "Diffuse"
	CustomEditor "TextureBlendEditor"
}
