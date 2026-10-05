using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proiectAsm
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void browseButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Text files (*.asm)|*.asm|All files (*.*)|*.*";
			openFileDialog.Title = "Select an Assembly File";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string filePath = openFileDialog.FileName;
				pathTextbox.Text = filePath;
			}
		}

		private void getAsmCode_Click(object sender, EventArgs e)
		{
			try
			{
				string fileContent = System.IO.File.ReadAllText(pathTextbox.Text);
				asmCodeTextbox.Text = fileContent;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error reading file: " + ex.Message);
			}
		}

		private LabelManager labelManager = new LabelManager();
		private void generateTokensButton_Click(object sender, EventArgs e)
		{
			string asmCode = asmCodeTextbox.Text;
			string[] lines = asmCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			List<string> tokens = new List<string>();
			List<string> labels = new List<string>();

			foreach (string line in lines)
			{
				string trimmedLine = line.Trim();

				if (trimmedLine.EndsWith(":"))
				{
					labels.Add(trimmedLine.Substring(0, trimmedLine.Length - 1));
					continue;
				}

				string[] lineTokens = trimmedLine.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
				tokens.AddRange(lineTokens);
			}

			labelManager.LoadLabelsFromList(labels);

			tokensTextbox.Text = string.Join(Environment.NewLine, tokens);
		}




		private void machineCodeButton_Click(object sender, EventArgs e)
		{
			string[] asmLines = asmCodeTextbox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);


			string[] tokens = tokensTextbox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			List<string> machineCodes = new List<string>();

			int i = 0;
			while (i < tokens.Length)
			{
				string opcode = tokens[i].ToUpper();

				if (!Instruction.InstructionSet.ContainsKey(opcode))
				{
					MessageBox.Show($"Unknown instruction: {opcode}");
					break;
				}

				Instruction instr = Instruction.InstructionSet[opcode];
				string classType = instr.ClassType;

				try
				{
					ushort machineCode = 0;

					switch (classType)
					{
						case "B1":
							if (i + 2 >= tokens.Length)
								throw new Exception($"Not enough operands for B1 instruction {opcode}");

							string dest = tokens[i + 1];
							string src = tokens[i + 2];
							List<string> b1Binaries = GenerateB1MachineCode(opcode, dest, src);
							machineCodes.AddRange(b1Binaries);
							i += 3;
							break;


						case "B2":
							if (i + 1 >= tokens.Length)
								throw new Exception($"Not enough operands for B2 instruction {opcode}");

							string operandB2 = tokens[i + 1];
							List<string> binaries = GenerateB2MachineCode(opcode, operandB2);
							machineCodes.AddRange(binaries);

							i += 2;
							break;


						case "B3":
							if (i + 1 >= tokens.Length)
								throw new Exception($"Not enough operands for B3 instruction {opcode}");

							string label = tokens[i + 1];
							List<string> b3Codes = GenerateB3MachineCode(opcode, label, machineCodes.Count + 1);
							machineCodes.AddRange(b3Codes);
							i += 2;
							break;

						case "B4":
							List<string> b4CodeList = GenerateB4MachineCode(opcode);
							machineCodes.AddRange(b4CodeList);
							i += 1;
							break;


						default:
							throw new Exception($"Unsupported instruction class: {classType}");
					}

					string binary = Convert.ToString(machineCode, 2).PadLeft(16, '0');
					machineCodes.Add(binary);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error processing instruction: " + ex.Message);
					break;
				}
			}

			machineCodeTextbox.Text = string.Join(
				Environment.NewLine,
				machineCodes.Where(code => code != "0000000000000000")
			);


			List<string> GenerateB1MachineCode(string opcode, string destOperand, string srcOperand)
			{
				if (!Instruction.InstructionSet.ContainsKey(opcode))
					throw new ArgumentException($"Invalid opcode: {opcode}");

				Instruction instr = Instruction.InstructionSet[opcode];

				if (instr.ClassType != "B1")
					throw new ArgumentException($"Instruction {opcode} is not of class B1");

				List<string> binaryList = new List<string>();
				ushort machineCode = 0;

				ushort opcodeBits = (ushort)instr.Opcode;
				machineCode |= (ushort)(opcodeBits << 12);

				int srcMode = AddressingMode.GetModeFromOperand(srcOperand);

				Match srcMatch = Regex.Match(srcOperand.ToUpper(), @"R\d+");
				if (!srcMatch.Success)
					throw new ArgumentException($"Invalid source register in operand: {srcOperand}");

				string srcReg = srcMatch.Value;

				if (!Register.RegisterSet.ContainsKey(srcReg))
					throw new ArgumentException($"Invalid source register: {srcReg}");
				int srcRegValue = Register.RegisterSet[srcReg].BinaryValue;
				machineCode |= (ushort)((srcMode & 0b11) << 10);
				machineCode |= (ushort)((srcRegValue & 0b1111) << 6);

				int destMode = AddressingMode.GetModeFromOperand(destOperand);

				Match destMatch = Regex.Match(destOperand.ToUpper(), @"R\d+");
				if (!destMatch.Success)
					throw new ArgumentException($"Invalid destination register in operand: {destOperand}");

				string destReg = destMatch.Value;

				if (!Register.RegisterSet.ContainsKey(destReg))
					throw new ArgumentException($"Invalid destination register: {destReg}");
				int destRegValue = Register.RegisterSet[destReg].BinaryValue;
				machineCode |= (ushort)((destMode & 0b11) << 4);
				machineCode |= (ushort)(destRegValue & 0b1111);

				binaryList.Add(Convert.ToString(machineCode, 2).PadLeft(16, '0'));

				if (srcMode == 0b11)
				{
					int? offset = ExtractNumberExcludingR(srcOperand);
					if (offset.HasValue && offset.Value != 0)
					{
						binaryList.Add(Convert.ToString((ushort)offset.Value, 2).PadLeft(16, '0'));
					}
				}

				if (destMode == 0b11)
				{
					int? offset = ExtractNumberExcludingR(destOperand);
					if (offset.HasValue && offset.Value != 0)
					{
						binaryList.Add(Convert.ToString((ushort)offset.Value, 2).PadLeft(16, '0'));
					}
				}

				return binaryList;
			}


			List<string> GenerateB2MachineCode(string opcode, string operand)
			{
				if (!Instruction.InstructionSet.ContainsKey(opcode))
					throw new ArgumentException($"Invalid opcode: {opcode}");

				Instruction instr = Instruction.InstructionSet[opcode];

				if (instr.ClassType != "B2")
					throw new ArgumentException($"Instruction {opcode} is not of class B2");

				List<string> binaryList = new List<string>();
				ushort machineCode = 0;
				ushort opcodeBits = (ushort)instr.Opcode;
				machineCode |= (ushort)(opcodeBits << 6);

				int addressingMode = AddressingMode.GetModeFromOperand(operand);
				Match match = Regex.Match(operand.ToUpper(), @"R\d+");

				if (match.Success)
				{
					string register = match.Value;

					if (!Register.RegisterSet.ContainsKey(register))
						throw new ArgumentException($"Invalid register: {register}");

					int regValue = Register.RegisterSet[register].BinaryValue;
					machineCode |= (ushort)((addressingMode & 0b11) << 4);
					machineCode |= (ushort)(regValue & 0b1111);
				}

				binaryList.Add(Convert.ToString(machineCode, 2).PadLeft(16, '0'));

				if (addressingMode == 0b11)
				{
					int? offset = ExtractNumberExcludingR(operand);
					if (offset.HasValue && offset.Value != 0)
					{
						short signedOffset = (short)offset.Value;
						ushort offsetValue = unchecked((ushort)signedOffset);
						binaryList.Add(Convert.ToString(offsetValue, 2).PadLeft(16, '0'));
					}
				}

				return binaryList;
			}


			List<string> GenerateB3MachineCode(string opcode, string label, int currentPC)
			{
				//nu calculeaza offset bine
				if (!Instruction.InstructionSet.ContainsKey(opcode))
					throw new ArgumentException($"Invalid opcode: {opcode}");

				if (!labelManager.Contains(label))
					throw new ArgumentException($"Label not found: {label}");

				Instruction instr = Instruction.InstructionSet[opcode];
				if (instr.ClassType != "B3")
					throw new ArgumentException($"Instruction {opcode} is not of class B3");

				int labelAddress = labelManager.GetAddress(label);

				int offset = labelAddress - currentPC;

				if (offset < -128 || offset > 127)
					throw new ArgumentException($"Offset for label {label} is out of range: {offset}");

				ushort opcodeBits = (ushort)(instr.Opcode << 8);
				byte signedOffset = (byte)offset;

				ushort machineCode = (ushort)(opcodeBits | signedOffset);

				return new List<string> { Convert.ToString(machineCode, 2).PadLeft(16, '0') };
			}


			List<string> GenerateB4MachineCode(string opcode)
			{
				if (!Instruction.InstructionSet.ContainsKey(opcode))
					throw new ArgumentException($"Invalid opcode: {opcode}");

				Instruction instr = Instruction.InstructionSet[opcode];

				if (instr.ClassType != "B4")
					throw new ArgumentException($"Instruction {opcode} is not of class B4");

				ushort machineCode = (ushort)instr.Opcode;
				string binary = Convert.ToString(machineCode, 2).PadLeft(16, '0');

				return new List<string> { binary };
			}



			int? ExtractNumberExcludingR(string input)
			{
				Match match = Regex.Match(input, @"(?<!R)\b-?\d+\b", RegexOptions.IgnoreCase);

				if (match.Success && int.TryParse(match.Value, out int result) && result != 0)
				{
					return result;
				}

				return null;
			}

		}

		private void saveObjButton_Click(object sender, EventArgs e)
		{
			try
			{
				string[] lines = machineCodeTextbox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

				using (FileStream fileStream = new FileStream("test.obj", FileMode.Create, FileAccess.Write))
				using (BinaryWriter writer = new BinaryWriter(fileStream))
				{
					foreach (string line in lines)
					{
						ushort value = Convert.ToUInt16(line, 2);
						writer.Write(value);
					}
				}

				DialogResult result = MessageBox.Show("The file test.obj was successfully saved!");
				if(result == DialogResult.OK)
				{
					Simulator simulator = new Simulator();
					simulator.Show();
					this.Hide();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error saving the file " + ex.Message);
			}
		}

	}
}