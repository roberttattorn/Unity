Shader "Custom/IslandTiling_Blended" {
    Properties {
        _MainTex ("Texture Atlas", 2D) = "white" {}
        _IslandRect ("Island Rect (X, Y, Width, Height)", Vector) = (0, 0, 0.5, 0.5)
        _TileScale ("Tile Scale", Float) = 4.0
        _BlendStrength ("Edge Blend Strength", Range(0, 0.5)) = 0.1
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;
        float4 _IslandRect;
        float _TileScale;
        float _BlendStrength;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            // 1. Calculate the repeating 0-1 UVs
            float2 fracUV = frac(IN.uv_MainTex * _TileScale);
            
            // 2. Remap to the sub-region (Island)
            float2 finalUV = _IslandRect.xy + (fracUV * _IslandRect.zw);
            
            // 3. Create the Edge Blend Mask
            // We check distance from 0 and 1 on both axes
            float2 edgeMask = smoothstep(0, _BlendStrength, fracUV) * 
                              smoothstep(1, 1 - _BlendStrength, fracUV);
            float finalAlpha = edgeMask.x * edgeMask.y;

            fixed4 c = tex2D(_MainTex, finalUV);
            o.Albedo = c.rgb;
            o.Alpha = c.a * finalAlpha; // Multiply texture alpha by our edge mask
        }
        ENDCG
    }
    FallBack "Diffuse"
}

