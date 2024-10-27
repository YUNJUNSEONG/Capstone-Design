// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:False,rpth:0,vtps:1,hqsc:True,nrmq:1,nrsp:0,vomd:1,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:6,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:1,qpre:4,rntp:5,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32740,y:33254,varname:node_2865,prsc:2|emission-5821-OUT,alpha-454-OUT,voffset-3014-OUT;n:type:ShaderForge.SFN_TexCoord,id:8211,x:32311,y:33598,varname:node_8211,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_RemapRange,id:3014,x:32475,y:33599,varname:node_3014,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-8211-UVOUT;n:type:ShaderForge.SFN_Color,id:5344,x:31782,y:32880,ptovrint:False,ptlb:color,ptin:_color,varname:node_5344,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5614187,c2:0.6902457,c3:0.8676471,c4:1;n:type:ShaderForge.SFN_Tex2d,id:5836,x:31815,y:33064,varname:node_5836,prsc:2,ntxv:0,isnm:False|UVIN-1377-UVOUT,TEX-3913-TEX;n:type:ShaderForge.SFN_Multiply,id:8538,x:32006,y:33103,varname:node_8538,prsc:2|A-5344-RGB,B-5836-RGB,C-5314-RGB;n:type:ShaderForge.SFN_TexCoord,id:5733,x:30639,y:33036,varname:node_5733,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_RemapRange,id:2406,x:30831,y:33030,varname:node_2406,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-5733-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:9001,x:31021,y:33031,varname:node_9001,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-2406-OUT;n:type:ShaderForge.SFN_ArcTan2,id:8835,x:31203,y:33034,varname:node_8835,prsc:2,attp:3|A-9001-G,B-9001-R;n:type:ShaderForge.SFN_Append,id:1422,x:31375,y:33044,varname:node_1422,prsc:2|A-8835-OUT,B-8835-OUT;n:type:ShaderForge.SFN_Rotator,id:1377,x:31583,y:32874,varname:node_1377,prsc:2|UVIN-1422-OUT,SPD-842-OUT;n:type:ShaderForge.SFN_TexCoord,id:6140,x:31181,y:33477,varname:node_6140,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Vector2,id:1034,x:31185,y:33657,varname:node_1034,prsc:2,v1:0.5,v2:0.5;n:type:ShaderForge.SFN_Distance,id:318,x:31378,y:33528,varname:node_318,prsc:2|A-6140-UVOUT,B-1034-OUT;n:type:ShaderForge.SFN_Power,id:3806,x:31747,y:33477,varname:node_3806,prsc:2|VAL-2082-OUT,EXP-1096-OUT;n:type:ShaderForge.SFN_Exp,id:1096,x:31455,y:33725,varname:node_1096,prsc:2,et:0|IN-7392-OUT;n:type:ShaderForge.SFN_Slider,id:7392,x:31007,y:33807,ptovrint:False,ptlb:fanwei,ptin:_fanwei,varname:node_7392,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.136752,max:8;n:type:ShaderForge.SFN_RemapRange,id:2082,x:31527,y:33514,varname:node_2082,prsc:2,frmn:0,frmx:0.5,tomn:0,tomx:0.8|IN-318-OUT;n:type:ShaderForge.SFN_Clamp01,id:5813,x:31938,y:33500,varname:node_5813,prsc:2|IN-3806-OUT;n:type:ShaderForge.SFN_Multiply,id:454,x:32125,y:33482,varname:node_454,prsc:2|A-5813-OUT,B-1649-OUT;n:type:ShaderForge.SFN_Slider,id:1649,x:31718,y:33679,ptovrint:False,ptlb:Alpha,ptin:_Alpha,varname:node_1649,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:10;n:type:ShaderForge.SFN_Tex2dAsset,id:3913,x:31588,y:33279,ptovrint:False,ptlb:tex,ptin:_tex,varname:node_3913,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5314,x:31816,y:33245,varname:node_5314,prsc:2,ntxv:0,isnm:False|UVIN-5562-UVOUT,TEX-3913-TEX;n:type:ShaderForge.SFN_Rotator,id:5562,x:31551,y:33076,varname:node_5562,prsc:2|UVIN-1422-OUT,SPD-5572-OUT;n:type:ShaderForge.SFN_Slider,id:842,x:31016,y:32914,ptovrint:False,ptlb:Rotatorspeed1,ptin:_Rotatorspeed1,varname:node_842,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:10;n:type:ShaderForge.SFN_Add,id:5821,x:32244,y:32879,varname:node_5821,prsc:2|A-5344-RGB,B-5836-RGB,C-5314-RGB,D-5344-A;n:type:ShaderForge.SFN_Slider,id:5572,x:30984,y:33381,ptovrint:False,ptlb:Rotatorspeed2,ptin:_Rotatorspeed2,varname:node_5572,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:10;proporder:5344-7392-1649-3913-842-5572;pass:END;sub:END;*/

Shader "Shader Forge/pmxg" {
    Properties {
        _color ("color", Color) = (0.5614187,0.6902457,0.8676471,1)
        _fanwei ("fanwei", Range(0, 8)) = 1.136752
        _Alpha ("Alpha", Range(0, 10)) = 0
        _tex ("tex", 2D) = "white" {}
        _Rotatorspeed1 ("Rotatorspeed1", Range(0, 10)) = 0.5
        _Rotatorspeed2 ("Rotatorspeed2", Range(0, 10)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay+1"
            "RenderType"="Overlay"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZTest Always
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float4 _color;
            uniform float _fanwei;
            uniform float _Alpha;
            uniform sampler2D _tex; uniform float4 _tex_ST;
            uniform float _Rotatorspeed1;
            uniform float _Rotatorspeed2;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                v.vertex.xyz = float3((o.uv0*2.0+-1.0),0.0);
                o.pos = v.vertex;
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_527 = _Time;
                float node_1377_ang = node_527.g;
                float node_1377_spd = _Rotatorspeed1;
                float node_1377_cos = cos(node_1377_spd*node_1377_ang);
                float node_1377_sin = sin(node_1377_spd*node_1377_ang);
                float2 node_1377_piv = float2(0.5,0.5);
                float2 node_9001 = (i.uv0*2.0+-1.0).rg;
                float node_8835 = (1-abs(atan2(node_9001.g,node_9001.r)/3.14159265359));
                float2 node_1422 = float2(node_8835,node_8835);
                float2 node_1377 = (mul(node_1422-node_1377_piv,float2x2( node_1377_cos, -node_1377_sin, node_1377_sin, node_1377_cos))+node_1377_piv);
                float4 node_5836 = tex2D(_tex,TRANSFORM_TEX(node_1377, _tex));
                float node_5562_ang = node_527.g;
                float node_5562_spd = _Rotatorspeed2;
                float node_5562_cos = cos(node_5562_spd*node_5562_ang);
                float node_5562_sin = sin(node_5562_spd*node_5562_ang);
                float2 node_5562_piv = float2(0.5,0.5);
                float2 node_5562 = (mul(node_1422-node_5562_piv,float2x2( node_5562_cos, -node_5562_sin, node_5562_sin, node_5562_cos))+node_5562_piv);
                float4 node_5314 = tex2D(_tex,TRANSFORM_TEX(node_5562, _tex));
                float3 emissive = (_color.rgb+node_5836.rgb+node_5314.rgb+_color.a);
                float3 finalColor = emissive;
                return fixed4(finalColor,(saturate(pow((distance(i.uv0,float2(0.5,0.5))*1.6+0.0),exp(_fanwei)))*_Alpha));
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
