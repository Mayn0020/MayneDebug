using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		MayneDebug.Log("AI NOT FOUND", MayneDebug.LogCatagories.ai | MayneDebug.LogCatagories.audio,  this, MayneDebug.LogCatagories.gamelogic);
		MayneDebug.LogError("DUNCAN IS AN ERROR", MayneDebug.LogCatagories.ai | MayneDebug.LogCatagories.audio,  this, MayneDebug.LogCatagories.gamelogic);
		MayneDebug.Log("Missing Object", MayneDebug.LogCatagories.gamelogic | MayneDebug.LogCatagories.audio,  this, MayneDebug.LogCatagories.gamelogic);
		MayneDebug.LogWarning("File now found", MayneDebug.LogCatagories.audio | MayneDebug.LogCatagories.networking | MayneDebug.LogCatagories.gamelogic, this, MayneDebug.LogCatagories.networking);
		//MayneDebug.LogError("FUCK", this, MayneDebug.LogCatagories.gamelogic);
		//MayneDebug.Log("Lame", this, MayneDebug.LogCatagories.gamelogic);
		//MayneDebug.LogWarning("Fake", this, MayneDebug.LogCatagories.networking);
		//MayneDebug.LogError("Hello?", this, MayneDebug.LogCatagories.gamelogic);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
