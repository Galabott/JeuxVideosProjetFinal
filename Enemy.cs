using Godot;
using System;

public class Enemy : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	System.Random random = new System.Random(); 

	Vector2 Velocity = Vector2.Zero;
	
	float MAX_SPEED = 100;
	
	int bruh = 1;
	
	bool move = false;
	
	const int FRICTION = 80;
	
	const int ACCELERATION = 90;
	
	bool dead = false;
	
	bool isAttacking = false;

	PackedScene healthKit;
	AudioStreamPlayer2D hitsound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hitsound = (AudioStreamPlayer2D)GetNode("AudioStreamPlayer2D");
		healthKit = (PackedScene)GD.Load("res://HealthKit.tscn");
		var attackarea = (Area2D)GetNode("AttackArea");
		var attackcollision = (CollisionShape2D)attackarea.GetChild(0);

		attackcollision.Disabled = true;
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
		
		var currentSprite = (AnimatedSprite)GetNode("AnimatedSprite");		
		var attackarea = (Area2D)GetNode("AttackArea");
		var attackcollision = (CollisionShape2D)attackarea.GetChild(0);
		
		if(GlobalVariables.Paused == false){
		if(dead == false && isAttacking == false){
			if (Velocity > Vector2.Zero) {
			currentSprite.FlipH = false;
			currentSprite.Play("walk");
			bruh = 1;
			} else if (Velocity < Vector2.Zero) {
				currentSprite.FlipH = true;
				currentSprite.Play("walk");
				bruh = 2;
			} 
			else{
				currentSprite.Play("idle");
			}
			
			
			if(move){
			
				Velocity = (GlobalVariables.PlayerPosition - this.GlobalPosition) * ACCELERATION;
				Vector2 dist = (GlobalVariables.PlayerPosition - this.GlobalPosition);
				if(dist.x < 0){
					dist.x *= -1;
				}
				if(dist.y < 0){
					dist.y *= -1;
				}
				if(dist.x < 50 && dist.y < 60){
					attackcollision.Disabled = false;
					currentSprite.Play("attack1");
					isAttacking = true;
				}
				//Velocity = this.Position.DirectionTo(GlobalVariables.PlayerPosition);
				
				Velocity.Normalized();
				Velocity = Velocity.Clamped(MAX_SPEED);
				MoveAndSlide(Velocity);
			}
			else{
				Velocity = Vector2.Zero;
			}
		}
			
		}
  }

	private void _on_Area2D_area_entered(Area2D area)
	{
		var currentSprite = (AnimatedSprite)GetNode("AnimatedSprite");
		if(area.IsInGroup("Staff") || area.IsInGroup("Rock")){
			dead = true;
			if(!GlobalVariables.Muted){
				hitsound.Play();
			}
			currentSprite.Play("die");

			if(area.IsInGroup("Rock")){
				GetTree().CallGroup("Rock","dequeue");
			}

		}

	}
	
	private void _on_AnimatedSprite_animation_finished()
	{
		var currentSprite = (AnimatedSprite)GetNode("AnimatedSprite");
		var attackarea = (Area2D)GetNode("AttackArea");
		var attackcollision = (CollisionShape2D)attackarea.GetChild(0);
		if(currentSprite.Animation == "attack1" || currentSprite.Animation == "attack2"){
			isAttacking = false;
			attackcollision.Disabled = true;
		}
		if(currentSprite.Animation == "die"){
			int dropchance = random.Next(1, 100);
			GD.Print("Your lucky number : " + dropchance);
			if(dropchance >= 80){
				GD.Print("HEALTH KIT DROPPED!");
				Node2D healthKit_instance = (Node2D)healthKit.Instance();
				GetTree().GetRoot().AddChild(healthKit_instance);
				healthKit_instance.GlobalPosition = this.GlobalPosition;
			}
			GetTree().CallGroup("Player","_interactiveZoom");
			GetTree().CallGroup("Player","_rageCount");
			GlobalVariables.PlayerScore += 100;
			GetTree().CallGroup("Score","ChangeScore");
			QueueFree();
		}
	}
	


	private void _on_DetectionRange_area_entered(Area2D area)
	{
		var currentSprite = (AnimatedSprite)GetNode("AnimatedSprite");

		if(area.IsInGroup("Player")){

			GD.Print("ENTERED");
			
			move = true;

		}

	}
	
	private void _on_DetectionRange_area_exited(Area2D area)
	{
			var currentSprite = (AnimatedSprite)GetNode("AnimatedSprite");
			var playerhitbox = (CollisionShape2D)area.GetChild(0);

		if(area.IsInGroup("Player")){
			
			GD.Print("EXITED");
			
			move = false;
		}

	}

}
