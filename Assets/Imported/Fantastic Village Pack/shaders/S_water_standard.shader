// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TidalFlask/Water Standard"
{
	Properties
	{
		[HDR]_BaseColor("Base Color", Color) = (0.2122642,0.529507,0.8490566,0)
		_Gloss("Gloss", Float) = 1.5
		_Opacity("Opacity", Range( 0 , 1)) = 0.95
		[HDR]_RipplesEmission("Ripples Emission", Color) = (1,1,1,0)
		_RippleScale("Ripple Scale", Float) = 5
		_RippleDissolve("Ripple Dissolve", Float) = 3.5
		_RippleSpeed("Ripple Speed", Float) = 1
		_RipplesTransparency("Ripples Transparency", Float) = 5
		_Normal1Speed("Normal 1 Speed", Vector) = (0.02,-0.02,0,0)
		_Normal2Speed("Normal 2 Speed", Vector) = (0.04,-0.01,0,0)
		_Normal1Size("Normal 1 Size", Float) = 0.1
		_Normal2Size("Normal 2 Size", Float) = 0.15
		_NormalStrength("Normal Strength", Float) = 0.5
		[HDR]_FoamColor("Foam Color", Color) = (1,1,1,0)
		_FoamDistance("Foam Distance", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			half3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform half _Normal1Size;
		uniform half2 _Normal1Speed;
		uniform half _Normal2Size;
		uniform half2 _Normal2Speed;
		uniform half _NormalStrength;
		uniform half4 _BaseColor;
		uniform half _RippleScale;
		uniform half _RippleSpeed;
		uniform half _RippleDissolve;
		uniform half4 _RipplesEmission;
		uniform half _RipplesTransparency;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform half _FoamDistance;
		uniform half4 _FoamColor;
		uniform half _Gloss;
		uniform half _Opacity;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		half3 PerturbNormal107_g8( half3 surf_pos, half3 surf_norm, half height, half scale )
		{
			// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
			float3 vSigmaS = ddx( surf_pos );
			float3 vSigmaT = ddy( surf_pos );
			float3 vN = surf_norm;
			float3 vR1 = cross( vSigmaT , vN );
			float3 vR2 = cross( vN , vSigmaS );
			float fDet = dot( vSigmaS , vR1 );
			float dBs = ddx( height );
			float dBt = ddy( height );
			float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
			return normalize ( abs( fDet ) * vN - vSurfGrad );
		}


		float2 voronoihash8( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi8( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash8( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			
			 		}
			 	}
			}
			return F1;
		}


		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float3 ase_worldPos = i.worldPos;
			half3 surf_pos107_g8 = ase_worldPos;
			half3 ase_worldNormal = WorldNormalVector( i, half3( 0, 0, 1 ) );
			half3 surf_norm107_g8 = ase_worldNormal;
			half3 break101 = ase_worldPos;
			half4 appendResult102 = (half4(break101.x , break101.z , 0.0 , 0.0));
			half4 temp_output_103_0 = ( 1.0 - appendResult102 );
			half simplePerlin2D40 = snoise( (temp_output_103_0*_Normal1Size + half4( ( _Time.y * _Normal1Speed ), 0.0 , 0.0 )).xy*10.0 );
			simplePerlin2D40 = simplePerlin2D40*0.5 + 0.5;
			half simplePerlin2D155 = snoise( (temp_output_103_0*_Normal2Size + half4( ( _Time.y * _Normal2Speed ), 0.0 , 0.0 )).xy*10.0 );
			simplePerlin2D155 = simplePerlin2D155*0.5 + 0.5;
			half height107_g8 = ( simplePerlin2D40 + simplePerlin2D155 );
			half scale107_g8 = _NormalStrength;
			half3 localPerturbNormal107_g8 = PerturbNormal107_g8( surf_pos107_g8 , surf_norm107_g8 , height107_g8 , scale107_g8 );
			half3 ase_worldTangent = WorldNormalVector( i, half3( 1, 0, 0 ) );
			half3 ase_worldBitangent = WorldNormalVector( i, half3( 0, 1, 0 ) );
			half3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			half3 worldToTangentDir42_g8 = mul( ase_worldToTangent, localPerturbNormal107_g8);
			half3 Normals181 = worldToTangentDir42_g8;
			o.Normal = Normals181;
			o.Albedo = _BaseColor.rgb;
			half4 color33 = IsGammaSpace() ? half4(1,1,1,0) : half4(1,1,1,0);
			half time8 = ( _Time.y * _RippleSpeed );
			half2 voronoiSmoothId0 = 0;
			half3 break2 = ase_worldPos;
			half4 appendResult3 = (half4(break2.x , break2.z , 0.0 , 0.0));
			float2 coords8 = ( appendResult3 / 2 ).xy * _RippleScale;
			float2 id8 = 0;
			float2 uv8 = 0;
			float voroi8 = voronoi8( coords8, time8, id8, uv8, 0, voronoiSmoothId0 );
			half4 temp_output_15_0 = ( pow( voroi8 , _RippleDissolve ) * _RipplesEmission );
			half2 _Vector1 = half2(0,1);
			half2 _Vector2 = half2(0.2,3);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			half4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth113 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			half distanceDepth113 = saturate( abs( ( screenDepth113 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamDistance ) ) );
			half4 Foam179 = ( ( 1.0 - distanceDepth113 ) * _FoamColor );
			half4 lerpResult116 = lerp( ( color33 * ( temp_output_15_0 + ( (_Vector1.x + (temp_output_15_0.r - _Vector1.x) * (_Vector2.x - _Vector1.x) / (_Vector2.x - _Vector1.x)) * _RipplesTransparency ) ) ) , Foam179 , Foam179);
			o.Emission = lerpResult116.rgb;
			o.Smoothness = ( 1.0 - _Gloss );
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular alpha:fade keepalpha fullforwardshadows noshadow 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 screenPos : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18909
502;259;1906;822;1506.652;478.1727;1.62654;True;False
Node;AmplifyShaderEditor.CommentaryNode;183;-4055.012,-230.8764;Inherit;False;4070.822;860.4115;;22;32;33;30;28;29;25;27;26;24;15;13;17;14;8;9;10;4;7;11;3;2;1;Ripples;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;1;-3939.85,51.36409;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;2;-3717.405,48.19258;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;121;-3872.534,-1585.548;Inherit;False;3096.085;1205.494;;18;181;42;159;45;155;40;154;39;60;152;103;37;153;102;151;38;101;99;Normals;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-3487.249,-121.4767;Inherit;False;Property;_RippleSpeed;Ripple Speed;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;3;-3547.261,42.39609;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;7;-3495.662,263.9961;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;0;False;0;False;2,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;12;-3669.258,-329.9565;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-3235.695,433.3276;Inherit;False;Property;_RippleScale;Ripple Scale;4;0;Create;True;0;0;0;False;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-3227.249,-138.4767;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;99;-3647.169,-1429.552;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;4;-3263.261,42.39609;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VoronoiNode;8;-2958.66,39.94944;Inherit;True;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.BreakToComponentsNode;101;-3435.724,-1429.723;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;14;-2774.531,184.9309;Inherit;False;Property;_RippleDissolve;Ripple Dissolve;5;0;Create;True;0;0;0;False;0;False;3.5;3.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;120;-1280.819,756.2426;Inherit;False;1184.583;447.6348;;6;179;114;113;59;58;54;Foam;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;17;-2290.162,119.0962;Inherit;False;Property;_RipplesEmission;Ripples Emission;3;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;13;-2552.531,35.93094;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;102;-3262.076,-1418.024;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;151;-3327.431,-559.3152;Inherit;False;Property;_Normal2Speed;Normal 2 Speed;9;0;Create;True;0;0;0;False;0;False;0.04,-0.01;-0.04,0.01;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;38;-3330.78,-963.1083;Inherit;False;Property;_Normal1Speed;Normal 1 Speed;8;0;Create;True;0;0;0;False;0;False;0.02,-0.02;0.02,-0.04;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;114;-1252.298,956.6044;Inherit;False;Property;_FoamDistance;Foam Distance;14;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;103;-3021.385,-1419.061;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-3030.638,-1192.975;Inherit;False;Property;_Normal1Size;Normal 1 Size;10;0;Create;True;0;0;0;False;0;False;0.1;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;-3026.288,-790.1819;Inherit;False;Property;_Normal2Size;Normal 2 Size;11;0;Create;True;0;0;0;False;0;False;0.15;0.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-3038.55,-1095.457;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-2046.116,37.18529;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;-3035.2,-691.6639;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DepthFade;113;-1040.261,838.6234;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;2.79;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;24;-1795.988,196.1695;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ScaleAndOffsetNode;154;-2745.914,-738.6368;Inherit;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;39;-2749.264,-1142.43;Inherit;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;26;-1656.988,267.1694;Inherit;False;Constant;_Vector1;Vector 1;4;0;Create;True;0;0;0;False;0;False;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;27;-1664.988,404.1694;Inherit;False;Constant;_Vector2;Vector 2;4;0;Create;True;0;0;0;False;0;False;0.2,3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;29;-1368.989,426.0694;Inherit;False;Property;_RipplesTransparency;Ripples Transparency;7;0;Create;True;0;0;0;False;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;40;-2415.498,-1150.898;Inherit;True;Simplex2D;True;True;2;0;FLOAT2;0,0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;155;-2406.092,-745.0862;Inherit;True;Simplex2D;True;True;2;0;FLOAT2;0,0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;25;-1464.988,198.1695;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;59;-812.6868,993.3473;Inherit;False;Property;_FoamColor;Foam Color;13;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;54;-779.1078,854.1481;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1996.47,-857.2809;Inherit;False;Property;_NormalStrength;Normal Strength;12;0;Create;True;0;0;0;False;0;False;0.5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;159;-2046.245,-1110.08;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1092.988,196.1695;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-555.2432,914.5452;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;42;-1744.302,-1110.773;Inherit;True;Normal From Height;-1;;8;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;0.5;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;33;-767.8588,-153.3785;Inherit;False;Constant;_EmissionIntensity;Emission Intensity;9;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;179;-332.9772,908.7869;Inherit;False;Foam;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-654.9292,36.83425;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;35;340.6329,244.6005;Inherit;False;Property;_Gloss;Gloss;1;0;Create;True;0;0;0;False;0;False;1.5;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;180;144.2289,85.54151;Inherit;False;179;Foam;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;181;-1056.534,-1117.107;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-212.9279,16.04753;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;119;378.9134,382.9292;Inherit;False;Property;_Opacity;Opacity;2;0;Create;True;0;0;0;False;0;False;0.95;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;393.1779,-302.1492;Inherit;False;Property;_BaseColor;Base Color;0;1;[HDR];Create;True;0;0;0;False;0;False;0.2122642,0.529507,0.8490566,0;0.2122642,0.529507,0.8490566,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;97;497.0473,250.1637;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;116;372.0543,15.41222;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;182;451.7447,-80.70496;Inherit;False;181;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;760.4822,-29.10124;Half;False;True;-1;2;;0;0;StandardSpecular;TidalFlask/Water Standard;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;4;False;-1;5;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;3;0;2;0
WireConnection;3;1;2;2
WireConnection;9;0;12;0
WireConnection;9;1;11;0
WireConnection;4;0;3;0
WireConnection;4;1;7;1
WireConnection;8;0;4;0
WireConnection;8;1;9;0
WireConnection;8;2;10;0
WireConnection;101;0;99;0
WireConnection;13;0;8;0
WireConnection;13;1;14;0
WireConnection;102;0;101;0
WireConnection;102;1;101;2
WireConnection;103;0;102;0
WireConnection;37;0;12;0
WireConnection;37;1;38;0
WireConnection;15;0;13;0
WireConnection;15;1;17;0
WireConnection;152;0;12;0
WireConnection;152;1;151;0
WireConnection;113;0;114;0
WireConnection;24;0;15;0
WireConnection;154;0;103;0
WireConnection;154;1;153;0
WireConnection;154;2;152;0
WireConnection;39;0;103;0
WireConnection;39;1;60;0
WireConnection;39;2;37;0
WireConnection;40;0;39;0
WireConnection;155;0;154;0
WireConnection;25;0;24;0
WireConnection;25;1;26;0
WireConnection;25;2;27;0
WireConnection;25;3;26;0
WireConnection;25;4;27;0
WireConnection;54;0;113;0
WireConnection;159;0;40;0
WireConnection;159;1;155;0
WireConnection;28;0;25;0
WireConnection;28;1;29;0
WireConnection;58;0;54;0
WireConnection;58;1;59;0
WireConnection;42;20;159;0
WireConnection;42;110;45;0
WireConnection;179;0;58;0
WireConnection;30;0;15;0
WireConnection;30;1;28;0
WireConnection;181;0;42;40
WireConnection;32;0;33;0
WireConnection;32;1;30;0
WireConnection;97;0;35;0
WireConnection;116;0;32;0
WireConnection;116;1;180;0
WireConnection;116;2;180;0
WireConnection;0;0;36;0
WireConnection;0;1;182;0
WireConnection;0;2;116;0
WireConnection;0;4;97;0
WireConnection;0;9;119;0
ASEEND*/
//CHKSM=6CF409B1413ED42EB29254776C561B0BB862C41F