Shader "sTools/WaterShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_WaterTexture ("Water Texture (RGB)", 2D) = "white" {}
		_WaterNormal ("Water Normal Map", 2D) = "blue" {}

		_ScrollXSpeed ("XSpeed", float) = 0.5
		_ScrollYSpeed ("YSpeed", float) = 0.5
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _WaterTexture;
		sampler2D _WaterNormal;

		struct Input 
		{
			float2 uv_WaterTexture;
		};

		fixed4 _Color;

		fixed _ScrollXSpeed;
		fixed _ScrollYSpeed;
		
		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed2 scrolledUV = IN.uv_WaterTexture;
			fixed xScrollValue = _ScrollXSpeed * _Time;
         	fixed yScrollValue = _ScrollYSpeed * _Time;
			scrolledUV += fixed2(xScrollValue, yScrollValue);
			
			fixed4 c = tex2D (_WaterTexture, scrolledUV) * _Color;
			fixed4 normal = tex2D (_WaterNormal, scrolledUV);
			
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = normal;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
