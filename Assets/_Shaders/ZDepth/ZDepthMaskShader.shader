Shader "sTools/ZDepth Mask (Surface)" 
{
	Properties 
	{
		_DepthSize ("Depth Size", range(0.1, 1)) = 0.5
		_DepthAtten ("Depth Attenuation", range(1,10)) = 1
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
		ZWrite off

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows 
		#pragma target 3.0

		//ZDEPTH PARAMETERS
		sampler2D _CameraDepthTexture;
		float _DepthSize;
		float _DepthAtten;

		struct Input 
		{
			float4 screenPos;
		};

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float depthValue = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
			float rim = pow(saturate(1 - (depthValue - IN.screenPos.w) * (1 - _DepthSize) / 0.2), _DepthAtten);

			o.Albedo = rim;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
