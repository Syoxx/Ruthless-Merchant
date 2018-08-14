// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:34952,y:32678,varname:node_9361,prsc:2|emission-9045-OUT,custl-8700-OUT,voffset-7409-OUT,tess-8805-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:8068,x:33083,y:32945,varname:node_8068,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3406,x:33083,y:33068,varname:node_3406,prsc:2;n:type:ShaderForge.SFN_LightVector,id:6869,x:32903,y:32800,varname:node_6869,prsc:2;n:type:ShaderForge.SFN_Dot,id:7782,x:33083,y:32800,cmnt:Lambert,varname:node_7782,prsc:2,dt:1|A-6869-OUT,B-1785-OUT;n:type:ShaderForge.SFN_Tex2d,id:851,x:33871,y:32397,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:5927,x:33635,y:32443,ptovrint:False,ptlb:MaxColor,ptin:_MaxColor,varname:_MaxColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07097723,c2:0.26959,c3:0.5188679,c4:1;n:type:ShaderForge.SFN_Multiply,id:544,x:34072,y:32551,cmnt:Diffuse Color,varname:node_544,prsc:2|A-851-RGB,B-1869-OUT;n:type:ShaderForge.SFN_Multiply,id:3747,x:33283,y:32828,cmnt:Attenuate and Color,varname:node_3747,prsc:2|A-7782-OUT,B-8068-OUT,C-3406-RGB;n:type:ShaderForge.SFN_AmbientLight,id:2701,x:34072,y:32376,varname:node_2701,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3113,x:34334,y:32427,cmnt:Diffuse Color,varname:node_3113,prsc:2|A-1739-OUT,B-2701-RGB,C-544-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1739,x:34072,y:32279,ptovrint:False,ptlb:Ambient Multiplier,ptin:_AmbientMultiplier,varname:_AmbientMultiplier,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_NormalVector,id:1785,x:32903,y:32935,prsc:2,pt:False;n:type:ShaderForge.SFN_Time,id:8293,x:31624,y:33432,varname:node_8293,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1625,x:34534,y:32836,varname:node_1625,prsc:2|A-9301-OUT,B-8621-OUT;n:type:ShaderForge.SFN_Slider,id:8621,x:34174,y:32973,ptovrint:False,ptlb:WaveStrength,ptin:_WaveStrength,varname:_WaveStrength,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ValueProperty,id:5642,x:34534,y:33364,ptovrint:False,ptlb:Tessellation,ptin:_Tessellation,varname:_Tessellation,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Max,id:8805,x:34708,y:33226,varname:node_8805,prsc:2|A-2418-OUT,B-5642-OUT;n:type:ShaderForge.SFN_Vector1,id:2418,x:34534,y:33270,varname:node_2418,prsc:2,v1:1;n:type:ShaderForge.SFN_Add,id:9045,x:34691,y:32299,varname:node_9045,prsc:2|A-5935-OUT,B-3113-OUT;n:type:ShaderForge.SFN_Fresnel,id:6620,x:34334,y:32188,varname:node_6620,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:3206,x:34334,y:32343,ptovrint:False,ptlb:Fresnel,ptin:_Fresnel,varname:_Fresnel,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Power,id:5935,x:34513,y:32188,varname:node_5935,prsc:2|VAL-6620-OUT,EXP-3206-OUT;n:type:ShaderForge.SFN_NormalVector,id:8905,x:34534,y:32974,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:7409,x:34708,y:33083,varname:node_7409,prsc:2|A-1625-OUT,B-8905-OUT,C-6501-OUT;n:type:ShaderForge.SFN_Sin,id:3152,x:32165,y:33239,varname:node_3152,prsc:2|IN-130-OUT;n:type:ShaderForge.SFN_Multiply,id:7754,x:31815,y:33239,varname:node_7754,prsc:2|A-3363-R,B-7665-OUT,C-8777-OUT;n:type:ShaderForge.SFN_Pi,id:7665,x:31655,y:33260,varname:node_7665,prsc:2;n:type:ShaderForge.SFN_Vector1,id:8777,x:31622,y:33374,varname:node_8777,prsc:2,v1:4;n:type:ShaderForge.SFN_Color,id:2956,x:33635,y:32263,ptovrint:False,ptlb:MinColor,ptin:_MinColor,varname:_MinColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.1479619,c2:0.8962264,c3:0.8394677,c4:1;n:type:ShaderForge.SFN_Lerp,id:1869,x:33871,y:32570,varname:node_1869,prsc:2|A-2956-RGB,B-5927-RGB,T-7431-OUT;n:type:ShaderForge.SFN_Multiply,id:8700,x:34334,y:32643,cmnt:Diffuse Color,varname:node_8700,prsc:2|A-544-OUT,B-3747-OUT;n:type:ShaderForge.SFN_Multiply,id:6113,x:31815,y:33375,varname:node_6113,prsc:2|A-8293-T,B-8266-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8266,x:31624,y:33595,ptovrint:False,ptlb:Wave_speed_1,ptin:_Wave_speed_1,varname:_Wave_speed_1,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:3363,x:31471,y:33195,varname:_NoiseTex1,prsc:2,tex:2d657126723b13d4c8e7ffd3076133b0,ntxv:0,isnm:False|TEX-2902-TEX;n:type:ShaderForge.SFN_Add,id:9301,x:32508,y:33239,varname:node_9301,prsc:2|A-8170-OUT,B-4138-OUT;n:type:ShaderForge.SFN_Vector1,id:7742,x:32165,y:33365,varname:node_7742,prsc:2,v1:2;n:type:ShaderForge.SFN_Divide,id:8170,x:32337,y:33239,varname:node_8170,prsc:2|A-3152-OUT,B-7742-OUT;n:type:ShaderForge.SFN_Vector1,id:4138,x:32337,y:33365,varname:node_4138,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Clamp01,id:5396,x:33033,y:32620,varname:node_5396,prsc:2|IN-3419-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8605,x:33289,y:32579,ptovrint:False,ptlb:Posterize_Number,ptin:_Posterize_Number,varname:_Posterize_Number,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Posterize,id:1139,x:33444,y:32453,varname:node_1139,prsc:2|IN-5396-OUT,STPS-8605-OUT;n:type:ShaderForge.SFN_Slider,id:5112,x:33231,y:32712,ptovrint:False,ptlb:Posterize-Effect,ptin:_PosterizeEffect,varname:_PosterizeEffect,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Lerp,id:7431,x:33635,y:32603,varname:node_7431,prsc:2|A-1139-OUT,B-5396-OUT,T-5112-OUT;n:type:ShaderForge.SFN_Add,id:130,x:31992,y:33239,varname:node_130,prsc:2|A-7754-OUT,B-6113-OUT;n:type:ShaderForge.SFN_Clamp01,id:5879,x:33471,y:32807,varname:node_5879,prsc:2|IN-3747-OUT;n:type:ShaderForge.SFN_NormalVector,id:7009,x:34012,y:33086,prsc:2,pt:False;n:type:ShaderForge.SFN_ComponentMask,id:986,x:34184,y:33086,varname:node_986,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-7009-OUT;n:type:ShaderForge.SFN_Abs,id:12,x:34352,y:33086,varname:node_12,prsc:2|IN-986-OUT;n:type:ShaderForge.SFN_Power,id:6501,x:34534,y:33129,varname:node_6501,prsc:2|VAL-12-OUT,EXP-1964-OUT;n:type:ShaderForge.SFN_Vector1,id:1964,x:34352,y:33233,varname:node_1964,prsc:2,v1:2;n:type:ShaderForge.SFN_Tex2d,id:6246,x:31622,y:32702,varname:_NoiseTex2,prsc:2,tex:2d657126723b13d4c8e7ffd3076133b0,ntxv:0,isnm:False|UVIN-1527-OUT,TEX-2902-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:2902,x:31231,y:33248,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:_Noise,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2d657126723b13d4c8e7ffd3076133b0,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:5117,x:31093,y:32681,varname:node_5117,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:2543,x:31093,y:32843,varname:node_2543,prsc:2;n:type:ShaderForge.SFN_Append,id:1527,x:31431,y:32702,varname:node_1527,prsc:2|A-5117-U,B-3180-OUT;n:type:ShaderForge.SFN_Add,id:3180,x:31279,y:32804,varname:node_3180,prsc:2|A-5117-V,B-2543-TSL;n:type:ShaderForge.SFN_Time,id:7015,x:31624,y:33017,varname:node_7015,prsc:2;n:type:ShaderForge.SFN_Sin,id:3226,x:32165,y:33017,varname:node_3226,prsc:2|IN-222-OUT;n:type:ShaderForge.SFN_Multiply,id:5903,x:31815,y:32939,varname:node_5903,prsc:2|A-6246-R,B-1644-OUT,C-4906-OUT;n:type:ShaderForge.SFN_Pi,id:1644,x:31655,y:32835,varname:node_1644,prsc:2;n:type:ShaderForge.SFN_Vector1,id:4906,x:31622,y:32959,varname:node_4906,prsc:2,v1:4;n:type:ShaderForge.SFN_Multiply,id:9372,x:31815,y:33075,varname:node_9372,prsc:2|A-7015-T,B-2542-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2542,x:31624,y:33180,ptovrint:False,ptlb:Wave_speed_2,ptin:_Wave_speed_2,varname:_Wave_speed_2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Add,id:3503,x:32508,y:33017,varname:node_3503,prsc:2|A-3577-OUT,B-9665-OUT;n:type:ShaderForge.SFN_Vector1,id:1902,x:32165,y:33143,varname:node_1902,prsc:2,v1:2;n:type:ShaderForge.SFN_Divide,id:3577,x:32337,y:33017,varname:node_3577,prsc:2|A-3226-OUT,B-1902-OUT;n:type:ShaderForge.SFN_Vector1,id:9665,x:32337,y:33143,varname:node_9665,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Add,id:222,x:31992,y:33017,varname:node_222,prsc:2|A-5903-OUT,B-9372-OUT;n:type:ShaderForge.SFN_Lerp,id:3419,x:32863,y:32620,varname:node_3419,prsc:2|A-9301-OUT,B-3503-OUT,T-4502-OUT;n:type:ShaderForge.SFN_NormalVector,id:3611,x:31989,y:33429,prsc:2,pt:False;n:type:ShaderForge.SFN_Abs,id:4899,x:32342,y:33441,varname:node_4899,prsc:2|IN-4005-OUT;n:type:ShaderForge.SFN_ComponentMask,id:4005,x:32151,y:33441,varname:node_4005,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-3611-OUT;n:type:ShaderForge.SFN_OneMinus,id:4502,x:32549,y:33441,varname:node_4502,prsc:2|IN-4899-OUT;proporder:851-5927-1739-8621-5642-3206-2956-8266-8605-5112-2902-2542;pass:END;sub:END;*/

