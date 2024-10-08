﻿Shader "Custom/StencilFrontBlack" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }	
		Offset -40, -10	
		LOD 200
		Stencil
		{
		 Ref 1
		 Comp Always
		 Pass Replace
		}
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
