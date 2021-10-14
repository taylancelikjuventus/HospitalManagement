using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace HospitalManagementSystem
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        /****************CONNECT TO MSSQL*************************/
        private SqlConnection connectToMSSQL()
        {
            //connect to MSSQL and show available ID on patientID label
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "data source= DESKTOP-C8BKT6R\\SQLEXPRESS01;database=hospitalmanagementsystem;integrated security=true;";
            conn.Open(); //open connection

            return conn;
        }

        /*******************If Add New Patient Clicked*********************/
        private void btnAddNewPatient_Click(object sender, EventArgs e)
        {
            //change color of its indicator
            labelIndic1.ForeColor = Color.Orange;
            labelIndic2.ForeColor = Color.CornflowerBlue;
            labelIndic3.ForeColor = Color.CornflowerBlue;
            labelIndic4.ForeColor = Color.CornflowerBlue;

          
            Dashboard_Load(this, null);
            panelAddNewPatient.Visible = true; //PARENTI olan Background Resmi  VISIBLE O yüzden Bu panel Görünür

            //connect to MSSQL and show available ID on patientID label
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "data source= DESKTOP-C8BKT6R\\SQLEXPRESS01;database=hospitalmanagementsystem;integrated security=true;";
            conn.Open(); //open connection

            // We return max number of id and show its increased value on patientID label
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select max(patientID) from AddPatient";

            SqlDataAdapter DA = new SqlDataAdapter(cmd);
            DataSet DS = new DataSet();
            DA.Fill(DS);

            //if there is any row in table
            if (DS.Tables[0].Rows[0][0].ToString() != "")
            {
                string returnedID = DS.Tables[0].Rows[0][0].ToString();
                Console.WriteLine("ID = " + returnedID);
                labelPatientID.Text = (int.Parse(returnedID) + 1).ToString();

            }
            else
            {
                labelPatientID.Text = "1";
            }

            conn.Close();//close Connection
        }


        /*******************If Add Diagnosis Clicked*********************/
        private void btnAddDiagnosis_Click(object sender, EventArgs e)
        {
            //change color of its indicator
            labelIndic2.ForeColor = Color.Orange;
            labelIndic1.ForeColor = Color.CornflowerBlue;
            labelIndic3.ForeColor = Color.CornflowerBlue;
            labelIndic4.ForeColor = Color.CornflowerBlue;

            //Show its Own Panel
            Dashboard_Load(this, null);
            panelAddNewPatient.Visible = true;  //Bu panel Alttakinin PArentidir.ÇünküBir alttaki panel Bunun üzerine yapıştırıldı.
                                                //O yüzden Bir bu panelin VISIBLE olması gerekir yoksa bunun child 'ları VISIBLE olsa bile görünmez
            panelAddDiagnosis.Visible = true;   //KENDISI VISIBLE ANCAK TUM PARENTLERININDE VISIBLE OLMASI GEREKIR
            
            
            
        }

        /*******************If Full History Of Patient Patient Clicked*********************/
        private void btnFullHistory_Click(object sender, EventArgs e)
        {
            //change color of its indicator
            labelIndic3.ForeColor = Color.Orange;
            labelIndic2.ForeColor = Color.CornflowerBlue;
            labelIndic1.ForeColor = Color.CornflowerBlue;
            labelIndic4.ForeColor = Color.CornflowerBlue;


            Dashboard_Load(this,null);
            panelAddNewPatient.Visible = true; //PARENT OF panelAddDiagnosis
            panelAddDiagnosis.Visible = true;  //PARENT OF panelFullHistory
            panelFullHistory.Visible = true;   //This Child is NOW visible and can be seen because its all parents are VISIBLE

            //SLEEP till table is ready
            Thread.Sleep(450);


            try
            {
                //Show all patients that are registered
                SqlConnection conn = connectToMSSQL();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from AddPatient inner join AddDiagnosis ON AddPatient.patientID = AddDiagnosis.PatientID";

                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                DA.Fill(DS);

                dataGridViewFullHistory.DataSource = DS.Tables[0];
              
                

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        /*******************If Hospital Information Clicked*********************/
        private void btnHospInfo_Click(object sender, EventArgs e)
        {
            //change color of its indicator
            labelIndic4.ForeColor = Color.Orange;
            labelIndic2.ForeColor = Color.CornflowerBlue;
            labelIndic3.ForeColor = Color.CornflowerBlue;
            labelIndic1.ForeColor = Color.CornflowerBlue;

            //Show all parents and this panel itself
            Dashboard_Load(this, null);
            panelAddNewPatient.Visible = true;
            panelAddDiagnosis.Visible = true;
            panelFullHistory.Visible = true;
            panelHospitalInformation.Visible = true;

        }

        /*************IF Exit Clicked***************/
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to quit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        /*************IF Dashboard is loaded hide all other panels but show background piicture********/
        private void Dashboard_Load(object sender, EventArgs e)
        {
            //HIDE all panels when page loaded then make them visible later when their
            //buttons on the left side of Dashboard Form is clicked
            panelAddNewPatient.Visible = false;
            panelAddDiagnosis.Visible = false;
            panelFullHistory.Visible = false;
            panelHospitalInformation.Visible = false;
          
        }


        /***************IF Save is clicked******************/
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string addr = txtAddress.Text;
            Int64 contact = Int64.Parse(txtContact.Text);
            int age = int.Parse(txtAge.Text);
            string gender = comboBoxGender.SelectedItem.ToString();
            string blood_g = comboBoxBloodGroup.SelectedItem.ToString();
            string major_disease = txtMajorDisease.Text;

            //connect to MSSQL and INSERT fields
            try
            {
                SqlConnection conn = connectToMSSQL();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "insert into AddPatient(pat_name,contact,age,gender,blood_group,major_disease) VALUES('" + name + "','" + contact + "','" + age + "','" + gender + "','" + blood_g + "','" + major_disease + "')";
                cmd.ExecuteNonQuery();
                conn.Close();

                //inform user
                MessageBox.Show(this, "Record inserted ...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //clear fields
                txtName.Clear();
                txtMajorDisease.Clear();
                txtContact.Clear();
                txtAge.Clear();
                txtAddress.Clear();
                comboBoxGender.ResetText();
                comboBoxBloodGroup.ResetText();

                //refresh Available PatientID number
                btnAddNewPatient_Click(this, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //refresh Available PatientID number
                btnAddNewPatient_Click(this, null);
            }

        }

        /**********IF searchbox is clear,clear table*****************/
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          if(textBox1.Text == "")
            {
                //clear table
                dataGridViewAddDiagnosis.DataSource = null;

                //clear fields
                txtsmpt.Clear();
                txtMedicine.Clear();
                txtDiag.Clear();
                comboBoxWrequired.ResetText();
                comboBoxWardBuilding.ResetText();
                comboBoxWardType.ResetText();
                comboBoxRoom.ResetText();
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //
            try
            {
                SqlConnection conn = connectToMSSQL();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from AddPatient WHERE patientID='"+textBox1.Text+"'";

                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                DA.Fill(DS);

                if (DS.Tables[0].Rows.Count != 0)
                    dataGridViewAddDiagnosis.DataSource = DS.Tables[0];
                else
                {
                    MessageBox.Show(this, "No such a patient found...","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error occured...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /***************IF Save Button is Clicked******************************/

        //NOT: always check gor combobox.text because if you checke for selected item
        //     then user must be select item from combobox OTHERWISE it fires NULL pointer error
        //     WHEN inseryin DAta from Combobox into Database table
        private void btnDiagSave_Click(object sender, EventArgs e)
        {
            //validate
            if (
                txtsmpt.Text != "" &&
                txtDiag.Text != "" &&
                txtMedicine.Text != "" &&
                dataGridViewAddDiagnosis.DataSource !=null //eger search ile hasta bulunamadıysa burası null olur
                )
            {
                //If Ward is required check required fields related to ward info
                if(comboBoxWrequired.SelectedItem.ToString() == "Yes")
                {

                    if (
                        comboBoxWardType.Text == "" ||
                        comboBoxBuildFloor.Text == "" ||
                        comboBoxRoom.Text == "" ||
                        comboBoxWardBuilding.Text == ""
                      )
                    {
                        MessageBox.Show(this, "Fill all Ward fields !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {


                        //insert fields into MSSQL's table , table = AddDiagnosis
                        //inserting default values for Ward field

                        SqlConnection conn = connectToMSSQL();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;

                        try
                        {
                            cmd.CommandText = "insert into AddDiagnosis(patientID,symptoms,diagnosis,medicines,ward_req,ward_type,ward_building,ward_room) values('" + int.Parse(textBox1.Text) + "','" + txtsmpt.Text + "','" + txtDiag.Text + "','" + txtMedicine.Text + "','"
                                + comboBoxWrequired.Text + "','" + comboBoxWardType.Text + "','" + comboBoxWardBuilding.Text + "','" + comboBoxRoom.Text + "')";

                            cmd.ExecuteNonQuery();
                            conn.Close();

                            //inform user
                            MessageBox.Show(this, "record inserted ...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //clear fields
                            txtsmpt.Clear();
                            txtDiag.Clear();
                            txtMedicine.Clear();
                            comboBoxWrequired.ResetText();  
                            comboBoxBuildFloor.ResetText();  
                            comboBoxRoom.ResetText();  
                            comboBoxWardBuilding.ResetText();  
                            comboBoxWardType.ResetText();  
                        
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }


                }
                else
                {
                    //insert fields into MSSQL's table , table = AddDiagnosis
                    //inserting default values for Ward field

                    SqlConnection conn = connectToMSSQL();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    try
                    {
                        cmd.CommandText = "insert into AddDiagnosis(patientID,symptoms,diagnosis,medicines) values('"+textBox1.Text+"','"+txtsmpt.Text+"','"+txtDiag.Text+"','"+txtMedicine.Text+"')";

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        //inform user
                        MessageBox.Show(this, "record inserted ...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    
                 }

            }
            else
            {
                MessageBox.Show(this, "Please fill required fields !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



        }
    }
}