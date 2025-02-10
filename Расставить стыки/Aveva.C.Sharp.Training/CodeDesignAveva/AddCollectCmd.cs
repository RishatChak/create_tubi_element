using System;
using System.Drawing;
using Aveva.ApplicationFramework.Presentation;


namespace Aveva.C.Sharp.Training
{
    class AddCollectCmd : Command
    {
        DockedWindow collform;

        [Obsolete]
        public AddCollectCmd(IWindowManager wndManager)
        {
            Key = "key";
            collform = wndManager.CreateDockedWindow("CMD", "Сварные стыки", new AddSelectPipe(), DockedPosition.Floating);
            collform.SaveLayout = true;
            collform.Width = 480;
            collform.Height = 360;

            Size currentMaxSize = collform.MaximumSize;
            Size newSize = new Size(480, 360); // Если нужно изменить только ширину
            collform.MaximumSize = newSize;

            wndManager.WindowLayoutLoaded += WndManager_WindowLoaded;
            collform.Closed += CollWindow_Closed;
            ExecuteOnCheckedChange = false;
        }

        void OnWindowLayoutLoaded(object sender, EventArgs e)
        {
            this.Checked = collform.Visible;
        }

        private void WndManager_WindowLoaded(object sender, EventArgs e)
        {
            Checked = collform.Visible;
        }

        private void CollWindow_Closed(object sender, EventArgs e)
        {
            Checked = false;
        }

        public override void Execute()
        {
            try
            {
                if (collform.Visible)
                    collform.Hide();
                else
                    collform.Show();
            }
            catch
            {

            }
        }
    }
}
