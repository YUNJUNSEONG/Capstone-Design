// 合併 MobileParticleAdditiveColor_Pro & MobileParticleAlphaBlendedColor_Pro
// BlendMode: Additive: Blend SrcAlpha One (1) / AlphaBlended: Blend SrcAlpha OneMinusSrcAlpha (10)

Shader "Custom/Particles/MobileParticleAddMultiColor"
{
	Properties
	{
		_MainTex("Particle Texture", 2D) = "white" {}

		_Color1 ("Color 1", Color) = (1, 1, 1, 1)

		_Color2 ("Color 2", Color) = (1, 1, 1, 1)

		[Enum(Additive, 1, AlphaBlended, 10)] _BlendMode ("BlendMode", Float) = 1

		[Enum(Off, 0, front, 1, back, 2)] _Cull ("Cull", Float) = 0
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha [_BlendMode]                         
		Cull [_Cull]
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog
			ENDCG
		}
	}

	CGINCLUDE

#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_ST;

	float4 _Color1;
	
	float4 _Color2;
	
	struct appdata_t
	{
		float4 position : POSITION;
		float4 texcoord : TEXCOORD0;
		fixed4 color : COLOR;
	};

	struct v2f
	{
		float4 position : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		fixed4 color : COLOR;
		UNITY_FOG_COORDS(1)
	};

	v2f vert(appdata_t v)
	{
		v2f o;

		o.position = UnityObjectToClipPos(v.position);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.color = v.color;

		UNITY_TRANSFER_FOG(o, o.position);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		half4 origin = tex2D(_MainTex, i.texcoord);

		half gray = (origin.x + origin.y + origin.z) / 3;
		
		half diff = 1 - gray;

		fixed4 color;

		color.r = gray * _Color1.r + diff * _Color2.r;
		color.g = gray * _Color1.g + diff * _Color2.g;
		color.b = gray * _Color1.b + diff * _Color2.b;
		color.a = gray * _Color1.a + diff * _Color2.a;

		color = i.color * origin * color;

		UNITY_APPLY_FOG_COLOR(i.fogCoord, color, (fixed4)0);

		return color;
	}

	ENDCG
}