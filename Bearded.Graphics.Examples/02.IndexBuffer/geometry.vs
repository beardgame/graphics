#version 150

in vec3 v_position;
in vec4 v_color;

out vec4 p_color;

void main()
{
	gl_Position = vec4(v_position, 1);
	p_color = v_color;
}
