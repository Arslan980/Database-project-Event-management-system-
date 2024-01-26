using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class TitlePageForm : Form
    {
        OracleConnection con;
        public int loggedInUserId;
        public TitlePageForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/xe;USER ID=f219312;PASSWORD=f219312";
            con = new OracleConnection(conStr);

            try
            {
                MessageBox.Show("open" );
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
            EventDetailsForm eventDetailsForm = new EventDetailsForm();
          
            eventDetailsForm.loggedInUserId = loggedInUserId;
            eventDetailsForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ParticipantRegisterationForm participantregistrationForm = new ParticipantRegisterationForm();
            participantregistrationForm.loggedInUserId = loggedInUserId;
            participantregistrationForm.ShowDialog();
        }

   

        private void button3_Click_1(object sender, EventArgs e)
        {
              SchedulingForm schedulingForm = new SchedulingForm();
            schedulingForm.loggedInUserId = loggedInUserId;
            schedulingForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SpeakerForm speakerForm = new SpeakerForm();
            speakerForm.loggedInUserId = loggedInUserId;
            speakerForm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            reportForm.loggedInUserId = loggedInUserId;
            reportForm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ParticipantManagementForm reportForm = new ParticipantManagementForm();
            reportForm.loggedInUserId = loggedInUserId;
            reportForm.ShowDialog();
        }

    }
}
