Shader "sTools/GlassShader" 
{
	Properties 
	{

		_DamageColor("Glass Damage Color", Color) = (1,1,1,0.5)
		_GlassDamageMap ("Glass Damage Map", 2D) = "white" {}
		_DamageTransparency ("Damage Transparency", Range(0,1)) = 0.5

		_DirtMap("Dirt Map", 2D) = "white" {}
		_DirtMask("Dirt Mask", 2D) = "white" {}
		_DirtTransparency("Dirt Map Transparency", Range(0,1)) = 0.5

		_RefractionBumpMap("Refraction Map", 2D) = "bump" {}
		_RefractionValue("Refraction Range", Range(0,1)) = 0.5
	}
	SubShader 
	{

	GrabPass
        {
           "_BackgroundTexture"
        }

		Tags 
		{ 
		"RenderType"="Transparent" 
		"IgnoreProjector"="True" 
		"Queue"="Transparent"
		"ForceNoShadowCasting" = "True"
		}

		CGPROGRAM
		#pragma surface surf Standard alpha 
		#pragma target 3.0
		 
		struct Input 
		{
			float2 uv_DirtMap;
			float2 uv_RefractionBumpMap;
			float4 screenPos;
		};

		sampler2D _GlassDamageMap;

		sampler2D _DirtMap;
		sampler2D _DirtMask;

		sampler2D _BackgroundTexture;
		sampler2D _RefractionBumpMap;

		half _DirtTransparency;
		half _DamageTransparency;
		half _RefractionValue;

		fixed4 _DamageColor;


		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			//Dirt & Damage
			float4 dirtMap =  tex2D(_DirtMap, IN.uv_DirtMap);
			float4 dirtMask = tex2D(_DirtMask, IN.uv_DirtMap) ;
			float4 dirtMaskFiltred = dirtMask * _DirtTransparency;
			float4 DamageMap = tex2D(_GlassDamageMap, IN.uv_DirtMap) * _DamageTransparency;
		
			//Get ScreenUV
			float w = max(0.0001, IN.screenPos.w);
			float2 ScreenUV = IN.screenPos.xy / w;

			//Refract 
			float2 RefractNormal = UnpackNormal(tex2D(_RefractionBumpMap, IN.uv_RefractionBumpMap  ));
			//Project BackgroundTexture on ScreenUV
			float4 ModifiedTexture = tex2D(_BackgroundTexture, ScreenUV + (RefractNormal * (_RefractionValue / 50)));

			//Albedo
			o.Albedo = ModifiedTexture + (dirtMap * dirtMask * _DirtTransparency);
			o.Emission = DamageMap * _DamageColor;
			o.Alpha = 1.0f;

		}
		ENDCG
	}
	FallBack "Diffuse"
}
