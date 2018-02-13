Shader "sTools/FoamShader" 
{
	Properties 
	{
		_FoamTexture ("Foam Texture", 2D) = "black" {}
		_FoamMask ("Foam Mask", 2D) = "white" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Diffuse"  }
		LOD 200
		ZWrite on

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows Lambert alpha
		#pragma target 3.0

		sampler2D _FoamTexture;
		sampler2D _FoamMask;

		struct Input 
		{
			float2 uv_FoamTexture;
		};


		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float4 foamTex = tex2D(_FoamTexture, IN.uv_FoamTexture);
			float4 foamMask = tex2D(_FoamTexture, IN.uv_FoamTexture);

			o.Albedo = foamTex;
			o.Alpha = foamMask;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
