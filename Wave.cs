using Godot;
using System;

public class Wave : Label
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
		private void ChangeWave(){
		
		this.SetText("Wave " + GlobalVariables.WaveCount);
	}

	private void SetTextWave(string text){
		this.SetText(text);
	}

	private void Win(){
		this.SetText("Waves completed! \n Proceed to the hole up in the rocks to continue!");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
