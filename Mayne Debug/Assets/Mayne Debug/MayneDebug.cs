using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public static class MayneDebug
{
	#region -= HTML Pieces =-
		public static string logWriteLocation = "./HTML Debug/debug.html";
		public static string logTopPiece = "./HTML Debug/parts/debugBottom.html";
		public static string lobBotPiece = "./HTML Debug/parts/debugTop.html";
	#endregion

	//We use editor prefs to store the log id.
	private readonly static string logIDPref = "LogID";
	private static bool clearOnPlay = true;

	//We use this to store the new lines that we are adding to the HTML file
	private static List<string> insertedHTML = new List<string>();

	//The two pieces that make up our HTML file
	private static string[] topHTML;
	private static string[] botHTML;

	//Once we are setup we don't have to load anymore
	private static bool isSetup = false;

	[Flags] //Types of logs
	public enum LogCatagories
	{
		none = 0x0,
		networking = 0x2,
		ai = 0x4,
		audio = 0x8,
		gamelogic = 0x10,
		all = 0x20
	}

	public static void Setup()
	{
		//Check if all the files exist
		if( !File.Exists( logTopPiece ) )
		{
			Debug.LogError("ERROR! Missing file at " + logTopPiece.ToString() );
			return;
		}

		if( !File.Exists( lobBotPiece ) )
		{
			Debug.LogError("ERROR! Missing file at " + lobBotPiece.ToString() );
			return;
		}

		//Now load each part into our string
		topHTML = File.ReadAllLines( logTopPiece );
		botHTML = File.ReadAllLines( lobBotPiece );

		if( clearOnPlay )
		{
			EditorPrefs.SetInt(logIDPref, 0);
		}

		

		isSetup = true;
	}

	public static void Log(string message, LogCatagories cat, MonoBehaviour sender, LogCatagories logType = LogCatagories.none )
	{
		if( !isSetup )
			Setup();

		WriteToLog( CreateLogItem( LogType.Log, cat, message, sender ) );
	}

	public static void LogWarning(string message, LogCatagories cat, MonoBehaviour sender, LogCatagories logType = LogCatagories.none )
	{
		if( !isSetup )
			Setup();

		WriteToLog( CreateLogItem( LogType.Warning, cat,  message, sender ) );
	}

	public static void LogError(string message, LogCatagories cat, MonoBehaviour sender, LogCatagories logType = LogCatagories.none )
	{
		if( !isSetup )
			Setup();

		WriteToLog( CreateLogItem( LogType.Error, cat, message, sender ) );
	}


	private static void WriteToLog( string Log )
	{
		insertedHTML.Add( Log );
		using( File.Create( logWriteLocation )){}

		using ( StreamWriter file  = new StreamWriter( logWriteLocation, true ) )
		{
			//Write Top
			for(int i = 0; i < botHTML.Length; i++ )
			{
				file.WriteLine( botHTML[i]);
			}

			//Write Inserted HTML
			for(int i = 0; i < insertedHTML.Count; i++ )
			{
				file.WriteLine( insertedHTML[i]);
			}

			//Write Bottom
			for(int i = 0; i < topHTML.Length; i++ )
			{
				file.WriteLine( topHTML[i]);
			}
		}

	}

	private static string CreateLogItem(LogType logType, LogCatagories logCat, string Message, MonoBehaviour sender )
	{


		int id = EditorPrefs.GetInt( logIDPref, 0 );
		string newLog;
		newLog = string.Format( "<tr class=\"log{0}", GetLogCatagorieNames (logCat) + GetLogTypeClasName( logType ) );
		newLog += string.Format( "\">" );
		newLog += string.Format( "<td>{0}</td>", id);
		newLog += string.Format( "<td class=\"logtype\"></td>" );
		newLog += string.Format( "<td>{0}</td>", Message );
		newLog += string.Format( "<td>{0}</td>", sender.name );
		newLog += string.Format( "<td>{0}</td>", Time.time.ToString() );
		newLog += string.Format( "<tr/>" );
		EditorPrefs.SetInt( logIDPref, id + 1 );

		return newLog;
	}

	private static string GetLogTypeClasName(LogType type )
	{
		switch (type) 
		{

		case LogType.Exception:
		case LogType.Error:
		case LogType.Assert:
			return " error";

		case LogType.Warning:
			return " warning";

		case LogType.Log:
			return "";
	
		default:
			return "";
		}
	}

	private static string GetLogCatagorieNames(LogCatagories cat)
	{
		string classString = "";

		for( int i = 2; i < 256; i *= 2 )
		{
			if( (((LogCatagories)i) & cat) == LogCatagories.ai)
			{
				classString += " ai";
			}

			if( (((LogCatagories)i) & cat) == LogCatagories.audio)
			{
				classString += " audio";
			}

			if( (((LogCatagories)i) & cat) == LogCatagories.gamelogic )
			{
				classString += " gamelogic";
			}

			if( (((LogCatagories)i) & cat) == LogCatagories.networking )
			{
				classString += " networking";
			}
		}

		return classString;
	}
}
