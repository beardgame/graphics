#version 130

uniform sampler2D diffuseTexture;

in vec4 p_color;
in vec2 p_texcoord;

out vec4 fragColor;

void main()
{
	vec4 c = p_color * texture(diffuseTexture, p_texcoord);
    fragColor = c;
}