Shader "Unlit/Laser"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color1 ("Texture", Color) = (1,1,1,1)
		_Color2 ("Texture", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 objectPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			half4 _Color1, _Color2;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.objectPos = v.vertex;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				half a = sin( i.uv.x * 3.14 ) * sin( i.uv.y * 3.14 );
				return half4( a, a, a, a ) * lerp( _Color1, _Color2, a );
			}
			ENDCG
		}
	}
}
