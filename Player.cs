using Godot;
using System;

public class Player : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	Vector2 Velocity;
	
	float MAX_SPEED = 150;
	
	
	int bruh = 1;
	
	const int FRICTION = 90;
	
	const int ACCELERATION = 100;
	
	bool isAttacking = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public Vector2 GetInput()	{
		
		var input_vector = Vector2.Zero;
		input_vector.x = Input.GetActionStrength("ui_right")
							- Input.GetActionStrength("ui_left");
		input_vector.y = Input.GetActionStrength("ui_down")
							- Input.GetActionStrength("ui_up");
							
		input_vector = input_vector.Normalized();
		return input_vector;
							
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _PhysicsProcess(float delta)
  {
  		var input_vector = GetInput();
		
		var currentSprite = (AnimatedSprite)GetNode("AnimatedSprite");
		
		if(input_vector != Vector2.Zero) {
			Velocity += input_vector * ACCELERATION;
		}
		else{
			Velocity = Vector2.Zero;
		}
		
		
		
		if (Input.IsActionPressed("ui_right") && isAttacking == false) {
			currentSprite.FlipH = false;
			currentSprite.Play("walk");
			bruh = 1;
		} else if (Input.IsActionPressed("ui_left") && isAttacking == false) {
			currentSprite.FlipH = true;
			currentSprite.Play("walk");
			bruh = 2;
		} 
		else if(Input.IsActionPressed("ui_up") && isAttacking == false){
			//currentSprite.FlipH = true;
			currentSprite.Play("walk");
			bruh = 3;
		}	
		else if(Input.IsActionPressed("ui_down") && isAttacking == false){
			//currentSprite.FlipH = true;
			currentSprite.Play("walk");
			bruh = 4;
		}
		else if(Input.IsActionPressed("ui_attack1")){
			//currentSprite.FlipH = true;

			bruh = 5;
			isAttacking = true;
		}
		else if(Input.IsActionPressed("ui_attack2")){
			//currentSprite.FlipH = true;

			bruh = 6;
			isAttacking = true;
		}
		else if(isAttacking){
			if(bruh == 5){
				currentSprite.Play("attack1");
			}
			else{
				currentSprite.Play("attack2");
			}
		}
//		
		else{
			currentSprite.Play("idle");
		}
		
		//moveandcollide + delta
		//moveandslide - no delta
		
		if(isAttacking == false){
			Velocity = Velocity.MoveToward(Vector2.Zero, FRICTION);
			Velocity = Velocity.Clamped(MAX_SPEED);
			MoveAndSlide(Velocity);
		}
  }

	private void _on_AnimatedSprite_animation_finished()
	{
		var currentSprite = (AnimatedSprite)GetNode("AnimatedSprite");
		if(currentSprite.Animation == "attack1" || currentSprite.Animation == "attack2"){
			isAttacking = false;
		}
	}
}



