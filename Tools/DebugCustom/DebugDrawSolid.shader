Shader "Hidden/DebugDrawSolid"
{
    SubShader
    {
        Pass
        {
            ZTest Always
            Cull Off
            ZWrite On
            Fog { Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            BindChannels
            {
                Bind "Color", color
                Bind "Vertex", vertex
            }
        }

        Pass
        {
            ZTest Always
            Cull Off
            ZWrite On
            Fog { Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil
            {
                Ref 1
                Comp NotEqual
                Pass Keep
            }

            BindChannels
            {
                Bind "Color", color
                Bind "Vertex", vertex
            }
        }
    }
}
