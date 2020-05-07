Shader "p0/NightVision"
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
			
            uniform sampler2D _MainTex;
			uniform sampler2D _TexA;
			uniform sampler2D _TexB;

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
				float texA = tex2D(_TexA, i.uv).r;
				float texB = tex2D(_TexB, i.uv).r;

				float delta = texB - texA;

				return lerp(color, float4(1,0,0,1), delta);
				
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
