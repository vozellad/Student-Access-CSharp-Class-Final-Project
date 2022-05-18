/*
 * Name: Demetrios Vozella
 * Final Project
 * Course: CSI255 (Fall 2021)
 * Date: 12/17/2021
 * Description: GUI for handling student records. Students may be an 
 *      International student, a Veteran, or neither. 
 *      Student records and serialized/deserialized.
 *      GUI includes loading student from list, creating new student 
 *      with ID and type, editing student info, and saving or removing 
 *      student.
 *      
 *                          Student
 *                          v     v
 *       InternationalStudent     VeteranStudent
 *      
 * Requirements: 
 *      GUI:           This file. 
 *      Inheritance:   InternationalStudent or VeteranStudent 
 *                         being based on Student
 *      Polymorphism:  CalculateFinancialBalance
 *      Serialization: LoadStudents() and SaveStudents() in this file.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Students; // Student classes from dll
using System.Globalization; // For currency formatting.

namespace StudentAccess
{
    public partial class StudentAccessForm : Form
    {
        // Instance variables

        private const string filePath = "StudentRecords.xml";
        // Holds all students. Info goes from and to file.
        private List<Student> students = new List<Student>();

        // Sets format for currency.
        // Mainly used to output negative financial balances.
        CultureInfo culture;

        public StudentAccessForm()
        {
            InitializeComponent();
        }

        // Properties

        public List<Student> Students
        {
            get { return students; }
            set { students = value; }
        }

        // Methods

        private void LoadStudents()
        {
            // Loads in list of students from xml file via serializer. 

            FileStream outFile = null;
            try
            {
                outFile = new FileStream(
                    filePath, FileMode.Open, FileAccess.Read);

                XmlSerializer serializer = new XmlSerializer(
                    typeof(List<Student>));
                List<Student> inS = 
                    (List<Student>)serializer.Deserialize(outFile);
                Students.Clear();
                foreach (Student s in inS)
                    Students.Add(s);
            }
            catch (SerializationException serEx)
            {
                MessageBox.Show("Error loading alarms: " + serEx.Message);
            }
            catch (IOException ioEx)
            {
                MessageBox.Show("Error loading alarms: " + ioEx.Message);
            }
            finally
            {
                if (outFile != null)
                    outFile.Close();
            }
        }

        private void SaveStudents()
        {
            // Save students to xml file via serializer.

            // Orders students by lowest ID numberic value.
            // This makes ID's easily searchable in showStudentsComboBox.
            Students = Students.OrderBy(s => s.ID).ToList();
            FileStream outFile = null;
            try
            {
                outFile = new FileStream(
                    filePath, FileMode.Create, FileAccess.Write);

                XmlSerializer serializer = 
                    new XmlSerializer(typeof(List<Student>));
                serializer.Serialize(outFile, Students);
            }
            catch (SerializationException serEx)
            {
                MessageBox.Show("Error saving students: " + serEx.Message);
            }
            catch (IOException ioEx)
            {
                MessageBox.Show("Error saving students: " + ioEx.Message);
            }
            finally
            {
                if (outFile != null)
                    outFile.Close();
            }
        }

        private void ResetInfoFields()
        {
            // Sets all info fields to default values.
            idLabel2.Text = "";
            majorCombo.Text = "";
            gpaBox.Text = "";
            creditsNumeric.Text = "";
            financialBalanceLabel2.Text = "";
            startRegPicker.Value = DateTime.Now;
            endRegPicker.Value = DateTime.Now;
            fullNameBox.Clear();
            birthdatePicker.Value = DateTime.Now;
            phoneNumBox.Clear();
            eMailBox.Clear();
            studentTypeLabel.Text = "";
        }

        private void ResetIDDropdown()
        {
            // Adds all student IDs to ID selection (dropdown).

            // Numerically orders by ascending.
            Students = Students.OrderBy(s => s.ID).ToList();

            // Clears list to add onto new list.
            showStudentsComboBox.Items.Clear();
            foreach (Student s in Students)
                showStudentsComboBox.Items.Add(s.ID.ToString());
            // Clears ID entry box.
            showStudentsComboBox.Text = "";
        }

        private void UpdateFields(Student updateS)
        {
            // Fill fields from selected student's info.
            idLabel2.Text = updateS.ID.ToString();
            majorCombo.Text = updateS.Major.ToString();
            gpaBox.Text = updateS.GPA.ToString();
            creditsNumeric.Value = updateS.Credits;
            // Makes appear like US currency.
            financialBalanceLabel2.Text =  
                updateS.FinancialBalance.ToString("C");
            startRegPicker.CustomFormat = null;
            startRegPicker.Format = DateTimePickerFormat.Long;
            startRegPicker.Value = updateS.StartReg;
            endRegPicker.Value = updateS.EndReg;
            fullNameBox.Text = updateS.FullName;
            birthdatePicker.Value = updateS.Birthdate;
            phoneNumBox.Text = updateS.PhoneNum;
            eMailBox.Text = updateS.EMail;

            // Updates label depending on student type. 
            if (updateS.GetType() == typeof(InternationalStudent))
                studentTypeLabel.Text = "International";
            else if (updateS.GetType() == typeof(VeteranStudent))
                studentTypeLabel.Text = "Veteran";
            else
                studentTypeLabel.Text = "";
        }

        private void SetFieldsReadOnly()
        {
            // Sets all info fields to be non-editable
            majorCombo.Enabled = false;
            gpaBox.ReadOnly = true;
            calculateFB.Enabled = false;
            creditsNumeric.ReadOnly = true;
            creditsNumeric.Increment = 0;
            startRegPicker.Enabled = false;
            startRegCover.Visible = true;
            endRegPicker.Enabled = false;
            endRegCover.Visible = true;
            fullNameBox.ReadOnly = true;
            birthdatePicker.Enabled = false;
            birthdateCover.Visible = true;
            phoneNumBox.ReadOnly = true;
            eMailBox.ReadOnly = true;
        }

        private void SetFieldsWrite()
        {
            // Sets all info fields to be editable.
            majorCombo.Enabled = true;
            gpaBox.ReadOnly = false;
            calculateFB.Enabled = true;
            creditsNumeric.ReadOnly = false;
            creditsNumeric.Increment = 1;
            startRegPicker.Enabled = true;
            startRegCover.Visible = false;
            endRegPicker.Enabled = true;
            endRegCover.Visible = false;
            fullNameBox.ReadOnly = false;
            birthdatePicker.Enabled = true;
            birthdateCover.Visible = false;
            phoneNumBox.ReadOnly = false;
            eMailBox.ReadOnly = false;
        }

        private void ResetAddSection()
        {
            // Resets ID entry box.
            newIDNumeric.Text = "";
            // Selects default radio button.
            studentTypeRadioNeither.Checked = true;
        }

        private void showStudentsComboBox_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            // User can trigger the SelectedIndexChanged event with an
            // empty textbox by selecting the white space on the
            // combobox. This eliminates the problem.
            if (showStudentsComboBox.Text == "")
                return;

            // Get student by selected ID.
            Student student = Students.Find(
                s => s.ID.ToString() == showStudentsComboBox.Text);

            // Info of selected student shows up on info fields.
            UpdateFields(student);
            // Makes info fields editable.
            SetFieldsWrite();
        }

        private void StudentAccessForm_Load(object sender, EventArgs e)
        {
            // Sets all boxes to their initial states of default values
            // and readonly for the student info fields.
            SetFieldsReadOnly();
            ResetInfoFields();
            // Reads students from file and
            // adds their IDs to ID selection (dropdown).
            LoadStudents();
            ResetIDDropdown();
            ResetAddSection();

            // Sets format for currency.
            // Mainly used to output negative financial balances.
            culture = CultureInfo.CreateSpecificCulture("en-US");
            // Makes it show a negative number instead of showing
            // negative numbers an absolute value in parentheses.
            culture.NumberFormat.CurrencyNegativePattern = 1;
        }

        private void saveStudentButton_Click(object sender, EventArgs e)
        {
            // For when user saves student while no student
            // is selected. (There's no current student.)
            if (showStudentsComboBox.Text == "")
            {
                MessageBox.Show("Select student to save.");
                return;
            }

            // Get student by selected ID.
            Student saveS = Students.Find(
                s => s.ID.ToString() == showStudentsComboBox.Text);

            // If the user enters text in the showStudentsComboBox textbox, 
            // it's likely that string won't match a student's ID.
            if (saveS == null)
            {
                MessageBox.Show("Can't find student ID.");
                return;
            }

            // Make sure gpa can be saved as a number
            // before saving other info.
            try
            {
                saveS.GPA = double.Parse(gpaBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Enter number for GPA.");
                return;
            }

            // Gets info from fields to store in student object.

            // Parsing Major from dropdown-list (string -> Major).
            CollegeMajor newM;
            Enum.TryParse<CollegeMajor>(majorCombo.Text, out newM);
            saveS.Major = newM;
            saveS.Credits = (int)creditsNumeric.Value;
            saveS.FinancialBalance = // Substring removes $.
                double.Parse((financialBalanceLabel2.Text).Substring(1));
            saveS.StartReg = startRegPicker.Value;
            saveS.EndReg = endRegPicker.Value;
            saveS.FullName = fullNameBox.Text;
            saveS.Birthdate = birthdatePicker.Value;
            saveS.PhoneNum = phoneNumBox.Text;
            saveS.EMail = eMailBox.Text;
        }

        private void removeStudentButton_Click(object sender, EventArgs e)
        {
            // For when user removes student while no
            // is selected. (There's no current student.)
            if (showStudentsComboBox.Text == "")
            {
                MessageBox.Show("Select student to remove.");
                return;
            }

            // Get student by selected ID to remove.
            // Also removes student from ID dropdown.
            Student removeS = Students.Find(
                s => s.ID.ToString() == showStudentsComboBox.Text);
            Students.Remove(removeS);

            // Update students to select from,
            // and set fields to default values and readonly.
            ResetIDDropdown();
            ResetInfoFields();
            SetFieldsReadOnly();
        }

        private void addStudentButton_Click(object sender, EventArgs e)
        {
            // For when an ID isn't entered 
            // before add student button is pressed.
            if (newIDNumeric.Text == "" || newIDNumeric.Value == 0)
            {
                MessageBox.Show("Enter new student ID.");
                return;
            }

            // Gets radio button selected in student type radio grouping.
            var selectedRadio = 
                studentTypeContainer.Controls.OfType<RadioButton>()
                .Where(r => r.Checked == true).FirstOrDefault();

            // For when radio buttons were not selected
            // before add student button is pressed.
            if (selectedRadio == null)
            {
                MessageBox.Show("Select student type.");
                return;
            }

            // Makes sure ID isn't a duplicate.
            // First takes all students with same ID.
            // If none, may continue.
            List<Student> sWithSameID = students.Where(
                s => s.ID == int.Parse(newIDNumeric.Text)).ToList();
            if (sWithSameID.Count > 0) // Should only be 0 or 1.
            {
                MessageBox.Show("ID taken.");
                return;
            }

            // Variant of student object depends on radio selected.
            // (base or derived, international or veteran)
            Student newS;
            if (selectedRadio == studentTypeRadioInternational)
                 newS = new InternationalStudent();
            else if (selectedRadio == studentTypeRadioVeteran)
                newS = new VeteranStudent();
            else
                newS = new Student();

            // Stores given ID to new student.
            newS.ID = (int)newIDNumeric.Value;

            // Makes fields editable.
            SetFieldsWrite();
            // Adds student to student list. (Will later be recorded in file.)
            Students.Add(newS);
            // Clears ID selection (since is now not up to date).
            ResetIDDropdown();
            ResetAddSection();
            // Updates ID selection.
            UpdateFields(newS);
            showStudentsComboBox.Text = newS.ID.ToString();
        }

        private void StudentAccessForm_FormClosing(
            object sender, FormClosingEventArgs e)
        {
            // Saves all data before app closes.
            SaveStudents();
        }

        private void calculateFB_Click(object sender, EventArgs e)
        {
            // Get student by selected ID.
            Student calS = Students.Find(
                s => s.ID.ToString() == showStudentsComboBox.Text);

            // Cancels calculation if no value was entered.
            if (creditsNumeric.Text == "")
            {
                MessageBox.Show("Enter credits amount.");
                return;
            }

            // Store entered credits.
            calS.Credits = (int)creditsNumeric.Value;
            // Calculate and store balance.
            calS.CalculateFinancialBalance();
            // Show balance in form.
            // String formatting avoids absolute number.
            financialBalanceLabel2.Text = 
                String.Format(culture, "{0:C}", calS.FinancialBalance);
        }
    }
}
