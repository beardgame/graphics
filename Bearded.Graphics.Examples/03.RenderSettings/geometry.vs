#version 150

// Here are the uniforms set by the IRenderSettings
uniform mat4 projection;
uniform mat4 view;

in vec3 v_position;
in vec4 v_color;

out vec3 p_position;
out vec4 p_color;

void main()
{
	gl_Position = projection * view * vec4(v_position, 1.0);
	p_position = v_position.xyz;
	p_color = v_color;
}
