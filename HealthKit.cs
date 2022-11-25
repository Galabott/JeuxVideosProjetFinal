using Godot;
using System;

public class HealthKit : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	  	var currentSprite = (AnimatedSprite)GetNode("AniSprite");
		currentSprite.Play("default");
  }

	private void _on_Area2D_area_entered(Area2D area)
	{
		if(area.IsInGroup("Player")){
			GetTree().CallGroup("Player","HealthKit");
		}
	}

	private void Consume(){
		QueueFree();
	}
}



