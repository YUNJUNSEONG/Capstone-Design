// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "NDEffect/UVOffset_Mask_Add_2Side" {
Properties {
	[HDR] _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("MainTex (RGBA)", 2D) = "white" {}
	_NoiseTex ("MaskTex (A)", 2D) = "white" {}
	_RollTimeX ("Roll Time X", Float) = 0.2
	_RollTimeY ("Roll Time Y", Float) = 0
	//拓展---
	//[ToggleOff] _FOG("开启雾效", Float) = 1.0
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	Cull Off Lighting Off ZWrite Off
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	// ---- Fragment program cards
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_particles
			
			#include "UnityCG.cginc"

			//#pragma multi_compile _ _UIMASKCLIP _FOG_OFF
			#pragma multi_compile _ _UIMASKCLIP
			//#include "UIParticles/UIMaskEffect.cginc"
			//#include "Assets/gameres/common/Shader/Fog/Fog.cginc"

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float _RollTimeX;
			float _RollTimeY;
			fixed4 _TintColor;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texMain : TEXCOORD0;
				float2 texNoise : TEXCOORD1;
				float4 worldPos : TEXCOORD2;
				float3 worldVec : TEXCOORD3;
			};
			float4 _MainTex_ST;
			float4 _NoiseTex_ST;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texMain = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texNoise = TRANSFORM_TEX(v.texcoord, _NoiseTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldVec = o.worldPos - _WorldSpaceCameraPos;
				return o;
			}
			fixed4 frag (v2f i) : COLOR
			{
				half2 uvoft = i.texMain;
				uvoft.x += _Time.yx * _RollTimeX;
				uvoft.y += _Time.yx * _RollTimeY;
				fixed4 offsetColor = tex2D(_NoiseTex, i.texNoise);
				fixed4 mainColor = tex2D(_MainTex, uvoft);
				fixed4 col = 2.0 * i.color * _TintColor * mainColor * offsetColor.a;
				#ifdef _UIMASKCLIP
					col.a *= GetMaskClipping(i.worldPos);
					col.a = lerp(0.0, col.a, step(0.001, col.a));
				#endif
				// #if !defined(_FOG_OFF)
				// 	col.rgb = GetBlendFogColor(col, i.worldPos, i.worldVec);
				// #endif
				return col;
			}
			ENDCG
		}
	} 	
}
}