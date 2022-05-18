/*
 * Name: Demetrios Vozella
 * Final Project
 * Course: CSI255 (Fall 2021)
 * Date: 12/17/2021
 * Description: Derived class of Student for a veteran student.
 *      Financial balance calculation includes additional funding. 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Students
{
    public class VeteranStudent : Student
    {
        // Default constructor
        public VeteranStudent()
            : base()
        { }

        // METHODS

        public override void CalculateFinancialBalance()
        {
            base.CalculateFinancialBalance();

            // Veterans get additional funding.
            double newFB = FinancialBalance * 0.85 - 10043.01;
            FinancialBalance = Math.Round(newFB, 2);
        }
    }
}
