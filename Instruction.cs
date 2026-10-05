using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiectAsm
{
	public class Instruction
	{
		public int Opcode { get; set; }
		public string ClassType { get; set; }

		public Instruction(int binaryOpcode, string classType)
		{
			Opcode = binaryOpcode;
			ClassType = classType;
		}


		public override string ToString()
		{
			return $"Opcode: {Convert.ToString(Opcode, 2).PadLeft(16, '0')}, Class: {ClassType}";
		}

		public static Dictionary<string, Instruction> InstructionSet { get; } = new Dictionary<string, Instruction>
		{
			{ "MOV", new Instruction(0b0000, classType:"B1") },
			{ "ADD", new Instruction(0b0001, classType : "B1") },
			{ "SUB", new Instruction(0b0010, classType : "B1") },
			{ "CMP", new Instruction(0b0011, classType: "B1") },
			{ "AND", new Instruction(0b0100, classType : "B1") },
			{ "OR", new Instruction(0b0101, classType : "B1") },
			{ "XOR", new Instruction(0b0110, classType : "B1") },
			{ "CLR", new Instruction(0b1000000000, classType: "B2") },
			{ "NEG", new Instruction(0b1000000001, classType: "B2") },
			{ "INC", new Instruction(0b1000000010, classType: "B2") },
			{ "DEC", new Instruction(0b1000000011, classType: "B2") },
			{ "ASL", new Instruction(0b1000000100, classType: "B2") },
			{ "ASR", new Instruction(0b1000000101, classType: "B2") },
			{ "LSR", new Instruction(0b1000000110, classType: "B2") },
			{ "LSL", new Instruction(0b1000000111, classType: "B2") },
			{ "ROL", new Instruction(0b1000001000, classType: "B2") },
			{ "ROR", new Instruction(0b1000001001, classType: "B2") },
			{ "RLC", new Instruction(0b1000001010, classType: "B2") },
			{ "RRC", new Instruction(0b1000001011, classType: "B2") },
			{ "JMP", new Instruction(0b1000001100, classType: "B2") },
			{ "CALL", new Instruction(0b1000001101, classType: "B2") },
			{ "PUSH", new Instruction(0b1000001110, classType: "B2") },
			{ "POP", new Instruction(0b1000001111, classType: "B2") },
			{ "BR", new Instruction(0b11000000, classType: "B3") },
			{ "BNE", new Instruction(0b11000001, classType: "B3") },
			{ "BEQ", new Instruction(0b11000010, classType: "B3") },
			{ "BPL", new Instruction(0b11000011, classType: "B3") },
			{ "BMI", new Instruction(0b11000100, classType: "B3") },
			{ "BCS", new Instruction(0b11000101, classType: "B3") },
			{ "BCC", new Instruction(0b11000110, classType: "B3") },
			{ "BVS", new Instruction(0b11000111, classType: "B3") },
			{ "BVC", new Instruction(0b11010000, classType: "B3") },
			{ "CLC", new Instruction(0b1110000000000000, classType: "B4") },
			{ "CLV", new Instruction(0b1110000000000001, classType: "B4") },
			{ "CLZ", new Instruction(0b1110000000000010, classType: "B4") },
			{ "CLS", new Instruction(0b1110000000000011, classType: "B4") },
			{ "CCC", new Instruction(0b1110000000000100, classType: "B4") },
			{ "SEC", new Instruction(0b1110000000000101, classType: "B4") },
			{ "SEV", new Instruction(0b1110000000000110, classType: "B4") },
			{ "SEZ", new Instruction(0b1110000000000111, classType: "B4") },
			{ "SES", new Instruction(0b1110000000001000, classType: "B4") },
			{ "SCC", new Instruction(0b1110000000001001, classType: "B4") },
			{ "NOP", new Instruction(0b1110000000001010, classType: "B4") },
			{ "RET", new Instruction(0b1110000000001011, classType: "B4") },
			{ "RETI", new Instruction(0b1110000000001100, classType: "B4") },
			{ "HALT", new Instruction(0b1110000000001101, classType: "B4") },
			{ "WAIT", new Instruction(0b1110000000001110, classType: "B4") },
			{ "PUSHPC", new Instruction(0b1110000000001111, classType: "B4") },
			{ "POPPC", new Instruction(0b1110000000010000, classType: "B4") },
			{ "PUSHFLAG", new Instruction(0b1110000000010001, classType: "B4") },
			{ "POPFLAG", new Instruction(0b1110000000010010, classType: "B4") }
		};
	}


}
