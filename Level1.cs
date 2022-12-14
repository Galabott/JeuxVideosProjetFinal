using Godot;
using System;

public class Level1 : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	PackedScene enemy;
	
	System.Random random = new System.Random(); 
	Node2D Spawner;

	Timer TimeSpawn;
	AudioStreamPlayer music;

	int i = 5;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemy = (PackedScene)GD.Load("res://EnemyLvl1.tscn");
		Spawner = (Node2D)GetNode("Spawner");
		TimeSpawn = (Timer)GetNode("SpawnTimer");
		music = (AudioStreamPlayer)GetNode("AudioStreamPlayer");
		GetTree().CallGroup("Wave","ChangeWave");

		if(GlobalVariables.Muted == false){
			music.Play();
		}
	}
	
	private void MusicToggle(){
		if(GlobalVariables.Muted){
			music.Stop();
		}
		else{
			music.Play();
		}
	}

	private void WaveCount(){
		GD.Print(" Enemies per wave = " + GlobalVariables.EnemiesPerWaves);
		GD.Print(" Wave = " + GlobalVariables.WaveCount);

		if(GlobalVariables.EnemiesPerWaves > 0){
			GlobalVariables.EnemiesPerWaves--;
		} // && GlobalVariables.WaveCount != 3
		if(GlobalVariables.EnemiesPerWaves == 0 ){
			i--;
			if(i == 0){
				GlobalVariables.WaveCount++;
				GlobalVariables.EnemiesPerWaves += random.Next(7, 15) * GlobalVariables.WaveCount;
				GetTree().CallGroup("Wave","ChangeWave");
				GD.Print("B4 Time Spawn = " + TimeSpawn.WaitTime);
				if(TimeSpawn.WaitTime > 1){
					TimeSpawn.WaitTime -= 1;
					GD.Print("AftR Time Spawn = " + TimeSpawn.WaitTime);
				}
				i += 5;
			}
			else if(i > 0){
				GetTree().CallGroup("Wave","SetTextWave", "FEW SECS!");
			}

		}
		// else if(GlobalVariables.WaveCount == 3 && GlobalVariables.EnemiesPerWaves == 0){
		// 	GetTree().CallGroup("Wave","Win");	
		// }
	}

	private void _on_SpawnTimer_timeout()
	{
		if(GlobalVariables.Paused == false){
			GD.Print("TIMEOUT");

		if(GlobalVariables.EnemiesPerWaves > 0){
			
			Node2D enemy_instance = (Node2D)enemy.Instance();
			AddChild(enemy_instance);
			enemy_instance.Position = Spawner.Position;

			var nodes = GetTree().GetNodesInGroup("spawn"); 
			var node = nodes[random.Next(0, 3)]; 
			var nod = (Node2D)node;
			var position = nod.Position; 
			Spawner.Position = position;
		}

		WaveCount();
		}

		
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}



