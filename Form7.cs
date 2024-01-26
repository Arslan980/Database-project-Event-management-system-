using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsApp1
{
    public partial class ParticipantManagementForm : Form
    {
        OracleConnection con;
        public int loggedInUserId;

        public ParticipantManagementForm()
        {
           
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ParticipantManagementForm_Load(object sender, EventArgs e)
        {

            string conStr = @"DATA SOURCE = localhost:1521/xe;USER ID=f219312;PASSWORD=f219312";
            con = new OracleConnection(conStr);

            try
            {

                con.Open();
                // Connection is now open and ready to use.

            }
            catch (Exception ex)
            {
                // Handle connection opening errors.
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string phone = textBox2.Text;
            string email = textBox3.Text;
            string payment = textBox4.Text;
            string status = comboBox1.Text;
            if (name == "" || phone == "" || email == "" || payment == "" || status == "") 
            {
                MessageBox.Show("please fill the missing terms");
            }
            else
            {
                if (checkBox1.Checked == false)
                {
                    MessageBox.Show("Please accept terms & conditions");
                }
                else
                {
                    MessageBox.Show("Registered Successfully");
                    user_regis();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void user_regis()
        {
            // Assuming 'con' is your OracleConnection object declared somewhere in your class.

            // Get the data from textboxes

            string name = textBox1.Text;
            string phone = textBox2.Text;
            string email = textBox3.Text;
            string payment = textBox4.Text;
            string status = comboBox1.Text;
            decimal numericValue = numericUpDown1.Value;
            int userId = loggedInUserId;

            // Construct the SQL query to insert data into the database with the sequence for USER_ID
            string insertQuery = "INSERT INTO PARTICIPANT_INFO (PARTICIPANT_ID, IDS_ID,  NAME, PHONE, EMAIL, PAYMENT, STATUS, ATTENDANCE) VALUES (PARTICIPANT_ID_SEQ.NEXTVAL, :userId, :name, :phone, :email, :payment, :status, :numericValue)";

            // Create an OracleCommand object
            using (OracleCommand cmd = new OracleCommand(insertQuery, con))
            {
                // Add parameters to the query to prevent SQL injection
                cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = loggedInUserId;

                cmd.Parameters.Add(":name", OracleDbType.Char, 50).Value = name;
                cmd.Parameters.Add(":phone", OracleDbType.Varchar2).Value = phone;
                cmd.Parameters.Add(":email", OracleDbType.Char, 50).Value = email;
                cmd.Parameters.Add(":payment", OracleDbType.Char, 50).Value = payment;
                cmd.Parameters.Add(":status", OracleDbType.Char, 50).Value = status;
                cmd.Parameters.Add(":numericValue", OracleDbType.Decimal).Value = numericValue;


                try
                {
                    // Open the connection if not open already
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    // Execute the query
                    cmd.ExecuteNonQuery();

                    // Optionally, you can display a message indicating successful insertion
                    MessageBox.Show("User registration data inserted successfully.");
                }
                catch (Exception ex)
                {
                    // Handle database insertion errors
                    MessageBox.Show("Error inserting user registration data: " + ex.Message);
                }
                finally
                {
                    // Close the connection
                    con.Close();
                }
            };

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
