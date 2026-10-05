using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proiectAsm
{
	public partial class Simulator : Form
	{
		public Simulator()
		{
			InitializeComponent();
		}

		int[] MEM = new int[100];
		int[] REG = new int[16];
		int PC = 0b0000000000000000;
		int SBUS = 0b0000000000000000, DBUS = 0b0000000000000000, RBUS = 0b0000000000000000;
		int IR = 0b0000000000000000, MDR = 0b0000000000000000, IVR = 0b0000000000000000, T = 0b0000000000000000;
		int SP = 0b0000000000000000, FLAG = 0b0000000000000000, ADR = 0b0000000000000000;
		int one = 0b0000000000000001;
		int minusOne = 0b1111111111111111;
		int zero = 0b0000000000000000;
		ulong MIR = 0;
		int BE = 0;
		int COND = 0;
		int BVI = 0;
		int BPO = 1;
		int BI = 0;
		int INTR = 0;
		ulong[] MPM = new ulong[] {
		0x60184D003, 0x00000198A, 0x000014804, 0x000010805, 0x301830000, 0x101908000, 0x301800000, 0x6019C0000, 0x701880000, 0x901634800,
		0x000000A0E, 0x000000B19, 0x000000D38, 0x000000D5A, 0x60188C813, 0x201500B15, 0x201880813, 0x60188C000, 0x901500B14, 0x293880000,
		0x000000802, 0x022900C1E, 0x022880C1E, 0x06288C000, 0x923880C1E, 0x000000802, 0x022900D25, 0x022880C25, 0x06288C000, 0x923880C25,
		0x501900B34, 0x59391CB34, 0x59391CB34, 0x59301CF00, 0x595920B34, 0x596920B34, 0x597920B34, 0x0B2920B34, 0x092920B34, 0xB9391CB34,
		0xC93918B34, 0x098918B34, 0x099918B34, 0x09A920B34, 0x09B920B34, 0x09C920B34, 0x09D920B34, 0x09E920B34, 0x3018C8F00, 0x801500000,
		0x301880000, 0x401804B34, 0x000000802, 0x901300F00, 0x0000C0F00, 0x0000C0F00, 0x000002B54, 0x000000F00, 0x000002BD4, 0x000000F00,
		0x000003354, 0x000000F00, 0x0000033D4, 0x000000F00, 0x000002354, 0x000000F00, 0x0000023D4, 0x000000F00, 0x000003B54, 0x000000F00,
		0x000003BD4, 0x000000F00, 0x000000B54, 0x000000000, 0x000000B4B, 0x60188C850, 0x000000802, 0x022500851, 0x60188C000, 0x923500851,
		0x953500000, 0x301808000, 0x6019C0000, 0x401600F00, 0x60188C859, 0x000000802, 0x022600F00, 0x60188C000, 0x923600F00, 0x953600F00,
		0x1A5100F00, 0x000000000, 0x1A6100F00, 0x000000000, 0x000000F00, 0x000000000, 0x00002C000, 0x000000000, 0x000024F00, 0x000000000,
		0x000028800, 0x000000000, 0x301808000, 0x6019C0F00, 0x301880000, 0x901604F00, 0x301808000, 0x1019C0F00, 0x301880000, 0x901104F00,
		0x301880000, 0x901604F00, 0x301880000, 0x901604000, 0x301880000, 0x901104F00
		};
		int ACLOW = 0x00;
		int CIL = 0x00;


		public void LoadObjFile(string path)
		{
			if (!File.Exists(path))
			{
				MessageBox.Show("File not found");
				return;
			}

			try
			{
				byte[] bytes = File.ReadAllBytes(path);
				if (bytes.Length % 2 != 0)
				{
					MessageBox.Show("File has an odd number of bytes, not valid 16-bit instructions.");
					return;
				}

				int wordCount = bytes.Length / 2;
				if (wordCount > MEM.Length)
				{
					MessageBox.Show("Memory overflow: too many instructions.");
					wordCount = MEM.Length;
				}

				for (int i = 0; i < wordCount; i++)
				{
					MEM[i] = bytes[i * 2] | (bytes[i * 2 + 1] << 8);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading binary file: " + ex.Message);
			}
		}


		private void Simulator_Load(object sender, EventArgs e)
		{
			string objPath = "D:\\asamblorProiect\\proiectAsm\\bin\\Debug\\test.obj";
			LoadObjFile(objPath);
			DisplayMemory();
			#region Initialize registers and flags
			PC_textbox.Text = Convert.ToString(PC, 2).PadLeft(16, '0');
			for (int i = 0; i < REG.Length; i++)
			{
				REG[i] = 0b0000000000000000;
			}
			R0_textbox.Text = Convert.ToString(REG[0], 2).PadLeft(16,'0');
			R1_textbox.Text = Convert.ToString(REG[1], 2).PadLeft(16, '0');
			R2_textbox.Text = Convert.ToString(REG[2], 2).PadLeft(16, '0');
			R3_textbox.Text = Convert.ToString(REG[3], 2).PadLeft(16, '0');
			R4_textbox.Text = Convert.ToString(REG[4], 2).PadLeft(16, '0');
			R5_textbox.Text = Convert.ToString(REG[5], 2).PadLeft(16, '0');
			R6_textbox.Text = Convert.ToString(REG[6], 2).PadLeft(16, '0');
			R7_textbox.Text = Convert.ToString(REG[7], 2).PadLeft(16, '0');
			R8_textbox.Text = Convert.ToString(REG[8], 2).PadLeft(16, '0');
			R9_textbox.Text = Convert.ToString(REG[9], 2).PadLeft(16, '0');
			R10_textbox.Text = Convert.ToString(REG[10], 2).PadLeft(16, '0');
			R11_textbox.Text = Convert.ToString(REG[11], 2).PadLeft(16, '0');
			R12_textbox.Text = Convert.ToString(REG[12], 2).PadLeft(16, '0');
			R13_textbox.Text = Convert.ToString(REG[13], 2).PadLeft(16, '0');
			R14_textbox.Text = Convert.ToString(REG[14], 2).PadLeft(16, '0');
			R15_textbox.Text = Convert.ToString(REG[15], 2).PadLeft(16, '0');

			SP_textbox.Text = Convert.ToString(SP, 2).PadLeft(16, '0');
			FLAG_textbox.Text = Convert.ToString(FLAG, 2).PadLeft(16, '0');
			IVR_textbox.Text = Convert.ToString(IVR, 2).PadLeft(16, '0');
			IR_textbox.Text = Convert.ToString(IR, 2).PadLeft(16, '0');
			MDR_textbox.Text = Convert.ToString(MDR, 2).PadLeft(16, '0');
			ADR_textbox.Text = Convert.ToString(ADR, 2).PadLeft(16, '0');
			SBUS_textbox.Text = Convert.ToString(SBUS, 2).PadLeft(16, '0');
			DBUS_textbox.Text = Convert.ToString(DBUS, 2).PadLeft(16, '0');
			RBUS_textbox.Text = Convert.ToString(RBUS, 2).PadLeft(16, '0');
			T_textbox.Text = Convert.ToString(T, 2).PadLeft(16, '0');
			ONE_textbox.Text = Convert.ToString(one, 2).PadLeft(16, '0');
			MINUSONE_textbox.Text = Convert.ToString(minusOne, 2).PadLeft(16, '0');
			ZERO_textbox.Text = Convert.ToString(zero, 2).PadLeft(16, '0');
			#endregion
			

		}

		private int CalculateGFunction()
		{
			int TF = (int)((MIR >> 7) & 0x1);
			int succesor = (int)((MIR >> 11) & 0x7);
			int g=0;
			switch (succesor)
			{
				case 0:
					g = TF & TF;
					break;
				case 1:
					g=TF&~TF;
					break;
				case 2:
					g= ACLOW & TF;
					break;
				case 3:
					g=CIL & TF;
					break;
				case 4:
					g = TF & (FLAG & 0b0000000000000001); //carry
					break;
				case 5:
					g=TF& (FLAG & 0b0000000001000000); //zero
					break;
				case 6:
					g= TF & (FLAG & 0b0000000010000000); //sign
					break;
				case 7:
					g = TF & (FLAG & 0b0000000100000000); //overflow bit 8 din flag
					break;
			}
			return g;
		}

		int MAR = 0;
		

		private void Sequencer()
		{
			int stare=0;
			int indexDecoded = 0;

			while (true)
			{
				switch (stare)
				{
					case 0:
						MIR = MPM[MAR];
						stare = 1;
						break;
					case 1:
						int g = CalculateGFunction();
						Succesor();
						if (g == 1)
						{
							indexDecoded = DecodeIndex();
							MAR = (int)(((int)MIR & 0x07F) + indexDecoded);
						}

						else MAR++;
						if (((MIR >> 18) & 0x3) != 0)
						{
							stare = 2;
						}
						else stare = 0;
						DecodeClass();
						DecodeSBUS(indexDecoded);
						DecodeDBUS(indexDecoded);
						DecodeALU();
						DestinationRBUS();
						OtherOp();

						break;
					case 2:
						stare = 3;
						break;
					case 3:
						MemOp();
						stare = 0;
						break;
				}
			}
		}


		private void DisplayMemory()
		{
			MEM_textbox.Clear();
			foreach (var item in MEM) 
			{
				byte lsb = (byte)(item & 0xFF);
				byte msb = (byte)((item >> 8) & 0xFF); 

				string binaryLsb = Convert.ToString(lsb, 2).PadLeft(8, '0');
				string binaryMsb = Convert.ToString(msb, 2).PadLeft(8, '0');

				MEM_textbox.AppendText(binaryLsb + Environment.NewLine);
				MEM_textbox.AppendText(binaryMsb + Environment.NewLine);
			}
		}

		private int DecodeIndex()
		{
			int indexValue = (int)((MIR >> 8) & 0x07);
			int index = 0;
			switch (indexValue)
			{
				case 0:
					index = 0;
					break;
				case 1:
					index = DecodeClass();
					break;
				case 2:
					index = IR >> 10 & 0x03; //mod adr sursa
					break;
				case 3:
					index = IR >> 4 & 0x03; //mod adr destinatie
					break;
				case 4:
					index = IR >> 12 & 0x07; //opcode b1
					break;
				case 5:
					index = IR >> 8 & 0x01F; //opcode b2
					break;
				case 6:
					index = IR >> 7 & 0x01E; //opcode b3 si b4
					break;
				case 7:
					index = INTR << 2;
					break;
			}
			return index;
			
		}

		private int DecodeClass()
		{
			int CL=0;
			int bit15IR = (IR >> 15) & 1;
			int bit14IR = (IR >> 14) & 1;
			int bit13IR = (IR >> 13) & 1;

			int CL1 = bit15IR & bit14IR;
			int CL0 = bit15IR & ~bit13IR;

			CL = (CL1 << 1) + CL0;


			return CL;
		}

		private void DecodeSBUS(int index)
		{
			ulong SourceSbus = (MIR >> 32) & 0xF;
			switch (SourceSbus)
			{
				case 0:
					break;
				case 1:
					SBUS = FLAG;
					break;
				case 2:
					SBUS = REG[index]; //nu stiu ce index trebuie pus aici (si nici la dbus)
					break;
				case 3:
					SBUS = SP;
					break;
				case 4:
					SBUS = T;
					break;
				case 5:
					SBUS = ~T;
					break;
				case 6:
					SBUS = PC;
					break;
				case 7:
					SBUS = IVR;
					break;
				case 8:
					SBUS = ADR;
					break;
				case 9:
					SBUS = MDR;
					break;
				case 10:
					SBUS = IR& 0x00FF;
					break;
				case 11:
					SBUS = zero;
					break;
				case 12:
					SBUS = minusOne;
					break;
			}
			SBUS_textbox.Clear();
			SBUS_textbox.AppendText(Convert.ToString(SBUS, 2).PadLeft(16, '0') + Environment.NewLine);
		}

		private void DecodeDBUS(int index)
		{
			ulong SourceDbus = (MIR >> 28) & 0xF;
			switch (SourceDbus)
			{
				case 0:
					break;
				case 1:
					DBUS = FLAG;
					break;
				case 2:
					DBUS = REG[index];
					break;
				case 3:
					DBUS = SP;
					break;
				case 4:
					DBUS = T;
					break;
				case 5:
					DBUS = PC;
					break;
				case 6:
					DBUS = IVR;
					break;
				case 7:
					DBUS = ADR;
					break;
				case 8:
					DBUS = MDR;
					break;
				case 9:
					DBUS = ~MDR;
					break;
				case 10:
					DBUS = IR & 0x00FF;
					break;
				case 11:
					DBUS = zero;
					break;
				case 12:
					DBUS = minusOne;
					break;
			}
			DBUS_textbox.Clear();
			DBUS_textbox.AppendText(Convert.ToString(DBUS, 2).PadLeft(16, '0') + Environment.NewLine);
		}

		private void DecodeALU()
		{
			ulong ALUOp = ((MIR >> 24) & 0xF);
			switch (ALUOp)
			{
				case 0: //NONE
					break;
				case 1: //SBUS
					RBUS = SBUS;
					break;
				case 2: //DBUS
					RBUS = DBUS;
					break;
				case 3: //ADD
					RBUS = SBUS + DBUS;
					break;
				case 4: //SUB
					RBUS = SBUS - DBUS;
					break;
				case 5: //AND
					RBUS = SBUS & DBUS;
					break;
				case 6: //OR
					RBUS = SBUS | DBUS;
					break;
				case 7: //XOR
					RBUS = SBUS ^ DBUS;
					break;
				case 8: // ASL
					if ((SBUS & 0b1000000000000000) == 0b1000000000000000) FLAG = FLAG | 0b0000000000000001; //carry flag pe 1
					RBUS = SBUS << 1;
					if ((RBUS & 0b1000000000000000) != 0)
						FLAG = FLAG | 0b0000000010000000; //punem sign flag pe 1
					else if (RBUS == 0) FLAG= FLAG | 0b0000000001000000; //punem zero flag pe 1
					break;
				case 9: //ASR
					if ((SBUS & 0b0000000000000001) == 0b0000000000000001) FLAG = FLAG | 0b0000000000000001; //carry flag pe 1
					RBUS = SBUS >> 1;
					if ((RBUS & 0b1000000000000000) != 0)
						FLAG = FLAG | 0b0000000010000000;
					else if (RBUS == 0) FLAG = FLAG | 0b0000000001000000;
					break;
				case 10: //LSR
					if ((SBUS & 0b0000000000000001) == 0b0000000000000001) FLAG = FLAG | 0b0000000000000001;
					RBUS = (ushort)(SBUS >> 1);

					if ((RBUS & 0b1000000000000000) != 0)
						FLAG = (ushort)(FLAG | 0b0000000010000000);
					else if (RBUS == 0) FLAG = (ushort)(FLAG | 0b0000000001000000);
					break;
				case 11: // ROL
					{
						bool msbSet = (SBUS & 0x8000) != 0;
						RBUS = (ushort)((SBUS << 1) | (msbSet ? 1 : 0));
						if (msbSet) FLAG |= 0b0000000000000001;
						if ((RBUS & 0x8000) != 0)
							FLAG |= 0b0000000010000000;
						else if (RBUS == 0)
							FLAG |= 0b0000000001000000;
					}
					break;
				case 12: // ROR
					{
						bool lsbSet = (SBUS & 0x0001) != 0;
						RBUS = (ushort)((SBUS >> 1) | (lsbSet ? 0x8000 : 0));
						if (lsbSet) FLAG |= 0b0000000000000001;
						if ((RBUS & 0x8000) != 0)
							FLAG |= 0b0000000010000000;
						else if (RBUS == 0)
							FLAG |= 0b0000000001000000;
					}
					break;
				case 13: // RLC
					{
						bool msbSet = (SBUS & 0x8000) != 0;
						bool carrySet = (FLAG & 0b0000000000000001) != 0;
						RBUS = (ushort)((SBUS << 1) | (carrySet ? 1 : 0));
						if (msbSet) FLAG |= 0b0000000000000001;
						if ((RBUS & 0x8000) != 0)
							FLAG |= 0b0000000010000000;
						else if (RBUS == 0)
							FLAG |= 0b0000000001000000;
					}
					break;
				case 14: //RRC
					{
						bool lsbSet = (SBUS & 0x0001) != 0;
						bool carrySet = (FLAG & 0b0000000000000001) != 0;
						RBUS = (ushort)((SBUS >> 1) | (carrySet ? 0x8000 : 0));
						if (lsbSet) FLAG |= 0b0000000000000001;
						if ((RBUS & 0x8000) != 0)
							FLAG |= 0b0000000010000000;
						else if (RBUS == 0)
							FLAG |= 0b0000000001000000;
					}
					break;
			}
			RBUS_textbox.Clear();
			RBUS_textbox.AppendText(Convert.ToString(RBUS, 2).PadLeft(16, '0') + Environment.NewLine);
		}

		private void DestinationRBUS()
		{
			int index = 2;
			ulong destRBUS = (MIR >> 20) & 0xF;
			switch (destRBUS)
			{
				case 0:
					break;
				case 1:
					FLAG = RBUS;
					FLAG_textbox.Clear();
					FLAG_textbox.AppendText(Convert.ToString(FLAG, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				case 2:
					FLAG = (ushort)(RBUS & 0x000F);
					FLAG_textbox.Clear();
					FLAG_textbox.AppendText(Convert.ToString(FLAG, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				case 3:
					REG[index] = RBUS;
					switch (index)
					{
						case 0:
							R0_textbox.Clear();
							R0_textbox.AppendText(Convert.ToString(REG[0], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 1:
							R1_textbox.Clear();
							R1_textbox.AppendText(Convert.ToString(REG[1], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 2:
							R2_textbox.Clear();
							R2_textbox.AppendText(Convert.ToString(REG[2], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 3:
							R3_textbox.Clear();
							R3_textbox.AppendText(Convert.ToString(REG[3], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 4:
							R4_textbox.Clear();
							R4_textbox.AppendText(Convert.ToString(REG[4], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 5:
							R5_textbox.Clear();
							R5_textbox.AppendText(Convert.ToString(REG[5], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 6:
							R6_textbox.Clear();
							R6_textbox.AppendText(Convert.ToString(REG[6], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 7:
							R7_textbox.Clear();
							R7_textbox.AppendText(Convert.ToString(REG[7], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 8:
							R8_textbox.Clear();
							R8_textbox.AppendText(Convert.ToString(REG[8], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 9:
							R9_textbox.Clear();
							R9_textbox.AppendText(Convert.ToString(REG[9], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 10:
							R10_textbox.Clear();
							R10_textbox.AppendText(Convert.ToString(REG[10], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 11:
							R11_textbox.Clear();
							R11_textbox.AppendText(Convert.ToString(REG[11], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 12:
							R12_textbox.Clear();
							R12_textbox.AppendText(Convert.ToString(REG[12], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 13:
							R13_textbox.Clear();
							R13_textbox.AppendText(Convert.ToString(REG[13], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 14:
							R14_textbox.Clear();
							R14_textbox.AppendText(Convert.ToString(REG[14], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
						case 15:
							R15_textbox.Clear();
							R15_textbox.AppendText(Convert.ToString(REG[15], 2).PadLeft(16, '0') + Environment.NewLine);
							break;
					}	
					break;
				case 4:
					SP = RBUS;
					SP_textbox.Clear();
					SP_textbox.AppendText(Convert.ToString(SP, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				case 5:
					T = RBUS;
					T_textbox.Clear();
					T_textbox.AppendText(Convert.ToString(T, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				case 6:
					PC = RBUS;
					PC_textbox.Clear();
					PC_textbox.AppendText(Convert.ToString(PC, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				case 7:
					IVR = RBUS;
					IVR_textbox.Clear();
					IVR_textbox.AppendText(Convert.ToString(IVR, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				case 8:
					ADR = RBUS;
					ADR_textbox.Clear();
					ADR_textbox.AppendText(Convert.ToString(ADR, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				case 9:
					MDR = RBUS;
					MDR_textbox.Clear();
					MDR_textbox.AppendText(Convert.ToString(MDR, 2).PadLeft(16, '0') + Environment.NewLine);
					break;

			}
		}

		
		private void IFCH()
		{
			IR = MEM[PC];
		}

		private void READ()
		{
			int address = ADR & 0xFFFF;
			if (address < 0 || address >= MEM.Length)
			{
				MessageBox.Show("Invalid memory address for READ operation.");
				return;
			}
			MDR = MEM[address];
			MDR_textbox.Clear();
			MDR_textbox.AppendText(Convert.ToString(MDR, 2).PadLeft(16, '0') + Environment.NewLine);
		}

		private void WRITE()
		{
			int address = ADR & 0xFFFF;
			if (address < 0 || address >= MEM.Length)
			{
				MessageBox.Show("Invalid memory address for WRITE operation.");
				return;
			}
			MEM[address] = MDR;
			DisplayMemory();
		}

		private void RUN_button_Click(object sender, EventArgs e)
		{
			Sequencer();
		}

		private void MemOp()
		{
			ulong memOp=(MIR>>18) & 0x3;
			switch (memOp)
			{
				case 0:
					break;
				case 1:
					IFCH();
					break;
				case 2:
					READ();
					break;
				case 3:
					WRITE();
					break;

			}
		}
		private void OtherOp()
		{
			ulong otherOp = (MIR >> 14) & 0xF;
			switch (otherOp)
			{
				case 0:
					break;
				case 1:
					SP = SP + 2;
					SP_textbox.Clear();
					SP_textbox.AppendText(Convert.ToString(SP, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				case 2:
					SP = SP - 2;
					SP_textbox.Clear();
					SP_textbox.AppendText(Convert.ToString(SP, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				case 3:
					PC = PC + 2;
					PC_textbox.Clear();
					PC_textbox.AppendText(Convert.ToString(PC, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				//mai trebuiesc implementate alte operatii
				case 4:
					//adu la 1 be[0]
					BE = BE | 0x01;
					break;
				case 5:
					//adu la 1 be[1]
					BE = BE | 0x02;
					break;
				case 6:
					//pd cond aritm pe rbus
					RBUS = COND;
					RBUS_textbox.Clear();
					RBUS_textbox.AppendText(Convert.ToString(RBUS, 2).PadLeft(16, '0') + Environment.NewLine);
					break;
				case 7:
					//cin pdCondaritm ???
					break;
				case 8:
					//adu la 1 bvi
					BVI = 1;
					break;
				case 9:
					//adu la 0 bvi
					BVI = 0;
					break;
				case 10:
					// INTA -2SP?????
					break;
				case 11:
					//adu la 0 be adu la 0 bi
					BE = 0;
					BI = 0;
					break;
			}

		}

		private void Step()
		{

		}
		private void JumpI()
		{
			
		}

		private void AcLowJumpI()
		{

		}
		private void CilJumpI()
		{

		}

		private void Succesor()
		{
			ulong succesor =(MIR >>11) & 0x7;
			switch (succesor)
			{
				case 0:
					Step();
					break;
				case 1:
					JumpI();
					break;
				case 2:
					AcLowJumpI();
					break;
				case 3:
					CilJumpI();
					break;
					//de implementat cazuri pt flag uri carry, overflow, zero, sign
			}
		}
	}
}
