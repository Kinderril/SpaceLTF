Shader "Hidden/Blur"
{
	Subshader 
	{
		Pass//Blur
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }

			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"
			
			struct v2f 
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;

				float4 uv01 : TEXCOORD1;
				float4 uv23 : TEXCOORD2;
				float4 uv45 : TEXCOORD3;
			};
			
			float2 _BlurOffsets0;			
			sampler2D _MainTex;
			float _Intensity;
			
			v2f vert (appdata_img v) 
			{
				v2f o;
				o.pos = v.vertex;

				o.uv.xy = v.texcoord.xy;

				o.uv01 =  v.texcoord.xyxy + _BlurOffsets0.xyxy * float4(1, 1, -1, -1);
				o.uv23 =  v.texcoord.xyxy + _BlurOffsets0.xyxy * float4(1, 1, -1, -1) * 2.0;
				o.uv45 =  v.texcoord.xyxy + _BlurOffsets0.xyxy * float4(1, 1, -1, -1) * 3.0;


				return o;
			}
			
			float4 frag (v2f i) : COLOR 
			{		
				float4 color = 0.28 * tex2D (_MainTex, i.uv);
				color += 0.22 * tex2D (_MainTex, i.uv01.xy);
				color += 0.22 * tex2D (_MainTex, i.uv01.zw);
				color += 0.10 * tex2D (_MainTex, i.uv23.xy);
				color += 0.10 * tex2D (_MainTex, i.uv23.zw);
				color += 0.04 * tex2D (_MainTex, i.uv45.xy);
				color += 0.04 * tex2D (_MainTex, i.uv45.zw);				
				return color * _Intensity;
			}

			ENDCG
		}		
	}
	Fallback off
}
