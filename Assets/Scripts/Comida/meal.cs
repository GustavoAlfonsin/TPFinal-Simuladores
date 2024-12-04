using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class meal 
{
	public string _name;
	public float _cookingTime;
	public float _cost;
	
    public meal(string name, float time, float cost)
    {
        this._name = name;
		this._cookingTime = time;
		this._cost = cost;
    }

}
