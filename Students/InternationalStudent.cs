/*
 * Name: Demetrios Vozella
 * Final Project
 * Course: CSI255 (Fall 2021)
 * Date: 12/17/2021
 * Description: Derived class of Student for an International student.
 *      Financial balance calculation includes higher costs. 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Students
{
    public class InternationalStudent : Student
    {
        // Default constructor
        public InternationalStudent()
            : base()
        { }

        // METHODS

        public override void CalculateFinancialBalance()
        {
            base.CalculateFinancialBalance();

            // International student gets increase costs.
            double newFB = FinancialBalance * 1.2 + 34247.74;
            FinancialBalance = Math.Round(newFB, 2);
        }
    }
}
