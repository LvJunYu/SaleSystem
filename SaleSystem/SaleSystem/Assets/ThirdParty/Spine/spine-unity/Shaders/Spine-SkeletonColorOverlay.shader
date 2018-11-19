Shader "Spine/SkeletonColorOverlay" {
	Properties {
		[NoScaleOffset] _MainTex ("Main Texture", 2D) = "black" {}
		_BlendColor ("Blend Color", Color) = (0.5,0.5,0.5,1)
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
		Blend One OneMinusSrcAlpha
		Cull Off
		ZWrite Off
		Lighting Off

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed4 _BlendColor;

			struct VertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			struct VertexOutput {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			VertexOutput vert (VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv = v.uv;
				o.vertexColor = v.vertexColor;
				o.pos = UnityObjectToClipPos(v.vertex); // Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
				return o;
			}

            fixed OverlayBlendMode(fixed basePixel, fixed blendPixel) {
				if (basePixel < 0.5) {
					return (2.0 * basePixel * blendPixel);
				} else {
					return (1.0 - 2.0 * (1.0 - basePixel) * (1.0 - blendPixel));
				}
			}
			
            fixed4 frag(VertexOutput i) : SV_Target{
                fixed4 renderTex = tex2D(_MainTex, i.uv);
                clip(renderTex.a - 0.01);
				renderTex.r = OverlayBlendMode((renderTex.r / renderTex.a), _BlendColor.r) * renderTex.a;
				renderTex.g = OverlayBlendMode((renderTex.g / renderTex.a), _BlendColor.g) * renderTex.a;
				renderTex.b = OverlayBlendMode((renderTex.b / renderTex.a), _BlendColor.b) * renderTex.a;
				
                return renderTex;
            }
			ENDCG
		}
	}
	FallBack "Diffuse"
}
