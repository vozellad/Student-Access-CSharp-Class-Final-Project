/*
 * Name: Demetrios Vozella
 * Final Project
 * Course: CSI255 (Fall 2021)
 * Date: 12/17/2021
 * Description: Base Student class that holds academic and personal
 *      information for a student. Also can calculate financial balance
 *      based on number of credits. 
 */

using System;
using System.Xml.Serialization;

// Simplified for use in CSI 255.
namespace Students
{
    public enum CollegeMajor
    {
        Accounting, ComputerScience, FineArts, GeneralStudies, History,
        Mathematics, Humanities, NaturalScience, Nursing, Psychology
    }

    // Allows Student derived classes to be serializable.
    [XmlInclude(typeof(InternationalStudent))]
    [XmlInclude(typeof(VeteranStudent))]

    [Serializable]
    public class Student
    {
        // INSTANCE VARIABLES

        // Academic info.
        private int id;
        private CollegeMajor major;
        private double gpa;
        private int credits;
        private double financialBalance;
        // Date range registered.
        private DateTime startReg;
        private DateTime endReg;

        // Personal info.
        private string fullName;
        private DateTime birthdate;
        private string phoneNum;
        private string eMail;

        // Constructor
        public Student(int newID, string newMajor, double newGPA, 
            int newCredits, double newFB,
            DateTime newStartReg, DateTime newEndReg, string newFullName, 
            DateTime newBirthdate, string newPhoneNum, string newEMail)
        {
            ID = newID;

            CollegeMajor newM;
            if (Enum.TryParse<CollegeMajor>(newMajor, out newM))
                Major = newM;
            else // Default case
                Major = CollegeMajor.GeneralStudies;
            
            GPA = newGPA;
            Credits = newCredits;
            FinancialBalance = newFB;
            StartReg = newStartReg;
            EndReg = newEndReg;
            FullName = newFullName;
            Birthdate = newBirthdate;
            PhoneNum = newPhoneNum;
            EMail = newEMail;
        }

        // Default constructor
        public Student() 
            : this(0, "GeneralStudies", 0, 0, 0, DateTime.Now,
                  DateTime.Now, "", DateTime.Now, "", "") 
        { }

        // PROPERTIES

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public CollegeMajor Major
        {
            get { return major; }
            set { major = value; }
        }

        public double GPA
        {
            get { return gpa; }
            set { gpa = value; }
        }

        public int Credits
        {
            get { return credits; }
            set { credits = value; }
        }

        public double FinancialBalance
        {
            get { return financialBalance; }
            set { financialBalance = value; }
        }

        public DateTime StartReg
        {
            get { return startReg; }
            set { startReg = value; }
        }

        public DateTime EndReg
        {
            get { return endReg; }
            set { endReg = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public DateTime Birthdate
        {
            get { return birthdate; }
            set { birthdate = value; }
        }

        public string PhoneNum
        {
            get { return phoneNum; }
            set { phoneNum = value; }
        }

        public string EMail
        {
            get { return eMail; }
            set { eMail = value; }
        }

        // METHODS

        public virtual void CalculateFinancialBalance()
        {
            const double costPerCredit = 312.36;
            const double tuitionAndFees = 10338.75;

            double newFB = (double)Credits * costPerCredit + tuitionAndFees;
            FinancialBalance = Math.Round(newFB, 2);
        }
    }
}
