Shader "p0/Reflective/Bumped Specular SMap" 
{
	SubShader 
	{
		Tags { "RenderType"="Opaque" }

		Stencil 
		{
			Ref 3
			WriteMask 3
			Pass replace
		}
		
		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert 
		#pragma target 3.0

		struct Input 
		{
			float2 uv_MainTex;
			float3 worldRefl;
			float3 viewDir;
			float3 localNormal;
			float3 worldNormal; 

		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT (Input, o);
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Emission = 1;			
		}
		ENDCG
	}
}
