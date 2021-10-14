using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       

        /*************IF Login Clicked*******************/
        private void button1_Click(object sender, EventArgs e)
        {

            string uname = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();

            //Validate User
            //We didn't use DB table to validate Username and password
            if (uname != "" && pass != "")
            {
             
                if (uname == "admin" && pass == "admin")
                {
                    //open up dashboard and hide this window
                    this.Visible = false;
                    Dashboard db = new Dashboard();
                    db.Show();

                }
                else
                {
                    MessageBox.Show(this, "Wrong Username or password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show(this, "Please Enter Username and Password to login System", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        

    }
}
