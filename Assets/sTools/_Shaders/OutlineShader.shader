// Upgrade NOTE: replaced 'glstate.matrix.modelview[0]' with 'UNITY_MATRIX_MV'
// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate.matrix.projection' with 'UNITY_MATRIX_P'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "sTools/Outline/OutlineShader" 
{
	Properties 
	{
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline width", Range (0, 0.1)) = .005
        _MainTex ("Albedo", 2D) = "white" { }
	}
	SubShader 
	{
		Tags { "LightMode" = "Always" }

		//RENDER OUTLINE
		CGPROGRAM
		// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct appdata members vertex,normal)
		#pragma exclude_renderers d3d11
		struct appdata 
		{
            float4 vertex;
            float3 normal;
        };

		struct v2f 
		{
            float4 pos : POSITION;
            float4 color : COLOR;
        };
           
		uniform float _OutlineWidth;
        uniform float4 _OutlineColor;
 
        v2f vert(appdata v) 
		{
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            float3 norm = mul ((float3x3)UNITY_MATRIX_MV, v.normal);
            norm.x *= UNITY_MATRIX_P[0][0];
            norm.y *= UNITY_MATRIX_P[1][1];
            o.pos.xy += norm.xy * o.pos.z * _OutlineWidth;
            o.color = _OutlineColor;
            return o;
        }
        ENDCG

		//RENDER OBJECT
		Tags { "LightMode" = "Always" }
		Lighting On
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		struct Input 
		{
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		uniform float _OutlineWidth;
		uniform float4 _OutlineColor;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
