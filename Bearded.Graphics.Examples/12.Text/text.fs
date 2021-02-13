#version 150

uniform sampler2D fontTexture;

in vec2 p_uv;

out vec4 fragColor;

void main()
{
    fragColor = texture(fontTexture, p_uv);
}
