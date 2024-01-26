using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Oracle.ManagedDataAccess.Client;


namespace WindowsFormsApp1
{
    public partial class ReportForm : Form
    {
        OracleConnection con;
        public int loggedInUserId;
        private DataTable eventDataTable;

        public ReportForm()
        {
            InitializeComponent();
            InitializeData();
           
        }
        private void InitializeData()
        {
            eventDataTable = new DataTable();
            eventDataTable.Columns.Add("EventName", typeof(string));
            eventDataTable.Columns.Add("Attendance", typeof(int));
            eventDataTable.Columns.Add("Revenue", typeof(decimal));
            eventDataTable.Columns.Add("Feedback", typeof(string));
            dataGridView1.DataSource = eventDataTable;
            dataGridView1.DataSource = eventDataTable;
        }
        private void LoadEventDataFromDatabase(int userId)
        {
            try
            {
                // Construct the SQL query to select data for a specific user from the REPORT_INFO table
                string selectQuery = "SELECT EVENT_NAME, ATTENDANCE, REVENUE, FEEDBACK FROM REPORT_INFO WHERE IDS_ID = :userId";

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

                    // Set the eventDataTable DataSource to the DataTable
                    eventDataTable = dataTable;

                    // Set the DataGridView DataSource to the eventDataTable
                    dataGridView1.DataSource = eventDataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading event data from the database: " + ex.Message);
            }
        }



        private void Form6_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/xe;USER ID=f219312;PASSWORD=f219312";
            con = new OracleConnection(conStr);

            try
            {

                con.Open();
                // Connection is now open and ready to use.
                LoadEventDataFromDatabase(loggedInUserId);

            }
            catch (Exception ex)
            {
                // Handle connection opening errors.
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }

            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            user_regis();
            string eventName = comboBox1.Text;
            int attendance = (int)numericUpDown1.Value;
            string revenue = textBox2.Text;
            string feedback = textBox1.Text;
            eventDataTable.Rows.Add(eventName, attendance, revenue, feedback);
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                DataRow selectedRow = eventDataTable.Rows[selectedIndex];

                // Update the event in the database
                UpdateEventInDatabase(selectedRow);

                // Update the event in the DataTable
                selectedRow["Event_Name"] = comboBox1.Text;
                selectedRow["Attendance"] = numericUpDown1.Value;
                selectedRow["Revenue"] = textBox2.Text;
                selectedRow["Feedback"] = textBox1.Text;

                // Update the DataGridView
                dataGridView1.Refresh();
            }
        }

        private void UpdateEventInDatabase(DataRow eventRow)
        {
            try
            {
                // Construct the SQL query to update the event in the database
                string updateQuery = "UPDATE REPORT_INFO SET EVENT_NAME = :newEventName, ATTENDANCE = :newAttendance, REVENUE = :newRevenue, FEEDBACK = :newFeedback WHERE EVENT_NAME = :oldEventName AND ATTENDANCE = :oldAttendance AND REVENUE = :oldRevenue AND FEEDBACK = :oldFeedback ";

                // Create an OracleCommand object
                using (OracleCommand cmd = new OracleCommand(updateQuery, con))
                {
                    // Add parameters to the query
                    cmd.Parameters.Add(":newEventName", OracleDbType.Varchar2, 50).Value = comboBox1.Text;
                    cmd.Parameters.Add(":newAttendance", OracleDbType.Decimal).Value = (int)numericUpDown1.Value;
                    cmd.Parameters.Add(":newRevenue", OracleDbType.Varchar2, 50).Value = textBox2.Text;
                    cmd.Parameters.Add(":newFeedback", OracleDbType.Varchar2, 500).Value = textBox1.Text;
                    cmd.Parameters.Add(":oldEventName", OracleDbType.Varchar2, 50).Value = eventRow["EVENT_NAME"];
                    cmd.Parameters.Add(":oldAttendance", OracleDbType.Decimal).Value = eventRow["ATTENDANCE"];
                    cmd.Parameters.Add(":oldRevenue", OracleDbType.Varchar2, 50).Value = eventRow["REVENUE"];
                    cmd.Parameters.Add(":oldFeedback", OracleDbType.Varchar2, 500).Value = eventRow["FEEDBACK"];
               

                    // Open the connection if not open already
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    // Execute the query
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Event updated successfully in the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating event in the database: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                DataRow selectedRow = eventDataTable.Rows[selectedIndex];

                // Delete from the database
                DeleteEventFromDatabase(selectedRow);

                // Remove the event from the DataTable
                eventDataTable.Rows.RemoveAt(selectedIndex);

                // Update the DataGridView
                dataGridView1.Refresh();
            }
        }

        private void DeleteEventFromDatabase(DataRow eventRow)
        {
            try
            {
                // Construct the SQL query to delete the event from the database
                string deleteQuery = "DELETE FROM REPORT_INFO WHERE EVENT_NAME = :eventName AND ATTENDANCE = :attendance AND REVENUE = :revenue AND FEEDBACK = :feedback ";

                // Create an OracleCommand object
                using (OracleCommand cmd = new OracleCommand(deleteQuery, con))
                {
                    // Add parameters to the query
                    cmd.Parameters.Add(":eventName", OracleDbType.Varchar2, 50).Value = eventRow["EVENT_NAME"];
                    cmd.Parameters.Add(":attendance", OracleDbType.Decimal).Value = eventRow["ATTENDANCE"];
                    cmd.Parameters.Add(":revenue", OracleDbType.Varchar2, 50).Value = eventRow["REVENUE"];
                    cmd.Parameters.Add(":feedback", OracleDbType.Varchar2, 500).Value = eventRow["FEEDBACK"];
                    

                    // Open the connection if not open already
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    // Execute the query
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Event deleted successfully from the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting event from the database: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FillReportData();
            reportViewer1.LocalReport.ReportPath = "D:\\5th Smester\\Database Lab\\Final Project\\WindowsFormsApp1\\WindowsFormsApp1\\Report1.rdlc";
            reportViewer1.RefreshReport();
        }

        private void FillReportData()
        {
            DataSetEvent eventData = new DataSetEvent();
            foreach (DataRow row in eventDataTable.Rows)
            {
                eventData.EventData.Rows.Add(row.ItemArray);
            }
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", eventData.Tables[0]));
        }
        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
        private void user_regis()
        {
            // Get the data from your form or controls

            string eventName = comboBox1.Text;
            decimal attendance = numericUpDown1.Value;
            string revenue = textBox2.Text;
            string feedback = textBox1.Text;
            int userId = loggedInUserId;

            // Construct the SQL query to insert data into the database
            string insertQuery = "INSERT INTO REPORT_INFO (REPORT_ID, EVENT_NAME, ATTENDANCE, REVENUE, FEEDBACK, IDS_ID) VALUES (REPORT_ID_SEQ.NEXTVAL, :eventName, :attendance, :revenue, :feedback, :userId)";

            // Create an OracleCommand object
            using (OracleCommand cmd = new OracleCommand(insertQuery, con))
            {
                // Add parameters to the query to prevent SQL injection
                cmd.Parameters.Add(":eventName", OracleDbType.Varchar2, 50).Value = eventName;
                cmd.Parameters.Add(":attendance", OracleDbType.Decimal).Value = attendance;
                cmd.Parameters.Add(":revenue", OracleDbType.Varchar2, 50).Value = revenue;
                cmd.Parameters.Add(":feedback", OracleDbType.Varchar2, 500).Value = feedback;
                cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;

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
            }
            // Connection will be automatically closed when leaving the using block
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
