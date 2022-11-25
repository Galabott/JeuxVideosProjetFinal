using Godot;
using System;

public static class GlobalVariables 
{
	public static Vector2 PlayerPosition = Vector2.Zero;
	
	public static int PlayerScore = 0;

	public static int WaveCount = 1;

	public static int EnemiesPerWaves = 5;
	
	public static bool Paused = false;
	public static bool Muted = true;


	public static void Reset(){
		PlayerPosition = Vector2.Zero;
	
		PlayerScore = 0;

		WaveCount = 1;

		EnemiesPerWaves = 5;
	}
}
