using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PACEBuzz
{
    public partial class About : Form
    {
        public event FormClosedEventHandler MainFormShow;

        public About(FormClosedEventHandler onFormClosed)
        {
            InitializeComponent();
            this.MainFormShow += onFormClosed;
        }

        private void txtLicense_TextChanged(object sender, EventArgs e)
        {

        }

        private void About_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.MainFormShow != null)
            {
                this.MainFormShow.Invoke(sender, e);
            }
        }

        private void About_Load(object sender, EventArgs e)
        {
            this.lblTroubleshooting.Text = 
@"-Try plugging the buzzers into a different USB port.

-Run as an administrator.

-Check for updated versions of this program.

-If no sounds, make sure the Sounds directory exists in the same directory as PACEBuzz.exe.

-E-mail Michael Bentley at bentley.michael.j@gmail.com if you're still stuck.  Please mention what Operating System you have (for instance, Windows 10 64 Bit).";
            this.txtLicense.Text =
@"This software is licensed under the MIT License.

Copyright (c) 2016 Partnership for Academic Competition Excellence
http://www.pace-nsc.org

Buzzer drivers based on code written by Ashley Deakin available at http://www.developerfusion.com/article/84338/making-usb-c-friendly/

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.";
        }

        private void cmdVisitPACE_Click(object sender, EventArgs e)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "explorer.exe";
                p.StartInfo.Arguments = "http://www.pace-nsc.org";
                p.Start();
            }
        }
    }
}
