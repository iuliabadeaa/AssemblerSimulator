using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiectAsm
{
	internal class Register
	{
		public string Name { get; set; }
		public int BinaryValue { get; set; }
		public int DecimalValue { get; set; }

		public Register(string name, int binaryValue, int decimalValue)
		{
			Name = name;
			BinaryValue = binaryValue;
			DecimalValue = decimalValue;
		}

		public override string ToString()
		{
			return $"Register: {Name}, Binary: {Convert.ToString(BinaryValue, 2).PadLeft(4, '0')}, Decimal: {DecimalValue}";
		}

		public static Dictionary<string, Register> RegisterSet { get; } = new Dictionary<string, Register>
		{
			{ "R0", new Register("R0", 0b0000, 0) },
			{ "R1", new Register("R1", 0b0001, 1) },
			{ "R2", new Register("R2", 0b0010, 2) },
			{ "R3", new Register("R3", 0b0011, 3) },
			{ "R4", new Register("R4", 0b0100, 4) },
			{ "R5", new Register("R5", 0b0101, 5) },
			{ "R6", new Register("R6", 0b0110, 6) },
			{ "R7", new Register("R7", 0b0111, 7) },
			{ "R8", new Register("R8", 0b1000, 8) },
			{ "R9", new Register("R9", 0b1001, 9) },
			{ "R10", new Register("R10", 0b1010, 10) },
			{ "R11", new Register("R11", 0b1011, 11) },
			{ "R12", new Register("R12", 0b1100, 12) },
			{ "R13", new Register("R13", 0b1101, 13) },
			{ "R14", new Register("R14", 0b1110, 14) },
			{ "R15", new Register("R15", 0b1111, 15) }	
		};	
	}
}
