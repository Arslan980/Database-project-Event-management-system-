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
    public partial class EventDetailsForm : Form
    {
        OracleConnection con;
        public int loggedInUserId;
        public EventDetailsForm()
        {
            InitializeComponent();
            //this.loggedInUserId = loggedInUserId;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void EventDetailsForm_Load(object sender, EventArgs e)
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
            
            string time = textBox2.Text;
            string location = textBox3.Text;
            string description = textBox4.Text;
           
            if (time == "" || location == "" || description == "")
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void user_regis()
        {
            // Get the data from your form or controls
            DateTime eventDate = dateTimePicker1.Value;
            string time = textBox4.Text;
            string location = textBox3.Text;
            string description = textBox2.Text;

            // Use the loggedInUserId obtained from the constructor
           int userId = loggedInUserId;

          
            if (userId <= -1)
            {
                MessageBox.Show("Error getting logged-in user ID.");
                return;
            }
          

            // Determine the value for the RADIO_BUTTON_COLUMN based on RadioButton state
            string radioButtonValue = radioButton1.Checked ? "PM" : (radioButton2.Checked ? "AM" : "NULL");

            // Construct the SQL query to insert data into the database
            string insertQuery = "INSERT INTO EVENT_DETAIL (EVENT_ID, IDS_ID, EVENT_DATE, TIME, LOCATION, DESCRIPTION, RADIO_BUTTON_COLUMN) VALUES (EVENT_ID_SEQ.NEXTVAL,:userId, :eventDate, :time, :location, :description, :radioButtonValue)";

            // Create an OracleCommand object
            using (OracleCommand cmd = new OracleCommand(insertQuery, con))
            {
                // Add parameters to the query to prevent SQL injection
                cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = loggedInUserId;
                cmd.Parameters.Add(":eventDate", OracleDbType.Date).Value = eventDate;
                cmd.Parameters.Add(":time", OracleDbType.Varchar2, 50).Value = time;
                cmd.Parameters.Add(":location", OracleDbType.Varchar2, 50).Value = location;
                cmd.Parameters.Add(":description", OracleDbType.Varchar2, 50).Value = description;
                cmd.Parameters.Add(":radioButtonValue", OracleDbType.Varchar2, 50).Value = radioButtonValue;

                try
                {
                    // Open the connection if not open already
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    // Execute the query
                    cmd.ExecuteNonQuery();

                    // Optionally, you can display a message indicating successful insertion
                    MessageBox.Show("Event data inserted successfully.");
                }
                catch (Exception ex)
                {
                    // Handle database insertion errors
                    MessageBox.Show("Error inserting event data: " + ex.Message);
                }
                finally
                {
                    // Close the connection
                    con.Close();
                }
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
