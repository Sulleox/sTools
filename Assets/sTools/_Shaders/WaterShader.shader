// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "sTools/WaterShader" 
{
	Properties 
	{
		_DepthColor ("Depth Color", Color) = (1,1,1,1)
		_ScrollXSpeed ("XSpeed", float) = 0.5
		_ScrollYSpeed ("YSpeed", float) = 0.5

		_WaterColor ("Water Color", Color) = (1,1,1,1)
		_WaveHeight("Wave Height", Range(0, 0.9)) = 0.5
		_WaterNormal ("Water Normal Map", 2D) = "blue" {}
		_WaterHeight ("Wave Height Map", 2D) = "black" {}

		_DepthSize ("Depth Size", Range(0, 0.9)) = 0.5
	}
	SubShader 
	{
		LOD 200
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
		ZWrite off
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert alpha
		#pragma target 4.6

		struct Input 
		{
			fixed2 scrolledUV;
			fixed xScrollValue;
         	fixed yScrollValue;

			float4 pos : SV_POSITION;
			float4 scrPos:TEXCOORD1;
		};

		//ZDEPTH TEST PARAMETERS
		uniform sampler2D _CameraDepthTexture;

		//SCROLLING PARAMETERS
		fixed _ScrollXSpeed;
		fixed _ScrollYSpeed;

		//WATER PARAMETERS
		sampler2D _WaterNormal;
		sampler2D _WaterHeight;
		fixed4 _DepthColor;
		fixed4 _WaterColor;
		float _WaveHeight;
		
		//DEPTH PARAMETERS
		float _DepthSize;

		//TEMPORARY VALUE
		float4 temp_scrolledUV;
		float temp_xScrollValue;
		float temp_yScrollValue;
		
		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o)
		{
			//ENABLE INPUT SAVE IN VERTEX
			UNITY_INITIALIZE_OUTPUT(Input, o);

			//SCROLLING OF NORMAL MAP & HEIGHT MAP
			temp_scrolledUV = fixed4 (v.texcoord.xy, 0, 0);
			temp_xScrollValue = _ScrollXSpeed * _Time;
         	temp_yScrollValue = _ScrollYSpeed * _Time;
			temp_scrolledUV += fixed4(temp_xScrollValue, temp_yScrollValue, 1, 1);

			float4 HeightMask = tex2Dlod(_WaterHeight, temp_scrolledUV);
			v.vertex.y -= _WaveHeight * HeightMask;

			//SAVE SCROLLED UV PARAMETERS
			o.scrolledUV = temp_scrolledUV;
			o.xScrollValue = temp_xScrollValue;
			o.yScrollValue = temp_yScrollValue;

			//SAVING ZDEPTH PARAMETERS
			o.pos = UnityObjectToClipPos (v.vertex);
			o.scrPos = ComputeScreenPos(o.pos);
			o.scrPos.y = o.scrPos.y;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = _WaterColor;
			fixed4 normal = tex2D(_WaterNormal, IN.scrolledUV);

			float depthValue = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.scrPos)));
			float rim = saturate(1 - (depthValue - IN.scrPos.w) * (1 - _DepthSize) / 0.2);
			
			//o.Albedo = c.rgb * (1-rim) + _DepthColor * rim;
			o.Albedo = c.rgb * (1-rim);
			o.Smoothness = 0.8;
			o.Normal = normal;
			o.Alpha = 1 - rim;
			o.Metallic = 0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
