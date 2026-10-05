using System;
using System.Collections.Generic;

public static class AddressingMode
{
	public static readonly Dictionary<string, int> Modes = new Dictionary<string, int>
	{
		{ "IMMEDIATE", 0b00 },
		{ "DIRECT", 0b01 },
		{ "INDIRECT", 0b10 },
		{ "INDEXED", 0b11 }
	};

		

	public static int GetModeFromOperand(string operand)
	{
		if (operand.StartsWith("#"))
			return 0b00;
		else if (operand.StartsWith("(") && operand.EndsWith(")"))
			return 0b10;
		else if (operand.Contains("+"))
			return 0b11;
		else
			return 0b01;
	}
}
