using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableFood
{
	private float start_time;
	private string _name;
	private int _idOrder;

	public int IDOrder
	{
		get { return _idOrder; }
		set { _idOrder = value; }
	}

	public string Name
	{
		get { return _name; }
	}

	private float _eatingTime;
	public float EatingTime
	{
		get { return _eatingTime; }
	}

	private float _cost;
	public float Cost
	{
		get { return _cost; }
	}

	private Estados.tableFood _state;
	public Estados.tableFood State
	{
		get { return _state; }
	}

    public TableFood(string name, float time, float cost)
    {
        this._name = name;
		this._eatingTime = time;
		this._cost = cost;
		this._state = Estados.tableFood.NotYetDelivered;
    }

	public void wasDelivered()
	{
		this._state = Estados.tableFood.Ontable;
		start_time = Time.time;
	}

	public void eating(float time)
	{
		if (_state == Estados.tableFood.Ontable)
		{
			float elapsedTime = time - start_time;
			if (elapsedTime >= _eatingTime)
			{
				_state = Estados.tableFood.Done;
			}

        }
	}
}
