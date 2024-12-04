using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoClient 
{
	private int _id;
	public int ID
	{
		get { return _id; }
		set { _id = value; }
	}

	private string _name;
	public string Name
	{
		get { return _name; }
		set { _name = value; }
	}

	private int _numberOfTable;

	public int NumberOfTable
	{
		get { return _numberOfTable; }
		set { _numberOfTable = value; }
	}


}
