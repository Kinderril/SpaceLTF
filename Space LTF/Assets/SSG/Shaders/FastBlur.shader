Shader "p0/FastBlur" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlurTex ("Blur (RGB)", 2D) = "white" {}
		_Value ("Value", Range(0, 1)) = 0
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest noshadow noambient novertexlights nodymlightmap nodirlightmap nofog nometa noforwardadd
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _BlurTex;
			fixed _Value;


			struct v2f
			{
				fixed2 uv : TEXCOORD0;
				float4 pos : POSITION;
			};

			v2f vert(appdata_img i)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(i.vertex);
				o.uv = i.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : COLOR0
			{
				return lerp(tex2D(_MainTex, i.uv), tex2D(_BlurTex, i.uv), _Value);
			}
		
			ENDCG
		}
	}
}
