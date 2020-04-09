using System.Windows.Forms;
using LibVLCSharp.Shared;

namespace WindowsFormsControlLibrary1
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            Core.Initialize();
        }
    }
}
