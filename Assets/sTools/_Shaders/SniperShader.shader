Shader "sTools/SniperShader" 
{
	Properties 
	{
		_RenderTex ("Render Texture", 2D) = "white" {}
		_UVDeformMap ("UV Deform Map", 2D) = "yellow" {}
		_Deformation ("UV Deformation", Range(1,3)) = 1
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _RenderTex;
		sampler2D _UVDeformMap;

		float _Deformation;

		struct Input 
		{
			float2 uv_RenderTex;
			float2 uv_DeformMap;
		};

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float2 uvDeform = tex2D (_UVDeformMap, (IN.uv_RenderTex));
			// float uDeform = abs(pow(IN.uv_RenderTex.x, _Deformation));
			// float vDeform = abs(pow(IN.uv_RenderTex.y, _Deformation));
			// float2 uvDeform = float2(uDeform,vDeform);
			// uvDeform.x = uvDeform.x - (1-_Deformation)/10;
			// uvDeform.y = uvDeform.y - (1-_Deformation)/10;

			fixed4 albedo = tex2D (_RenderTex, uvDeform);
			o.Albedo = albedo.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
