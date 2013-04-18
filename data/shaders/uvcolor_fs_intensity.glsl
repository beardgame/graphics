#version 130

uniform sampler2D diffuseTexture;
uniform float intensity = 1;

in vec4 p_color;
in vec2 p_texcoord;

out vec4 fragColor;

void main()
{
	vec4 c = p_color * texture(diffuseTexture, p_texcoord) * vec4(vec3(intensity), 1);
    fragColor = c;
}