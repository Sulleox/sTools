Shader "sTools/TextureBlend(NotFinished)" 
{
	Properties 
	{
		//First Texture
		_FirstColor ("First Color", Color) = (1,1,1,1)
		_FirstTexture ("First Texture Map", 2D) = "white" {}
		_FirstMetallicMap("First Metallic Map", 2D) = "black" {}
		_FirstGlossinessRange ("First Smoothness Range", Range(0,1)) = 0.5
		_FirstMetallicRange ("First Metallic Range", Range(0,1)) = 0.0
		_FirstBumpMap("First Texture Normal Map", 2D) = "white"{}

		//Second Texture
		_SecondColor ("Second Color", Color) = (1,1,1,1)
		_SecondTexture ("Second Texture Map", 2D) = "white" {}
		_SecondMetallicMap("Second Metallic Map", 2D) = "black" {}
		_SecondGlossinessRange ("Second Smoothness Range", Range(0,1)) = 0.5
		_SecondMetallicRange ("Second Metallic Range", Range(0,1)) = 0.0
		_SecondBumpMap("Second Texture Normal Map", 2D) = "white"{}

	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _FirstTexture;

		struct Input 
		{
			float2 uv_FirstTexture;
		};

		half _FirstGlossiness;
		half _FirstMetallic;
		fixed4 _FirstColor;

		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_FirstTexture, IN.uv_FirstTexture) * _FirstColor;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _FirstMetallic;
			o.Smoothness = _FirstGlossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
