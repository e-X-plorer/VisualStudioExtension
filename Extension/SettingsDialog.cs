using System;
using System.Windows.Forms;
using ClassLengthAnalyzer;

namespace Extension
{
    public partial class SettingsDialog : Form
    {
        private bool _enabledSetting;
        private int _maxLinesSetting;
        private int _maxMembersSetting;

        public SettingsDialog()
        {
            InitializeComponent();
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            _enabledSetting = Settings.Default.Enabled;
            _maxLinesSetting = Settings.Default.MaxLinesCount;
            _maxMembersSetting = Settings.Default.MaxMemberCount;
            AnalyzerToggleCheckbox.Checked = _enabledSetting;
            MaxLinesInputField.Text = _maxLinesSetting.ToString();
            MaxMembersInputField.Text = _maxMembersSetting.ToString();
        }

        private void AnalyzerToggleCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            MaxLinesInputField.Enabled = AnalyzerToggleCheckbox.Checked;
            MaxMembersInputField.Enabled = AnalyzerToggleCheckbox.Checked;
            _enabledSetting = AnalyzerToggleCheckbox.Checked;
        }

        private void MaxLinesInputField_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            _maxLinesSetting = (int)e.ReturnValue;
        }

        private void MaxMembersInputField_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            _maxMembersSetting = (int)e.ReturnValue;
        }

        private void RevertButton_Click(object sender, EventArgs e)
        {
            Settings.Default.Reload();
            InitializeSettings();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            Settings.Default.Enabled = _enabledSetting;
            Settings.Default.MaxLinesCount = _maxLinesSetting;
            Settings.Default.MaxMemberCount = _maxMembersSetting;
            Settings.Default.Save();

            Close();
        }
    }
}
