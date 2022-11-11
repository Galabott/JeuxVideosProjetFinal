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

	int i = 5;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemy = (PackedScene)GD.Load("res://EnemyLvl1.tscn");
		Spawner = (Node2D)GetNode("Spawner");
		TimeSpawn = (Timer)GetNode("SpawnTimer");
		GetTree().CallGroup("Wave","ChangeWave");	
	}

	private void WaveCount(){
		GD.Print(" Enemies per wave = " + GlobalVariables.EnemiesPerWaves);
		GD.Print(" Wave = " + GlobalVariables.WaveCount);

		if(GlobalVariables.EnemiesPerWaves > 0){
			GlobalVariables.EnemiesPerWaves--;
		}
		if(GlobalVariables.EnemiesPerWaves == 0 && GlobalVariables.WaveCount != 3){
			i--;
			if(i == 0){
				GlobalVariables.WaveCount++;
				GlobalVariables.EnemiesPerWaves += random.Next(7, 15);
				GetTree().CallGroup("Wave","ChangeWave");
				i += 5;
			}
			else if(i > 0){
				GetTree().CallGroup("Wave","SetTextWave", "FEW SECS!");
			}

		}
		else if(GlobalVariables.WaveCount == 3 && GlobalVariables.EnemiesPerWaves == 0){
			GetTree().CallGroup("Wave","Win");	
		}
	}

	private void _on_SpawnTimer_timeout()
	{

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
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}



