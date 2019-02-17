using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMainMold
{
    class SingleInstanceApplication : WindowsFormsApplicationBase
    {
        public SingleInstanceApplication()
        {
            IsSingleInstance = true;
        }

        protected override void OnCreateMainForm()
        {
            MainForm = new MainFrm();
        }
    }
}
