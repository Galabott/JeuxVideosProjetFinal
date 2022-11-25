using Godot;
using System;

public class Menu : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	VBoxContainer Options;
	AudioStreamPlayer mainmenumusic;
	public override void _Ready()
	{
		mainmenumusic = (AudioStreamPlayer)GetNode("AudioStreamPlayer");
		Options = (VBoxContainer)GetNode("VBoxContainer2");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	private void play(){
		mainmenumusic.Play();
		GlobalVariables.Muted = false;
	}
	private void mute(){
		mainmenumusic.Stop();
		GlobalVariables.Muted = true;
	}
	private void _on_Start_pressed()
	{
		GetTree().ChangeScene("res://Level1.tscn");
	}
	private void _on_Quit_pressed()
	{
		GetTree().Quit();
	}
	private void _on_Button_pressed()
	{
		if(GlobalVariables.Muted == true){//      //      //      
			play();
		}
		else{
			mute();
		}
	}
			
	private void _on_Options_pressed()
	{
		if(Options.Visible){
				Options.Visible = false;
		}
		else{
			Options.Visible = true;
		}
	}
}




		








