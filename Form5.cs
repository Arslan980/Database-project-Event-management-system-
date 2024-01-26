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
    public partial class SpeakerForm : Form
    {
        OracleConnection con;
        public int loggedInUserId;
        class Speaker 
        {
            public string Name { get; set; }
            public string Expertise { get; set; }
            public string Availability { get; set; }
            public Speaker(string name, string expertise, string availability)
            {
                Name = name;
                Expertise = expertise;
                Availability = availability;
            }
        }

        private List<Speaker> speakers = new List<Speaker>();
        public SpeakerForm()
        {
            InitializeComponent();
            InitializeListView();
        }
        private void InitializeListView()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.Columns.Add("Name", 150);
            listView1.Columns.Add("Expertise", 200);
            listView1.Columns.Add("Availability", 150);
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/xe;USER ID=f219312;PASSWORD=f219312";
            con = new OracleConnection(conStr);

            try
            {
                con.Open();
                // Connection is now open and ready to use.
                LoadDataFromDatabase(loggedInUserId);

            }
            catch (Exception ex)
            {
                // Handle connection opening errors.
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }
        }
        private void LoadDataFromDatabase(int userId)
        {
            try
            {
                string selectQuery = "SELECT NAME, EXPERTISE, AVAILABILITY FROM SPEAKER_INFO WHERE IDS_ID = :userId";

                using (OracleCommand cmd = new OracleCommand(selectQuery, con))
                {
                    cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;

                    DataTable dataTable = new DataTable();

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }

                    speakers.Clear(); // Clear the existing list before loading new data

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string name = row["NAME"].ToString();
                        string expertise = row["EXPERTISE"].ToString();
                        string availability = row["AVAILABILITY"].ToString();

                        Speaker speaker = new Speaker(name, expertise, availability);
                        speakers.Add(speaker);
                    }

                    UpdateListView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data from the database: " + ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            user_regis();
            string name = textBox1.Text;
            string expertise = textBox2.Text;
            string availability = comboBox1.Text;
            Speaker newSpeaker = new Speaker(name, expertise, availability);
            speakers.Add(newSpeaker);
            UpdateListView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedIndex = listView1.SelectedIndices.Count > 0 ? listView1.SelectedIndices[0] : -1;
            if (selectedIndex >= 0)
            {
                // Update the ListView with the selected speaker's information
                UpdateSelectedSpeaker();

                // Now, update the selected speaker's properties
                Speaker selectedSpeaker = speakers[selectedIndex];
                selectedSpeaker.Name = textBox1.Text;
                selectedSpeaker.Expertise = textBox2.Text;
                selectedSpeaker.Availability = comboBox1.Text;

                // Update the ListView again to reflect the changes
                UpdateListView();
            }
        }

        private void UpdateSelectedSpeaker()
        {
            int selectedIndex = listView1.SelectedIndices.Count > 0 ? listView1.SelectedIndices[0] : -1;

            if (selectedIndex >= 0)
            {
                Speaker selectedSpeaker = speakers[selectedIndex];
                string newName = textBox1.Text;
                string newExpertise = textBox2.Text;
                string newAvailability = comboBox1.Text;

                // Update the speaker in the database
                UpdateSpeakerInDatabase(selectedSpeaker, newName, newExpertise, newAvailability);

                // Update the speaker in the list
                selectedSpeaker.Name = newName;
                selectedSpeaker.Expertise = newExpertise;
                selectedSpeaker.Availability = newAvailability;

                // Update the ListView
                UpdateListView();
            }
        }

        private void UpdateSpeakerInDatabase(Speaker speaker, string newName, string newExpertise, string newAvailability)
        {
            try
            {
                string updateQuery = "UPDATE SPEAKER_INFO SET NAME = :newName, EXPERTISE = :newExpertise, AVAILABILITY = :newAvailability WHERE SPEAKER_ID = :speakerId AND IDS_ID = :userId";

                using (OracleCommand cmd = new OracleCommand(updateQuery, con))
                {
                    cmd.Parameters.Add(":newName", OracleDbType.Varchar2, 50).Value = newName;
                    cmd.Parameters.Add(":newExpertise", OracleDbType.Varchar2, 50).Value = newExpertise;
                    cmd.Parameters.Add(":newAvailability", OracleDbType.Varchar2, 50).Value = newAvailability;
                    cmd.Parameters.Add(":speakerId", OracleDbType.Int32).Value = GetSpeakerId(speaker);
                    cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = loggedInUserId;

                    // Open the connection if not open already
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    // Execute the query
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating data in the database: " + ex.Message);
            }
            finally
            {
                // Close the connection
                con.Close();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            // Delete the selected speaker
            DeleteSelectedSpeaker();
        }

        private void DeleteSelectedSpeaker()
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                int selectedIndex = listView1.SelectedIndices[0];
                Speaker selectedSpeaker = speakers[selectedIndex];

                // Delete from the database
                DeleteSpeakerFromDatabase(selectedSpeaker);

                // Remove the speaker from the list
                speakers.RemoveAt(selectedIndex);

                // Update the ListView
                UpdateListView();
            }
        }

        private void DeleteSpeakerFromDatabase(Speaker speaker)
        {
            try
            {
                string deleteQuery = "DELETE FROM SPEAKER_INFO WHERE SPEAKER_ID = :speakerId AND IDS_ID = :userId";

                using (OracleCommand cmd = new OracleCommand(deleteQuery, con))
                {
                    cmd.Parameters.Add(":speakerId", OracleDbType.Int32).Value = GetSpeakerId(speaker); // Replace GetSpeakerId with the method to get the speaker ID
                    cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = loggedInUserId;

                    // Open the connection if not open already
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    // Execute the query
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting data from the database: " + ex.Message);
            }
            finally
            {
                // Close the connection
                con.Close();
            }
        }

        // Add a method to get the speaker ID
        private int GetSpeakerId(Speaker speaker)
        {
            // You need to implement the logic to retrieve the speaker ID based on the provided speaker data.
            // For example, you might need to query the database to get the ID.

            // Replace this with your actual logic
            // For demonstration purposes, assume you have a method to query the ID
            return GetSpeakerIdFromDatabase(speaker.Name, speaker.Expertise, speaker.Availability);
        }

        // Example method to get the speaker ID from the database
        private int GetSpeakerIdFromDatabase(string name, string expertise, string availability)
        {
            int speakerId = -1; // Set a default value

            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                string selectQuery = "SELECT SPEAKER_ID FROM SPEAKER_INFO WHERE NAME = :name OR EXPERTISE = :expertise OR AVAILABILITY = :availability AND IDS_ID = :userId";

                using (OracleCommand cmd = new OracleCommand(selectQuery, con))
                {
                    cmd.Parameters.Add(":name", OracleDbType.Varchar2, 50).Value = name;
                    cmd.Parameters.Add(":expertise", OracleDbType.Varchar2, 50).Value = expertise;
                    cmd.Parameters.Add(":availability", OracleDbType.Varchar2, 50).Value = availability;
                    cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = loggedInUserId;

                    // Execute the query to get the speaker ID
                    object result = cmd.ExecuteScalar();

                    // Convert the result to int if it's not null
                    if (result != null && int.TryParse(result.ToString(), out int parsedId))
                    {
                        speakerId = parsedId;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting speaker ID from the database: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return speakerId;
        }





        private void UpdateListView()
        {
            listView1.Items.Clear();
            foreach (Speaker speaker in speakers)
            {
                ListViewItem item = new ListViewItem(speaker.Name);
                item.SubItems.Add(speaker.Expertise);
                item.SubItems.Add(speaker.Availability);
                listView1.Items.Add(item);
            }
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";

            listView1.Refresh();
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void user_regis()
        {
            // Get the data from your form or controls

            string name = textBox1.Text;
            string expertise = textBox2.Text;
            string availability = comboBox1.Text;

            // Construct the SQL query to insert data into the database
            string insertQuery = "INSERT INTO SPEAKER_INFO (SPEAKER_ID, IDS_ID, NAME, EXPERTISE, AVAILABILITY) VALUES (SPEAKER_ID_SEQ.NEXTVAL, :userId, :name, :expertise, :availability)";

            // Create an OracleCommand object
            using (OracleCommand cmd = new OracleCommand(insertQuery, con))
            {
                // Add parameters to the query to prevent SQL injection
                cmd.Parameters.Add(":userId", OracleDbType.Int32).Value = loggedInUserId;
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, 50).Value = name;
                cmd.Parameters.Add(":expertise", OracleDbType.Varchar2, 50).Value = expertise;
                cmd.Parameters.Add(":availability", OracleDbType.Varchar2, 50).Value = availability;

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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
