//SPECIAL THANKS TO LOUIS VALET

Shader "sTools/ZDepthShader" 
{
	Properties 
	{
		_RimSize ("Rim Size", Range(01, 1)) = 0.5
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
		ZWrite off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _CameraDepthTexture;

			struct v2f 
			{
				float4 pos : SV_POSITION;
				float4 scrPos:TEXCOORD1;
			};

			float _RimSize;

			//Vertex Shader
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.scrPos = ComputeScreenPos(o.pos);
				o.scrPos.y = o.scrPos.y;
				return o;
			}

			//Fragment Shader
			half4 frag (v2f i) : SV_Target
			{
				float depthValue = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)));
				float rim = saturate(1 - (depthValue - i.scrPos.w) * _RimSize / 0.2);
				return rim;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
