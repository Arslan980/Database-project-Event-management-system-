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
    public partial class LoginPageForm : Form
    {
        OracleConnection con;
        public LoginPageForm()
        {
            InitializeComponent();
        }
        public int LoggedInUserId { get; private set; } = -1;

        private void button1_Click(object sender, EventArgs e)
        {
            string user_name = textBox1.Text;
            string password = textBox2.Text;

            if (user_name == "" || password == "")
            {
                MessageBox.Show("Please Enter Correct Credentials");
            }
            else
            {
                // Check the credentials in the database
                int userId = CheckCredentials(user_name, password);

                if (userId != -1)
                {
                    // Set the logged-in user ID
                    LoggedInUserId = userId;

                    MessageBox.Show("Logged In Successfully! User ID: " + userId);
                    TitlePageForm titleForm = new TitlePageForm();
                    titleForm.loggedInUserId = userId;
                    titleForm.ShowDialog();

                    // Pass the user ID to the constructor of EventDetailsForm
                    
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.");
                }
            }
        }

        private int CheckCredentials(string userName, string password)
        {
            // Construct the SQL query to check credentials
            string query = "SELECT IDS_ID FROM LOGUP_INFO WHERE USER_NAME = :user_name AND PASSWORD = :password";

            using (OracleCommand cmd = new OracleCommand(query, con))
            {
                cmd.Parameters.Add(":user_name", OracleDbType.Char, 50).Value = userName;
                cmd.Parameters.Add(":password", OracleDbType.Varchar2).Value = password;

                try
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    // Execute the query
                    object result = cmd.ExecuteScalar();

                    // Check if the result is not null
                    if (result != null)
                    {
                        // Convert the result to integer (user ID)
                        return Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking credentials: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }

            // Return -1 if credentials are not valid
            return -1;
        }


        private void UserManagementForm_Load(object sender, EventArgs e)
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateAccountForm signupForm = new CreateAccountForm();
            signupForm.ShowDialog();
        }
        
    }
}
