// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmProfile : DockFormBase
    {
        private string _profileName;
        private string _profileDir = string.Empty;
        private const string FILTER = "Profile Files(*.profile)|*.profile";
        private const string CAPTION = "CodeBuilder Preview";
        private readonly DevHosting _hosting;

        public frmProfile(DevHosting hosting)
        {
            InitializeComponent();
            Icon = Properties.Resources.profile;
            _profileDir = Path.Combine(hosting.WorkPath, "profiles");
            _hosting = hosting;
        }

        public Action PropertyChangeAct { get; set; }

        public void ReloadProfile()
        {
            propertyGrid1.SelectedObject = _hosting.Profile;
            RefreshInfo();
        }

        private void frmProfile_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = _hosting.Profile;
            RefreshInfo();
        }

        private void tlbOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = FILTER;
                dialog.InitialDirectory = _profileDir;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _profileName = dialog.FileName;
                    _hosting.Profile = ProfileUnity.LoadProfile(_hosting, _hosting.Template, _profileName);
                    propertyGrid1.SelectedObject = _hosting.Profile;
                    PropertyChangeAct?.Invoke();
                    RefreshInfo();
                }
            }
        }

        private void tlbSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_profileName))
            {
                ProfileUnity.SaveFile(_hosting, _hosting.Template?.TId, _hosting.Profile);
            }
            else
            {
                ProfileUnity.SaveAsFile(_hosting.Profile, _profileName);
            }
        }

        private void tlbSaveAs_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = FILTER;
                dialog.InitialDirectory = _profileDir;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _profileName = dialog.FileName;
                    ProfileUnity.SaveAsFile(_hosting.Profile, _profileName);
                }
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (PropertyChangeAct != null)
            {
                PropertyChangeAct();
            }
        }

        private void RefreshInfo()
        {
            if (_hosting.Profile is IProfileInfo info)
            {
                tlbInfo.ToolTipText = info.FileName;
            }
        }

        private void tlbHelp_Click(object sender, EventArgs e)
        {
            Process.Start("http://fireasy.cn/docs/codebuilder-profile");
        }
    }
}
