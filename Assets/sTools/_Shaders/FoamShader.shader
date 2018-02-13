Shader "sTools/FoamShader" 
{
	Properties 
	{
		_FoamTexture ("Foam Texture", 2D) = "black" {}
		_FoamMask ("Foam Mask", 2D) = "white" {}

		_ScrollXSpeed ("XSpeed", float) = 0.5
		_ScrollYSpeed ("YSpeed", float) = 0.5
	}
	SubShader 
	{
		Tags { "RenderType"="Diffuse"  }
		LOD 200
		ZWrite on

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows Lambert alpha
		#pragma target 3.0

		struct Input 
		{
			float2 uv_FoamTexture;
		};

		sampler2D _FoamTexture;
		sampler2D _FoamMask;
		fixed _ScrollXSpeed;
		fixed _ScrollYSpeed;
		float2 temp_scrolledUV;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float temp_xScrollValue = _ScrollXSpeed * _Time;
         	float temp_yScrollValue = _ScrollYSpeed * _Time;
			temp_scrolledUV += fixed4(temp_xScrollValue, temp_yScrollValue, 1, 1);
			
			float4 foamTex = tex2D(_FoamTexture, IN.uv_FoamTexture);
			float4 foamMask = tex2D(_FoamMask, IN.uv_FoamTexture);

			o.Albedo = foamTex;
			o.Alpha = foamMask;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
