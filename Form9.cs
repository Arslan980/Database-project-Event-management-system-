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
    public partial class CreateAccountForm : Form
    {
        OracleConnection con;
        public CreateAccountForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string email = textBox2.Text;
            string password = textBox3.Text;
            string confirm_password = textBox4.Text;

            if (name == "" || email == "" || password == "" || confirm_password == "") 
            {
                MessageBox.Show("please fill the missing terms");
            }
            else
            {
                if (confirm_password != password) 
                {
                    MessageBox.Show("Passwords are not maching");
                }
                if (checkBox1.Checked == false)
                {
                    MessageBox.Show("Please accept terms & conditions");
                }
                else
                {
                    user_regis();
                    MessageBox.Show("Registered Successfully! Now you can log in");
                    LoginPageForm loginForm = new LoginPageForm();
                    loginForm.ShowDialog();
                }
            }
        }

        private void CreateAccountForm_Load(object sender, EventArgs e)
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
        private void user_regis()
        {
            // Assuming 'con' is your OracleConnection object declared somewhere in your class.

            // Get the data from textboxes

            string user_name = textBox1.Text;
            string password = textBox3.Text;
            string email = textBox2.Text;
           

            // Construct the SQL query to insert data into the database with the sequence for USER_ID
            string insertQuery = "INSERT INTO LOGUP_INFO (IDS_ID,USER_NAME,PASSWORD,EMAIL) VALUES (IDS_ID_SEQ.NEXTVAL, :user_name, :password ,:email)";

            // Create an OracleCommand object
            using (OracleCommand cmd = new OracleCommand(insertQuery, con))
            {
                // Add parameters to the query to prevent SQL injection
                cmd.Parameters.Add(":user_name", OracleDbType.Char, 50).Value = user_name;
                cmd.Parameters.Add(":password", OracleDbType.Varchar2).Value = password;
                cmd.Parameters.Add(":email", OracleDbType.Varchar2).Value = email;



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
    }
}
