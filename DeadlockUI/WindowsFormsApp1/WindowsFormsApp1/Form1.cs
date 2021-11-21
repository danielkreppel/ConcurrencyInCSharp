using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //change this method to "async void" and the "task.Wait();" to "await task;" to prevent the deadlock
        private void btnDeadlock_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Status: Running...";
            
            Task task = RunThisAsync();
            
            //Deadlock! Will block the UI Thread,
            //which cannot be used anymore to resume the "RunThisAsync"
            task.Wait();

            //This will not be called
            lblStatus.Text = "Status: Finished";
        }

        async Task RunThisAsync()
        {
            //Execute some I/O operation which will
            //require some time to be accomplished externally to your app
            var httpClient = new HttpClient();
            await httpClient.GetAsync("http://www.google.com")// .ConfigureAwait(false); //To prevent deadlock

            //You can also test like this:
            //await Task.Delay(TimeSpan.FromSeconds(1));
        }

    }
}
