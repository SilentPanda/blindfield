Shader "SilentPanda/Background" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _sourceColor ("Source Color", Color) = (1, 1, 1, 1)
        _targetColor ("Target Color", Color) = (0, 0, 0, 0)
        _blendAmount ("Blend Amount", Range (0, 1)) = 0
    }

    SubShader {
        Pass {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform half4 _sourceColor;
            uniform half4 _targetColor;
            uniform float _blendAmount;

			uniform half2 _playerCenter;

            float4 frag(v2f_img i) : COLOR
			{
				half2 uv = half2(0,0);
				uv.x = distance(half2(0.5,0.5), i.uv) + _Time.w;

				return half4(i.pos.xy, 0, 0);// tex2D(_MainTex, i.uv);
            }

            ENDCG
        }
    }
}
