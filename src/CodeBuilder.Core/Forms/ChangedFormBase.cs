// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CodeBuilder.Core.Forms
{
    public class ChangedFormBase : DockFormBase, IChangeManager
    {
        public virtual bool IsChanged { get; set; }

        public virtual bool SaveChanges(bool notify)
        {
            return false;
        }

        protected virtual void ProcessChanged(bool changed)
        {
            IsChanged = changed;
            if (changed && !Text.EndsWith("*"))
            {
                Text = Text + " *";
            }
            if (!changed && Text.EndsWith("*"))
            {
                Text = Text.Replace(" *", string.Empty);
            }
        }
    }
}
