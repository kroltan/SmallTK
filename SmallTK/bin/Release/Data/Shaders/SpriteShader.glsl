-- Version
#version 330



-- Vertex
#include SpriteShader.Version
in vec3 InPosition;
in vec2 InTexCoord;

smooth out vec2 TexCoord;

uniform mat4 ModelViewProjectionMatrix;

void main() {
	// transform vertex position
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition,1);
	// pass through texture coordinate
	TexCoord = InTexCoord;
}



-- Fragment
#include SpriteShader.Version

smooth in vec2 TexCoord;

out vec4 FragColor;

uniform sampler2D Texture;

void main() {
	vec4 texColor = texture(Texture, TexCoord);
	FragColor = vec4(texColor.rgb, texColor.a);
}