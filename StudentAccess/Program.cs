/*
 * Name: Demetrios Vozella
 * Final Project
 * Course: CSI255 (Fall 2021)
 * Date: 12/17/2021
 * Description: GUI for handling student records. Students may be an 
 *      International student, a Veteran, or neither. International and 
 *      Veteran students are represented by classes derived from the 
 *      Student class. Derived classes change how the financial balance 
 *      of the student is calculated. For all students, it's calculated 
 *      from the amount of credits. International students have 
 *      higher costs. Veterans have additional funding that result in 
 *      lower costs. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace StudentAccess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StudentAccessForm());
        }
    }
}
