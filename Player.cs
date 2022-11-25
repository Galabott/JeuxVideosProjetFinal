using Godot;
using System;

public class Player : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	Vector2 Velocity;
	
	float MAX_SPEED = 150;
	
	bool dead = false;
	
	private int health = 100;

	int bruh = 1;
	
	const int FRICTION = 90;
	
	const int ACCELERATION = 100;
	
	bool isAttacking = false;

	System.Random random = new System.Random(); 

	Timer timer = new Timer();
	
	Timer btimer = new Timer();
	Timer attacktimer = new Timer();
	Timer magictimer = new Timer();
	Timer ragetimer = new Timer();


	bool canAttack = true;
	bool canMagic = true;


	Boolean invincible = false;
	int timerI = 3;

	AnimatedSprite currentSprite;

	ShaderMaterial shader;
	
	int zoombonus = 0;
	bool zoomactive = false;

	int rageCount = 0;
	bool rageactive = false;

	Camera2D cam; 

	PackedScene rockProjectile;

	AudioStreamPlayer2D hitsound;
	AudioStreamPlayer2D healthkit;
	
	//Control endmenu;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rockProjectile = (PackedScene)GD.Load("res://RockProjectile.tscn");
		
		hitsound = (AudioStreamPlayer2D)GetNode("AudioStreamPlayer2D");
		healthkit = (AudioStreamPlayer2D)GetNode("AudioStreamPlayer2D2");


		timer.WaitTime = 1; 
		timer.Connect("timeout", this, "on_timeout_complete");
		AddChild(timer);

		btimer.WaitTime = 5; 
		btimer.Connect("timeout", this, "on_btimeout_complete");
		AddChild(btimer);
		btimer.OneShot = true;

		attacktimer.WaitTime = 3;
		attacktimer.Connect("timeout", this, "on_attacktimeout_complete");
		AddChild(attacktimer);
		attacktimer.OneShot = true;

		magictimer.WaitTime = 3;
		magictimer.Connect("timeout", this, "on_magictimer_complete");
		AddChild(magictimer);
		magictimer.OneShot = true;
		
		ragetimer.WaitTime = 10;
		ragetimer.Connect("timeout", this, "on_ragetimeout_complete");
		AddChild(ragetimer);
		ragetimer.OneShot = true;

		var attackarea = (Area2D)GetNode("AttackArea");
		var attackcollision = (CollisionShape2D)attackarea.GetChild(0);

		currentSprite = (AnimatedSprite)GetNode("AnimatedSprite");
		shader = currentSprite.Material as ShaderMaterial;

		attackcollision.Disabled = true;

		cam = (Camera2D)GetNode("Camera2D");
		
		//AudioStreamPlayer2D
	}

	private int getHealth(){
		return health;
	}

	private void on_magictimer_complete(){
		canMagic = true;
	}

	private void on_timeout_complete(){
		timerI--;
		if(timerI % 2 == 0){
			shader.SetShaderParam("flash_modifier", 0);
		}
		else{
			shader.SetShaderParam("flash_modifier", 0.75);
		}
		if(timerI == 0){
			invincible = false;
			GD.Print("TIMEOUT INVINCIBLE");
			timerI = 3;
			timer.Stop();
		}
	}
	
		private void HealthKit(){

			if(health < 100){
				GetTree().CallGroup("HealthKit","Consume");
				health = 100;
				if(!GlobalVariables.Muted){
					healthkit.Play();
				}
				GetTree().CallGroup("Health","ChangeHealth", health);
				GD.Print("Health Consumed!");			
			}
			else{
				GD.Print("I dont need health!");			
			}
			
			
		}

		private void on_btimeout_complete(){
			GD.Print("Zoom inactive");
			zoomactive = false;
		}

		private void on_attacktimeout_complete(){
			canAttack = true;
		}
		
		private void on_ragetimeout_complete(){
			rageactive = false;
			zoomactive = false;
			MAX_SPEED = 150;
		}
		
		private void toggle_music(){
			if(GlobalVariables.Muted){
				GlobalVariables.Muted = false;
				GetTree().CallGroup("Level1","MusicToggle");	
			}
			else{
				GlobalVariables.Muted = true;
				GetTree().CallGroup("Level1","MusicToggle");	
			}
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
		var PauseMenu = (CanvasLayer)GetNode("PauseMenu");
		if(GlobalVariables.Paused == true){//      //      //      
			PauseMenu.Visible = true;
		}
		else{
			PauseMenu.Visible = false;
		}

		var attackarea = (Area2D)GetNode("AttackArea");
		var attackcollision = (CollisionShape2D)attackarea.GetChild(0);

		GlobalVariables.PlayerPosition = this.GlobalPosition;

		if(Input.IsActionJustPressed("ui_pause")){
			GlobalVariables.Paused = !GlobalVariables.Paused;
		}
		
		if(Input.IsActionJustPressed("ui_mute")){
			toggle_music();
		}
		
		if(Input.IsActionJustPressed("ui_rage")){
			_rageMode();
		}

		if(GlobalVariables.Paused == false){
				if(input_vector != Vector2.Zero) {
				Velocity += input_vector * ACCELERATION;
			}
			else{
				Velocity = Vector2.Zero;
			}

			if(Input.IsActionJustPressed("ui_rock") && canMagic == true){
				Node2D rockprojectile_instance = (Node2D)rockProjectile.Instance();
				GetTree().GetRoot().AddChild(rockprojectile_instance);
				canMagic = false;
				magictimer.Start();
			}
			
			if(dead == false){
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
			else if(Input.IsActionPressed("ui_attack1") && canAttack){
				//currentSprite.FlipH = true;
				canAttack = false;
				attacktimer.Start();
				bruh = 5;
				isAttacking = true;
			}
			else if(Input.IsActionPressed("ui_attack2") && canAttack){
				//currentSprite.FlipH = true;
				canAttack = false;
				attacktimer.Start();
				bruh = 6;
				isAttacking = true;
			}
			else if(Input.IsActionPressed("ui_DIE!")){
				_setHealth(1);
			}
			else if(isAttacking){
				if(bruh == 5){
					currentSprite.Play("attack1");
					attackcollision.Disabled = false;
				}
				else{
					
					currentSprite.Play("attack2");
					attackcollision.Disabled = false;
				}
			}
	//		
				else{
					currentSprite.Play("idle");
				}
				if(isAttacking == false){
					Velocity = Velocity.MoveToward(Vector2.Zero, FRICTION);
					Velocity = Velocity.Clamped(MAX_SPEED);
					MoveAndSlide(Velocity);
				}
				
				if(health <= 0){
					currentSprite.Play("die");
					//dead = true;
				}
			}
			
			if(zoomactive && cam.Zoom.x < 2){
				
				cam.Zoom += new Vector2(0.25f, 0.25f);
			}
			else if(!zoomactive && cam.Zoom.x > 1){
				cam.Zoom -= new Vector2(0.25f, 0.25f);
			}
		
		}

		
		
		//moveandcollide + delta
		//moveandslide - no delta
		
		
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
			GetTree().ChangeScene("res://EndMenu.tscn");
		}
	}

	private void _setHealth(int dmg){
		health -= dmg;
		GetTree().CallGroup("Health","ChangeHealth", health);
	}
	
	private void _interactiveZoom(){
		zoombonus++;
		if(zoombonus == 5){
			GD.Print("Zoom active");
			btimer.Start();
			zoomactive = true;
			zoombonus = 0;
		}
		if(zoombonus > 0 && zoomactive == true){
			zoombonus = 0;
			btimer.Stop();
			btimer.Start();
		}
	}
	private void _rageMode(){
		if(rageCount >= 10){
			rageactive = true;
			zoomactive = true;
			MAX_SPEED = 250;
			rageCount = 0;
			ragetimer.Start();
			GetTree().CallGroup("Rage", "toggleVisibility");
		}
	}
	// Rage, toggleVisibility
	private void _rageCount(){
		rageCount++;
		if(rageCount == 10){
			GetTree().CallGroup("Rage", "toggleVisibility");
		}
	}

		private void _on_HitBox_area_entered(Area2D area)
	{
		var currentSprite = (AnimatedSprite)GetNode("AnimatedSprite");
				if(area.IsInGroup("EnemyAttack") && invincible == false){
					
					GD.Print("Timer Start");
					invincible = true;
					timer.Start();
					shader.SetShaderParam("flash_modifier", 0.75);

					if(!GlobalVariables.Muted){
						hitsound.Play();
					}
					
					GD.Print("DMG");
					
					int dmg = random.Next(60, 99);
					if(rageactive == true){
						dmg = dmg/4;
					}
					_setHealth(dmg);
					
					GD.Print("HEALTH " + health);
					if(health <= 0){
						dead = true;
						currentSprite.Play("die");
						GetTree().CallGroup("Health","ChangeHealth", 0);
					}
					
					
				}
			}
	
}