Shader "Shader Forge/Water" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _MaxColor ("MaxColor", Color) = (0.07097723,0.26959,0.5188679,1)
        _AmbientMultiplier ("Ambient Multiplier", Float ) = 1
        _WaveStrength ("WaveStrength", Range(0, 1)) = 0
        _Tessellation ("Tessellation", Float ) = 1
        _Fresnel ("Fresnel", Float ) = 0
        _MinColor ("MinColor", Color) = (0.1479619,0.8962264,0.8394677,1)
        _Wave_speed_1 ("Wave_speed_1", Float ) = 1
        _Posterize_Number ("Posterize_Number", Float ) = 2
        _PosterizeEffect ("Posterize-Effect", Range(0, 1)) = 1
        _Noise ("Noise", 2D) = "white" {}
        _Wave_speed_2 ("Wave_speed_2", Float ) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _MaxColor;
            uniform float _AmbientMultiplier;
            uniform float _WaveStrength;
            uniform float _Tessellation;
            uniform float _Fresnel;
            uniform float4 _MinColor;
            uniform float _Wave_speed_1;
            uniform float _Posterize_Number;
            uniform float _PosterizeEffect;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Wave_speed_2;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 _NoiseTex1 = tex2Dlod(_Noise,float4(TRANSFORM_TEX(o.uv0, _Noise),0.0,0));
                float4 node_8293 = _Time;
                float node_9301 = ((sin(((_NoiseTex1.r*3.141592654*4.0)+(node_8293.g*_Wave_speed_1)))/2.0)+0.5);
                v.vertex.xyz += ((node_9301*_WaveStrength)*v.normal*pow(abs(v.normal.g),2.0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return max(1.0,_Tessellation);
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
////// Emissive:
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float4 _NoiseTex1 = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float4 node_8293 = _Time;
                float node_9301 = ((sin(((_NoiseTex1.r*3.141592654*4.0)+(node_8293.g*_Wave_speed_1)))/2.0)+0.5);
                float4 node_2543 = _Time;
                float2 node_1527 = float2(i.uv0.r,(i.uv0.g+node_2543.r));
                float4 _NoiseTex2 = tex2D(_Noise,TRANSFORM_TEX(node_1527, _Noise));
                float4 node_7015 = _Time;
                float node_5396 = saturate(lerp(node_9301,((sin(((_NoiseTex2.r*3.141592654*4.0)+(node_7015.g*_Wave_speed_2)))/2.0)+0.5),(1.0 - abs(i.normalDir.g))));
                float3 node_544 = (_Diffuse_var.rgb*lerp(_MinColor.rgb,_MaxColor.rgb,lerp(floor(node_5396 * _Posterize_Number) / (_Posterize_Number - 1),node_5396,_PosterizeEffect))); // Diffuse Color
                float3 emissive = (pow((1.0-max(0,dot(normalDirection, viewDirection))),_Fresnel)+(_AmbientMultiplier*UNITY_LIGHTMODEL_AMBIENT.rgb*node_544));
                float3 node_3747 = (max(0,dot(lightDirection,i.normalDir))*attenuation*_LightColor0.rgb); // Attenuate and Color
                float3 finalColor = emissive + (node_544*node_3747);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _MaxColor;
            uniform float _AmbientMultiplier;
            uniform float _WaveStrength;
            uniform float _Tessellation;
            uniform float _Fresnel;
            uniform float4 _MinColor;
            uniform float _Wave_speed_1;
            uniform float _Posterize_Number;
            uniform float _PosterizeEffect;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Wave_speed_2;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 _NoiseTex1 = tex2Dlod(_Noise,float4(TRANSFORM_TEX(o.uv0, _Noise),0.0,0));
                float4 node_8293 = _Time;
                float node_9301 = ((sin(((_NoiseTex1.r*3.141592654*4.0)+(node_8293.g*_Wave_speed_1)))/2.0)+0.5);
                v.vertex.xyz += ((node_9301*_WaveStrength)*v.normal*pow(abs(v.normal.g),2.0));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return max(1.0,_Tessellation);
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float4 _NoiseTex1 = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float4 node_8293 = _Time;
                float node_9301 = ((sin(((_NoiseTex1.r*3.141592654*4.0)+(node_8293.g*_Wave_speed_1)))/2.0)+0.5);
                float4 node_2543 = _Time;
                float2 node_1527 = float2(i.uv0.r,(i.uv0.g+node_2543.r));
                float4 _NoiseTex2 = tex2D(_Noise,TRANSFORM_TEX(node_1527, _Noise));
                float4 node_7015 = _Time;
                float node_5396 = saturate(lerp(node_9301,((sin(((_NoiseTex2.r*3.141592654*4.0)+(node_7015.g*_Wave_speed_2)))/2.0)+0.5),(1.0 - abs(i.normalDir.g))));
                float3 node_544 = (_Diffuse_var.rgb*lerp(_MinColor.rgb,_MaxColor.rgb,lerp(floor(node_5396 * _Posterize_Number) / (_Posterize_Number - 1),node_5396,_PosterizeEffect))); // Diffuse Color
                float3 node_3747 = (max(0,dot(lightDirection,i.normalDir))*attenuation*_LightColor0.rgb); // Attenuate and Color
                float3 finalColor = (node_544*node_3747);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform float _WaveStrength;
            uniform float _Tessellation;
            uniform float _Wave_speed_1;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 _NoiseTex1 = tex2Dlod(_Noise,float4(TRANSFORM_TEX(o.uv0, _Noise),0.0,0));
                float4 node_8293 = _Time;
                float node_9301 = ((sin(((_NoiseTex1.r*3.141592654*4.0)+(node_8293.g*_Wave_speed_1)))/2.0)+0.5);
                v.vertex.xyz += ((node_9301*_WaveStrength)*v.normal*pow(abs(v.normal.g),2.0));
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return max(1.0,_Tessellation);
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
