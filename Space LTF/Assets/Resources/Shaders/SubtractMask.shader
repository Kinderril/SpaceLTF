Shader "p0/SubtractMask"
{
	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off Fog { Mode off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos  : POSITION;
				float2 uv  : TEXCOORD0;
			};
			
            uniform sampler2D _ssaoBlur1;
			uniform sampler2D _ssaoMask1;
            uniform sampler2D _ssaoBlur2;
			uniform sampler2D _ssaoMask2;
			uniform sampler2D _MainTex;

			v2f vert (appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				 o.uv = v.texcoord;
				return o;
			}
			float4 frag (v2f i) : COLOR
			{
				float4 color = tex2D(_MainTex, i.uv);
				float texA = tex2D(_ssaoMask1, i.uv).r;
				float texB = tex2D(_ssaoBlur1, i.uv).r;
				float texA1 = tex2D(_ssaoMask2, i.uv).r;
				float texB2 = tex2D(_ssaoBlur2, i.uv).r;

				float delta3 = texB - texA;
				if (delta3 > 0){
				
					return lerp(color, float4(0,1,0,1), saturate(delta3 * 0.7));
				}
				float delta4 = texB2 - texA1;
				if (delta4 > 0){
				
					return lerp(color, float4(1,0,0,1), saturate(delta4 * 0.7));
				}
				return color;

				//return delta;
				
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
