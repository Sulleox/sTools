Shader "sTools/VertexColor/VertexColorShader" 
{
	Properties 
	{
		_RedAlbedo ("Red Texture Albedo", 2D) = "red"{}
		_GreenAlbedo ("Green Texture Albedo", 2D) = "green"{}
		_BlueAlbedo ("Blue Texture Albedo", 2D) = "blue"{}
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

			fixed2 uv_RedAlbedo;
			fixed2 uv_GreenAlbedo;
			fixed2 uv_BlueAlbedo;
		};

		sampler2D _RedAlbedo;
		sampler2D _GreenAlbedo;
		sampler2D _BlueAlbedo;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 redAlbedo = tex2D(_RedAlbedo, IN.uv_RedAlbedo);
			fixed4 greenAlbedo = tex2D(_GreenAlbedo, IN.uv_GreenAlbedo);
			fixed4 blueAlbedo = tex2D(_BlueAlbedo, IN.uv_BlueAlbedo);

			fixed4 RGLerp = lerp(redAlbedo, greenAlbedo, IN.color.g);
			fixed4 RGBLerp = lerp(RGLerp, blueAlbedo, IN.color.b);
			fixed4 RGBGLerp = lerp(RGBLerp, redAlbedo, IN.color.r);
			o.Albedo = RGBGLerp ;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
