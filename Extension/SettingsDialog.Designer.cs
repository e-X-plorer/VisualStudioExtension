namespace Extension
{
    partial class SettingsDialog
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
            this.AnalyzerToggleCheckbox = new System.Windows.Forms.CheckBox();
            this.MaxLinesInputField = new System.Windows.Forms.MaskedTextBox();
            this.MaxLinesText = new System.Windows.Forms.Label();
            this.MaxMembersText = new System.Windows.Forms.Label();
            this.MaxMembersInputField = new System.Windows.Forms.MaskedTextBox();
            this.RevertButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AnalyzerToggleCheckbox
            // 
            this.AnalyzerToggleCheckbox.AutoSize = true;
            this.AnalyzerToggleCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AnalyzerToggleCheckbox.Location = new System.Drawing.Point(12, 12);
            this.AnalyzerToggleCheckbox.Name = "AnalyzerToggleCheckbox";
            this.AnalyzerToggleCheckbox.Size = new System.Drawing.Size(353, 20);
            this.AnalyzerToggleCheckbox.TabIndex = 0;
            this.AnalyzerToggleCheckbox.Text = "Enable code analysis and code fix suggestions";
            this.AnalyzerToggleCheckbox.UseVisualStyleBackColor = true;
            this.AnalyzerToggleCheckbox.CheckedChanged += new System.EventHandler(this.AnalyzerToggleCheckbox_CheckedChanged);
            // 
            // MaxLinesInputField
            // 
            this.MaxLinesInputField.BeepOnError = true;
            this.MaxLinesInputField.Culture = new System.Globalization.CultureInfo("");
            this.MaxLinesInputField.Enabled = false;
            this.MaxLinesInputField.Location = new System.Drawing.Point(251, 38);
            this.MaxLinesInputField.Mask = "099999";
            this.MaxLinesInputField.Name = "MaxLinesInputField";
            this.MaxLinesInputField.Size = new System.Drawing.Size(42, 20);
            this.MaxLinesInputField.TabIndex = 1;
            this.MaxLinesInputField.ValidatingType = typeof(int);
            this.MaxLinesInputField.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.MaxLinesInputField_TypeValidationCompleted);
            // 
            // MaxLinesText
            // 
            this.MaxLinesText.AutoSize = true;
            this.MaxLinesText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MaxLinesText.Location = new System.Drawing.Point(30, 39);
            this.MaxLinesText.Name = "MaxLinesText";
            this.MaxLinesText.Size = new System.Drawing.Size(186, 16);
            this.MaxLinesText.TabIndex = 2;
            this.MaxLinesText.Text = "Maximum line count in a class:";
            // 
            // MaxMembersText
            // 
            this.MaxMembersText.AutoSize = true;
            this.MaxMembersText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MaxMembersText.Location = new System.Drawing.Point(30, 65);
            this.MaxMembersText.Name = "MaxMembersText";
            this.MaxMembersText.Size = new System.Drawing.Size(215, 16);
            this.MaxMembersText.TabIndex = 3;
            this.MaxMembersText.Text = "Maximum member count in a class:";
            // 
            // MaxMembersInputField
            // 
            this.MaxMembersInputField.BeepOnError = true;
            this.MaxMembersInputField.Enabled = false;
            this.MaxMembersInputField.Location = new System.Drawing.Point(251, 64);
            this.MaxMembersInputField.Mask = "099999";
            this.MaxMembersInputField.Name = "MaxMembersInputField";
            this.MaxMembersInputField.Size = new System.Drawing.Size(42, 20);
            this.MaxMembersInputField.TabIndex = 4;
            this.MaxMembersInputField.ValidatingType = typeof(int);
            this.MaxMembersInputField.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.MaxMembersInputField_TypeValidationCompleted);
            // 
            // RevertButton
            // 
            this.RevertButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RevertButton.Location = new System.Drawing.Point(12, 106);
            this.RevertButton.Name = "RevertButton";
            this.RevertButton.Size = new System.Drawing.Size(175, 23);
            this.RevertButton.TabIndex = 5;
            this.RevertButton.Text = "Revert";
            this.RevertButton.UseVisualStyleBackColor = true;
            this.RevertButton.Click += new System.EventHandler(this.RevertButton_Click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ApplyButton.Location = new System.Drawing.Point(197, 106);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(175, 23);
            this.ApplyButton.TabIndex = 6;
            this.ApplyButton.Text = "Apply changes";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(384, 141);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.RevertButton);
            this.Controls.Add(this.MaxMembersInputField);
            this.Controls.Add(this.MaxMembersText);
            this.Controls.Add(this.MaxLinesText);
            this.Controls.Add(this.MaxLinesInputField);
            this.Controls.Add(this.AnalyzerToggleCheckbox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Partial class analyzer settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox AnalyzerToggleCheckbox;
        private System.Windows.Forms.MaskedTextBox MaxLinesInputField;
        private System.Windows.Forms.Label MaxLinesText;
        private System.Windows.Forms.Label MaxMembersText;
        private System.Windows.Forms.MaskedTextBox MaxMembersInputField;
        private System.Windows.Forms.Button RevertButton;
        private System.Windows.Forms.Button ApplyButton;
    }
}