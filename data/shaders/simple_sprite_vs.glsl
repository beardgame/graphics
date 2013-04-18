#version 130

uniform mat4 projectionMatrix;
uniform mat4 modelviewMatrix;

in vec3 v_position;
in vec2 v_texcoord;
in vec4 v_color;
in vec2 v_expand;

out vec4 p_color;
out vec2 p_texcoord;

void main()
{
    vec4 p = modelviewMatrix * vec4(v_position, 1.0);
	p += vec4(v_expand, 0.0, 0.0);
	gl_Position = projectionMatrix * p;
	p_color = v_color;
	p_texcoord = v_texcoord;
}