using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules {

	public string RuleName = " ";
	public string Operator = " ";

	public int Amount = 0;

	public string Output = " ";

	public Rules(string name, string Op, int a, string outp)
	{
		RuleName = name;
		Operator = Op;
		Amount = a;
		Output = outp;
	}
}
