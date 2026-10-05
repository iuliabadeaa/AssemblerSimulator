namespace proiectAsm
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.pathTextbox = new System.Windows.Forms.TextBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.asmCodeTextbox = new System.Windows.Forms.TextBox();
			this.getAsmCode = new System.Windows.Forms.Button();
			this.generateTokensButton = new System.Windows.Forms.Button();
			this.tokensTextbox = new System.Windows.Forms.TextBox();
			this.machineCodeButton = new System.Windows.Forms.Button();
			this.machineCodeTextbox = new System.Windows.Forms.TextBox();
			this.saveObjButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// pathTextbox
			// 
			this.pathTextbox.Location = new System.Drawing.Point(32, 37);
			this.pathTextbox.Name = "pathTextbox";
			this.pathTextbox.Size = new System.Drawing.Size(314, 22);
			this.pathTextbox.TabIndex = 0;
			// 
			// browseButton
			// 
			this.browseButton.Location = new System.Drawing.Point(403, 37);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(142, 23);
			this.browseButton.TabIndex = 1;
			this.browseButton.Text = "BROWSE .ASM FILE";
			this.browseButton.UseVisualStyleBackColor = true;
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// asmCodeTextbox
			// 
			this.asmCodeTextbox.Location = new System.Drawing.Point(32, 145);
			this.asmCodeTextbox.Multiline = true;
			this.asmCodeTextbox.Name = "asmCodeTextbox";
			this.asmCodeTextbox.Size = new System.Drawing.Size(201, 261);
			this.asmCodeTextbox.TabIndex = 2;
			// 
			// getAsmCode
			// 
			this.getAsmCode.Location = new System.Drawing.Point(32, 91);
			this.getAsmCode.Name = "getAsmCode";
			this.getAsmCode.Size = new System.Drawing.Size(168, 23);
			this.getAsmCode.TabIndex = 3;
			this.getAsmCode.Text = "GET ASM CODE";
			this.getAsmCode.UseVisualStyleBackColor = true;
			this.getAsmCode.Click += new System.EventHandler(this.getAsmCode_Click);
			// 
			// generateTokensButton
			// 
			this.generateTokensButton.Location = new System.Drawing.Point(275, 91);
			this.generateTokensButton.Name = "generateTokensButton";
			this.generateTokensButton.Size = new System.Drawing.Size(135, 23);
			this.generateTokensButton.TabIndex = 4;
			this.generateTokensButton.Text = "TOKENIZE";
			this.generateTokensButton.UseVisualStyleBackColor = true;
			this.generateTokensButton.Click += new System.EventHandler(this.generateTokensButton_Click);
			// 
			// tokensTextbox
			// 
			this.tokensTextbox.Location = new System.Drawing.Point(275, 145);
			this.tokensTextbox.Multiline = true;
			this.tokensTextbox.Name = "tokensTextbox";
			this.tokensTextbox.Size = new System.Drawing.Size(201, 261);
			this.tokensTextbox.TabIndex = 5;
			// 
			// machineCodeButton
			// 
			this.machineCodeButton.Location = new System.Drawing.Point(534, 91);
			this.machineCodeButton.Name = "machineCodeButton";
			this.machineCodeButton.Size = new System.Drawing.Size(237, 23);
			this.machineCodeButton.TabIndex = 6;
			this.machineCodeButton.Text = "GENERATE MACHINE CODE";
			this.machineCodeButton.UseVisualStyleBackColor = true;
			this.machineCodeButton.Click += new System.EventHandler(this.machineCodeButton_Click);
			// 
			// machineCodeTextbox
			// 
			this.machineCodeTextbox.Location = new System.Drawing.Point(534, 145);
			this.machineCodeTextbox.Multiline = true;
			this.machineCodeTextbox.Name = "machineCodeTextbox";
			this.machineCodeTextbox.Size = new System.Drawing.Size(201, 261);
			this.machineCodeTextbox.TabIndex = 7;
			// 
			// saveObjButton
			// 
			this.saveObjButton.Location = new System.Drawing.Point(305, 415);
			this.saveObjButton.Name = "saveObjButton";
			this.saveObjButton.Size = new System.Drawing.Size(138, 31);
			this.saveObjButton.TabIndex = 8;
			this.saveObjButton.Text = "SAVE OBJ";
			this.saveObjButton.UseVisualStyleBackColor = true;
			this.saveObjButton.Click += new System.EventHandler(this.saveObjButton_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.saveObjButton);
			this.Controls.Add(this.machineCodeTextbox);
			this.Controls.Add(this.machineCodeButton);
			this.Controls.Add(this.tokensTextbox);
			this.Controls.Add(this.generateTokensButton);
			this.Controls.Add(this.getAsmCode);
			this.Controls.Add(this.asmCodeTextbox);
			this.Controls.Add(this.browseButton);
			this.Controls.Add(this.pathTextbox);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TextBox pathTextbox;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.TextBox asmCodeTextbox;
		private System.Windows.Forms.Button getAsmCode;
		private System.Windows.Forms.Button generateTokensButton;
		private System.Windows.Forms.TextBox tokensTextbox;
		private System.Windows.Forms.Button machineCodeButton;
		private System.Windows.Forms.TextBox machineCodeTextbox;
		private System.Windows.Forms.Button saveObjButton;
	}
}

