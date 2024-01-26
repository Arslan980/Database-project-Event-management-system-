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
    public partial class SchedulingForm : Form
    {
        OracleConnection con;
        public int loggedInUserId;
        private DataTable sessionDataTable;

        public SchedulingForm()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            sessionDataTable = new DataTable();
            sessionDataTable.Columns.Add("SessionName", typeof(string));
            sessionDataTable.Columns.Add("StartTime", typeof(DateTime));
            sessionDataTable.Columns.Add("EndTime", typeof(DateTime));
            dataGridView2.DataSource = sessionDataTable;
        }

        private void LoadSessionDataFromDatabase(int userId)
        {
            try
            {
                // Construct the SQL query to select data for a specific user from the SCHEDULING table
                string selectQuery = "SELECT SESSION_NAME, START_TIME, END_TIME FROM SCHEDULING WHERE IDS_ID = :userId";

                // Create an OracleCommand object
                using (OracleCommand cmd = new OracleCommand(selectQuery, con))
                {
                    // Add parameters to the query
                    cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;

                    // Create a DataTable to store the retrieved data
                    DataTable dataTable = new DataTable();

                    // Create an OracleDataAdapter to fill the DataTable
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }

                    // Set the sessionDataTable DataSource to the DataTable
                    sessionDataTable = dataTable;

                    // Set the DataGridView DataSource to the sessionDataTable
                    dataGridView2.DataSource = sessionDataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading session data from the database: " + ex.Message);
            }
        }

        private void SchedulingForm_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/xe;USER ID=f219312;PASSWORD=f219312";
            con = new OracleConnection(conStr);

            try
            {
                con.Open();
                // Connection is now open and ready to use.
                LoadSessionDataFromDatabase(loggedInUserId);
            }
            catch (Exception ex)
            {
                // Handle connection opening errors.
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            user_regis();
            LoadSessionDataFromDatabase(loggedInUserId);
            string sessionName = textBox1.Text;
            DateTime startTime = dateTimePicker1.Value;
            DateTime endTime = dateTimePicker2.Value;

            sessionDataTable.Rows.Add(sessionName, startTime, endTime);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView2.SelectedRows[0].Index;
                DataRow selectedRow = sessionDataTable.Rows[selectedIndex];

                // Update the session in the database
                UpdateSessionInDatabase(selectedRow);

                // Update the session in the DataTable
                selectedRow["SESSION_NAME"] = textBox1.Text;
                selectedRow["START_TIME"] = dateTimePicker1.Value;
                selectedRow["END_TIME"] = dateTimePicker2.Value;

                // Update the DataGridView
                dataGridView2.Refresh();
            }
        }

        private void UpdateSessionInDatabase(DataRow sessionRow)
        {
            try
            {
                // Construct the SQL query to update the session in the database
                string updateQuery = "UPDATE SCHEDULING SET SESSION_NAME = :newSessionName, START_TIME = :newStartTime, END_TIME = :newEndTime WHERE SESSION_NAME = :oldSessionName AND START_TIME = :oldStartTime AND END_TIME = :oldEndTime AND IDS_ID = :userId";

                // Create an OracleCommand object
                using (OracleCommand cmd = new OracleCommand(updateQuery, con))
                {
                    // Add parameters to the query
                    cmd.Parameters.Add(":newSessionName", OracleDbType.Varchar2, 50).Value = textBox1.Text;
                    cmd.Parameters.Add(":newStartTime", OracleDbType.Date).Value = dateTimePicker1.Value;
                    cmd.Parameters.Add(":newEndTime", OracleDbType.Date).Value = dateTimePicker2.Value;
                    cmd.Parameters.Add(":oldSessionName", OracleDbType.Varchar2, 50).Value = sessionRow["SESSION_NAME"];
                    cmd.Parameters.Add(":oldStartTime", OracleDbType.Date).Value = sessionRow["START_TIME"];
                    cmd.Parameters.Add(":oldEndTime", OracleDbType.Date).Value = sessionRow["END_TIME"];
                    cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = loggedInUserId;

                    // Execute the query
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Session updated successfully in the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating session in the database: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView2.SelectedRows[0].Index;
                DataRow selectedRow = sessionDataTable.Rows[selectedIndex];

                // Delete from the database
                DeleteSessionFromDatabase(selectedRow);
                sessionDataTable.Rows.RemoveAt(selectedIndex);
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = sessionDataTable;
            }
        }

        private void DeleteSessionFromDatabase(DataRow sessionRow)
        {
            try
            {
                // Construct the SQL query to delete the session from the database
                string deleteQuery = "DELETE FROM SCHEDULING WHERE SESSION_NAME = :sessionName AND START_TIME = :startTime AND END_TIME = :endTime AND IDS_ID = :userId";

                // Create an OracleCommand object
                using (OracleCommand cmd = new OracleCommand(deleteQuery, con))
                {
                    // Add parameters to the query
                    cmd.Parameters.Add(":sessionName", OracleDbType.Varchar2, 50).Value = sessionRow["SESSION_NAME"];
                    cmd.Parameters.Add(":startTime", OracleDbType.Date).Value = sessionRow["START_TIME"];
                    cmd.Parameters.Add(":endTime", OracleDbType.Date).Value = sessionRow["END_TIME"];
                    cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = loggedInUserId;
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    // Execute the query
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Session deleted successfully from the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting session from the database: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void user_regis()
        {
            // Get the data from your form or controls

            string sessionName = textBox1.Text;
            DateTime startTime = dateTimePicker1.Value;
            DateTime endTime = dateTimePicker2.Value;

            // Construct the SQL query to insert data into the database
            string insertQuery = "INSERT INTO SCHEDULING (SCHEDULING_ID, IDS_ID, SESSION_NAME, START_TIME, END_TIME) VALUES (SCHEDULING_ID_SEQ.NEXTVAL, :userId, :sessionName, :startTime, :endTime)";

            // Create an OracleCommand object
            using (OracleCommand cmd = new OracleCommand(insertQuery, con))
            {
                // Add parameters to the query to prevent SQL injection
                cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = loggedInUserId;
                cmd.Parameters.Add(":sessionName", OracleDbType.Varchar2, 50).Value = sessionName;
                cmd.Parameters.Add(":startTime", OracleDbType.Date).Value = startTime;
                cmd.Parameters.Add(":endTime", OracleDbType.Date).Value = endTime;

                try
                {
                    // Open the connection if not open already
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    // Execute the query
                    cmd.ExecuteNonQuery();

                    // Optionally, you can display a message indicating successful insertion
                    MessageBox.Show("Session data inserted successfully.");
                }
                catch (Exception ex)
                {
                    // Handle database insertion errors
                    MessageBox.Show("Error inserting session data: " + ex.Message);
                }
                finally
                {
                    // Close the connection
                    con.Close();
                }
            }
        }
    }
}
