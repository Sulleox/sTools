// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "sTools/MirrorShader[Not Finished]" 
{
	Properties 
	{
		_RenderTex ("Render Texture", 2D) = "white" {}
		_DirtMap ("Dirt Map", 2D) = "black"{}
		_DirtMask ("Dirt Mask", 2D) = "black" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _RenderTex;
		sampler2D _DirtMap;
		sampler2D _DirtMask;

		struct Input 
		{
			float2 uv_RenderTex;
		};

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			//Invert UV
			float2 mirrorUV;
			mirrorUV.x = 1 - IN.uv_RenderTex.x;
			mirrorUV.y = IN.uv_RenderTex.y;

			//Dirt Map & Dirt Mask
			float4 DirtMap = tex2D(_DirtMap, IN.uv_RenderTex);
			float4 DirtMask = tex2D(_DirtMask, IN.uv_RenderTex);

			o.Albedo = tex2D(_RenderTex, mirrorUV) * (1-DirtMask) + DirtMap * DirtMask;
			o.Alpha = 1.0f;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
