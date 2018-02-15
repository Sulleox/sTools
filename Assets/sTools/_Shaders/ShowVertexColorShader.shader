Shader "sTools/ShowVertexColorShader" 
{
	Properties 
	{
		
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
			fixed4 color : COLOR;
		};

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			o.Albedo = IN.color;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
