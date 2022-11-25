using Godot;
using System;

public class PauseMenu : CanvasLayer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void _on_Menu_pressed()
	{
			GetTree().ChangeScene("res://Menu.tscn");
	}


	private void _on_Restart_pressed()
	{
			GetTree().ChangeScene("res://Level1.tscn");
	}


	private void _on_Quit_pressed()
	{
			GetTree().Quit();
	}

}





