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
using System.IO;
using System.Windows.Forms;

namespace CodeBuilder
{
    public partial class frmProfile : DockFormBase
    {
        private string _profileName;
        private string _profileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profiles");
        private const string FILTER = "Profile Files(*.profile)|*.profile";
        private const string CAPTION = "CodeBuilder Preview";
        private readonly IDevHosting _hosting;

        public frmProfile(IDevHosting hosting)
        {
            InitializeComponent();
            Icon = Properties.Resources.profile;
            _hosting = hosting;
        }

        public Action PropertyChangeAct { get; set; }

        public void ReloadProfile()
        {
            propertyGrid1.SelectedObject = _hosting.Profile;
        }

        private void frmProfile_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = _hosting.Profile;
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
                    //StaticUnity.Profile = ProfileUnity.LoadFile(profileName);
                    //propertyGrid1.SelectedObject = StaticUnity.Profile;
                    PropertyChangeAct?.Invoke();
                }
            }
        }

        private void tlbSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_profileName))
            {
                ProfileUnity.SaveFile(_hosting.Template?.TId, _hosting.Profile);
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
    }
}
