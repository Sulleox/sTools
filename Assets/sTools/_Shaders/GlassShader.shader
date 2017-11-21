Shader "sTools/GlassShader" 
{
	Properties 
	{
		_GlassSteamMap("Glass Steam Map", 2D) = "black" {}
		_GlassTransparency("Glass Map Transparency", Range(0,1)) = 0.5

		_GlassDamageMap ("Glass Damage Map", 2D) = "white" {}
		_DamageTransparency ("Glass Damage Map Transparency", Range(0,1)) = 0.5
		_DamageColor("Glass Damage Color", Color) = (1,1,1,0.5)

		_DirtMap("Dirt Map", 2D) = "white" {}
	}
	SubShader 
	{
		Tags 
		{ 
		"RenderType"="Transparent" 
		"IgnoreProjector"="True" 
		"Queue"="Transparent"
		}

		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard alpha 
		#pragma target 3.0
		 
		struct Input 
		{
			float2 uv_GlassSteamMap;
		};

		sampler2D _GlassSteamMap;
		sampler2D _GlassDamageMap;
		sampler2D _DirtMap;

		half _GlassTransparency;
		half _DamageTransparency;

		fixed4 _DamageColor;


		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float4 dirtMap = tex2D(_DirtMap, IN.uv_GlassSteamMap);
			float4 glassMap = tex2D(_GlassSteamMap, IN.uv_GlassSteamMap);
			float4 DamageMap = tex2D(_GlassDamageMap, IN.uv_GlassSteamMap) * _DamageColor * (1- glassMap);
		
			//Albedo
			o.Albedo = dirtMap + DamageMap ;
			//Alpha
			o.Alpha = (DamageMap * _DamageTransparency) + (glassMap * _GlassTransparency);
			//Emission
			o.Emission = DamageMap * _DamageTransparency;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
