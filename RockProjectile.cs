using Godot;
using System;

public class RockProjectile : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.

	Vector2 input_vector = Vector2.Zero;
	Vector2 Velocity = Vector2.Zero;

	const int ACCELERATION = 100;
	const int FRICTION = 90;

	float MAX_SPEED = 175;

	Timer timer = new Timer();
	
	AnimatedSprite currentSprite;


	public override void _Ready()
	{
		currentSprite = (AnimatedSprite)GetNode("AnimatedSprite");
		
		timer.WaitTime = 3; 
		timer.Connect("timeout", this, "on_timeout_complete");
		AddChild(timer);
		timer.Start();
		currentSprite.Play("rock");

		this.GlobalPosition = GlobalVariables.PlayerPosition;
		input_vector = (GetGlobalMousePosition() - GlobalVariables.PlayerPosition);


		input_vector = input_vector.Normalized() * ACCELERATION * 2;

	}

	private void on_timeout_complete(){
		dequeue();
	}

	private void dequeue(){
		QueueFree();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	if(GlobalVariables.Paused == false){
		MoveAndSlide(input_vector);	
	}
  }

}
