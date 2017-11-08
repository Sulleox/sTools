Shader "sTools/SnowShader"
{
	Properties
	{
		_snowColor("Color", Color) = (1,1,1,1)
		_snowTex("SnowTexture (RGB)", 2D) = "white" {}
		_otherColor("Color", Color) = (1,1,1,1)
		_otherTex("OthrTexture (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_SnowVector("SnowVector", vector) = (1,1,1,1)
		_SnowThreshold("Threshold", Range(0,1)) = 0.0
		_Amount("Amount", Range(0,0.2)) = 0.0
	}
		SubShader
		{
			Tags{ "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows vertex:vert addshadow

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			struct Input
			{
				float2 uv_snowTex;
				float2 uv_otherTex;
				float _SnowMask;
			};

			sampler2D _snowTex;
			sampler2D _otherTex;

			half _Glossiness;
			half _Metallic;
			float3 _SnowVector;
			float4 _snowColor;
			float4 _otherColor;
			float _Amount;
			float _SnowThreshold;
			float _SnowMask;

			UNITY_INSTANCING_CBUFFER_START(Props)
			UNITY_INSTANCING_CBUFFER_END

				void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);
				float3 worldNormal = UnityObjectToWorldNormal(v.normal);
				_SnowMask = saturate(saturate(dot(_SnowVector, worldNormal)) / _SnowThreshold);
				v.vertex.xyz += v.normal * _Amount * _SnowMask;
				o._SnowMask = _SnowMask;

			}

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				fixed4 snowAlbedo = tex2D(_snowTex, IN.uv_snowTex) * _snowColor;
				fixed4 otherAlbedo = tex2D(_otherTex, IN.uv_otherTex) * _otherColor;
				o.Albedo = lerp(otherAlbedo, snowAlbedo, IN._SnowMask);
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = _snowColor.a;
			}



			ENDCG
		}
			FallBack "Diffuse"
}
