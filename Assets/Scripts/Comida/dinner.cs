using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dinner : meal
{
	private float startTime;
	private int _id;

	public int ID
	{
		get { return _id; }
		set { _id = value; }
	}

	private Estados.foodInKitchen _state;
	public Estados.foodInKitchen State
	{
		get { return _state; }
	}

	private int _tableID;

	public int TableID
	{
		get { return _tableID; }
		set { _tableID = value; }
	}


	public dinner(int id, string name, float time, float cost, float start) : base(name, time, cost)
    {
		_id = id;
		_state = Estados.foodInKitchen.cooking;
		startTime = start;
    }

	public void Cooking(float time)
	{
        float elapsedTime = time - startTime;
        if (_state == Estados.foodInKitchen.cooking && elapsedTime >= _cookingTime)
		{
			_state = Estados.foodInKitchen.Ready;
		}
	}
	public void delivered()
	{
		_state = Estados.foodInKitchen.Delivered;
	}

	public void throwFood()
	{
		_state = Estados.foodInKitchen.Discarded;
	}
}
