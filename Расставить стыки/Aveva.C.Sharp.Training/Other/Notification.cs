using COUPWELD.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tulpep.NotificationWindow;

namespace COUPWELD.Other
{
    public class NotificationClass
    {
        public void Notification(string titleText = "", string contentText = "")
        {
            try
            {
                var font = new Font("Microsoft Sans", 18.0f);
                var notification = new PopupNotifier
                {
                    TitleText = titleText,
                    ShowGrip = true,
                    ContentText = contentText,
                    ContentFont = font,
                    Size = new Size(600, 100),
                    Image = Resources.information48,
                    Scroll = true
                };

                notification.Popup();
                // notification.Click += (sender, args) => Focus();
            }
            catch (Exception)
            {
            }
        }
    }
}
