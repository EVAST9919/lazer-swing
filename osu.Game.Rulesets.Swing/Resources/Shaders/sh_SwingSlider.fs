﻿#define HIGH_PRECISION_VERTEX

#define TWO_PI 6.28318530718
#define HALF_PI 1.57079632679

#include "sh_Utils.h"
#include "sh_Masking.h"

varying highp vec2 v_TexCoord;

uniform lowp vec4 accent;
uniform mediump float headAngle;
uniform mediump float tailAngle;
uniform highp float texelSize;

const float playfield_size = 512.0;
const float circle_size = 25.0;
const float shadow_size = 7.0;

void main(void)
{
    highp vec2 pixelPos = v_TexCoord / (v_TexRect.zw - v_TexRect.xy);
    
    mediump float pixelAngle = atan(0.5 - pixelPos.y, 0.5 - pixelPos.x) - HALF_PI;
    pixelAngle += float(pixelAngle < 0.0) * TWO_PI;

    highp float dst;
    
    highp float sprite_size = playfield_size + circle_size + shadow_size * 2.0;
    highp float pathRadius = (circle_size * 0.5 + shadow_size) / sprite_size;
    highp float arcDstFromEdge = 0.5 - pathRadius;

    if (pixelAngle > tailAngle && pixelAngle < headAngle)
    {
        dst = abs(distance(pixelPos, vec2(0.5)) - arcDstFromEdge);
    }
    else
    {
        highp vec2 arcStart = vec2(0.5) + vec2(cos(headAngle - HALF_PI), sin(headAngle - HALF_PI)) * vec2(arcDstFromEdge);
        highp vec2 arcEnd = vec2(0.5) + vec2(cos(tailAngle - HALF_PI), sin(tailAngle - HALF_PI)) * vec2(arcDstFromEdge);

        dst = min(distance(pixelPos, arcStart), distance(pixelPos, arcEnd));
    }

    highp float textureDepth = dst / pathRadius;

    if (textureDepth > 1.0)
    {
        gl_FragColor = vec4(0.0);
        return;
    }

    lowp vec4 col;

    if (textureDepth > 1.0 - (shadow_size / sprite_size / pathRadius))
    {
        col = mix(vec4(vec3(0.0), 0.25), vec4(0.0), smoothstep(1.0 - (shadow_size / sprite_size / pathRadius), 1.0, textureDepth));
    }
    else if (textureDepth > 1.0 - (shadow_size / sprite_size / pathRadius) - texelSize / pathRadius)
    {
        col = mix(vec4(1.0), vec4(vec3(0.0), 0.25), smoothstep(1.0 - (shadow_size / sprite_size / pathRadius) - texelSize / pathRadius, 1.0 - (shadow_size / sprite_size / pathRadius), textureDepth));
    }
    else if (textureDepth > 0.5)
    {
        col = vec4(1.0);
    }
    else if (textureDepth > 0.5 - texelSize / pathRadius)
    {
        col = mix(vec4(1.0), vec4(vec3(accent.rgb * 0.5), 1.0), smoothstep(0.5, 0.5 - texelSize / pathRadius, textureDepth));
    }
    else if (textureDepth > 0.5 - (shadow_size / sprite_size / pathRadius))
    {
        col = mix(vec4(vec3(accent.rgb * 0.5), 1.0), accent, smoothstep(0.5, 0.5 - (shadow_size / sprite_size / pathRadius), textureDepth));
    }
    else
    {
        col = accent;
    }
    
    gl_FragColor = getRoundedColor(col, v_TexCoord);
}