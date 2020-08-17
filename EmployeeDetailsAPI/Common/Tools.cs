using EmployeeDetailsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EmployeeDetailsAPI.Common
{
    public class Tools
    {
        public Employee GetEmployeeDetails(string EmpId)
        {
            Employee Emp = null;
            try
            {
                List<Employee> EmployeeDetails = GetEmployeeDetailsFromCsv();
                if (EmployeeDetails != null && EmployeeDetails.Count > 0)
                {
                    Emp = EmployeeDetails.Where(e => e.EmployeeId.Equals(EmpId)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Emp;
        }

        public List<Employee> GetFilteredEmployeeDetails(string FirstName, string LastName, string Country, string Branch)
        {
            List<Employee> EmpList = null;
            try
            {
                EmpList = GetEmployeeDetailsFromCsv();

                if (!String.IsNullOrEmpty(FirstName))
                {
                    if (FirstName.Last().ToString().Equals("*"))
                    {
                        EmpList = EmpList.Where(e => !String.IsNullOrEmpty(e.FullName) && e.FullName.Trim().Split(' ')[0].ToUpper().StartsWith(FirstName.ToUpper().Replace("*", String.Empty))).ToList();
                    }
                    else
                    {
                        EmpList = EmpList.Where(e => !String.IsNullOrEmpty(e.FullName) && e.FullName.Trim().Split(' ')[0].ToUpper().Equals(FirstName.ToUpper())).ToList();
                    }
                }
                if (!String.IsNullOrEmpty(LastName))
                {
                    if (LastName.Last().ToString().Equals("*"))
                    {
                        EmpList = EmpList.Where(e => !String.IsNullOrEmpty(e.FullName) && e.FullName.Trim().Split(' ')[e.FullName.Trim().Split(' ').Length - 1].ToUpper().StartsWith(LastName.Replace("*", String.Empty).ToUpper())).ToList();
                    }
                    else
                    {
                        EmpList = EmpList.Where(e => !String.IsNullOrEmpty(e.FullName) && e.FullName.Trim().Split(' ')[e.FullName.Trim().Split(' ').Length - 1].ToUpper().Equals(LastName.ToUpper())).ToList();
                    }
                }
                if (!String.IsNullOrEmpty(Country))
                {
                    EmpList = EmpList.Where(e => !String.IsNullOrEmpty(e.Country) && e.Country.Trim().ToUpper().Equals(Country.ToUpper())).ToList();
                }
                if (!String.IsNullOrEmpty(Branch))
                {
                    EmpList = EmpList.Where(e => !String.IsNullOrEmpty(e.Branch) && e.Branch.Trim().ToUpper().Equals(Branch.ToUpper())).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EmpList;
        }

        private List<Employee> GetEmployeeDetailsFromCsv()
        {
            List<Employee> EmpList = new List<Employee>();
            try
            {
                string FilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"bin", @"Csv Files\").Replace(@"file:\", String.Empty) + "employee.csv";
                string[] lines = System.IO.File.ReadAllLines(FilePath);
                for (int i = 1; i <= lines.Length - 1; i++)
                {
                    if (!String.IsNullOrEmpty(lines[i]))
                    {
                        var values = lines[i].Split(',');
                        Employee Emp = new Employee();
                        DateTime DateOfBirth;
                        DateTime JoinedDate;
                        double Salary;
                        Emp.EmployeeId = values[0];
                        Emp.FullName = values[1];
                        Emp.Gender = values[2];
                        Emp.DateOfBirth = values[3] != null && DateTime.TryParse(values[3], out DateOfBirth) ? DateOfBirth : DateTime.MinValue;
                        Emp.JoinedDate = values[4] != null && DateTime.TryParse(values[4], out JoinedDate) ? JoinedDate : DateTime.MinValue;
                        Salary = values[5] != null && double.TryParse(values[5], out Salary) ? Salary : 0.0;
                        Emp.Branch = values[6];
                        Emp.Country = values[7];
                        Emp.Salary = ConvertCurrency(Emp.Country, Salary);
                        Emp.PATETax = CalculatePATETax(Emp.Country, Emp.Salary);
                        Emp.NetPayAmount = Emp.Salary - Emp.PATETax;
                        EmpList.Add(Emp);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return EmpList;
        }

        private List<Currency> GetCurruncyDetailsFromCsv()
        {
            List<Currency> CurrencyList = new List<Currency>();
            string FilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"bin", @"Csv Files\").Replace(@"file:\", String.Empty) + "currency.csv";
            try
            {
                string[] lines = System.IO.File.ReadAllLines(FilePath);
                for (int i = 1; i <= lines.Length - 1; i++)
                {
                    if (!String.IsNullOrEmpty(lines[i]))
                    {
                        var values = lines[i].Split(',');
                        Currency Crr = new Currency();
                        double Rate;
                        Crr.Country = values[0];
                        Crr.Type = values[1];
                        Crr.Rate = values[2] != null && double.TryParse(values[2], out Rate) ? Rate : 0.0;
                        CurrencyList.Add(Crr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return CurrencyList;
        }

        private double ConvertCurrency(string Country, double Amount)
        {
            double ConvertedAmount = 0.0;
            double CurrRate = 0.0;
            try
            {
                Currency Curr = GetCurruncyDetailsFromCsv().Where(c => c.Country.Equals(Country.Trim())).FirstOrDefault();
                if (Curr != null)
                {
                    CurrRate = Curr.Rate;
                }
                if (CurrRate > 0.0)
                {
                    ConvertedAmount = Amount * CurrRate;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ConvertedAmount;
        }

        private double CalculatePATETax(string Country, double Salary)
        {
            double PATETax = 0.0;
            if (Country.Trim().Equals("Sri Lanka"))
            {
                if (Salary >= 100000 && Salary < 250000)
                {
                    PATETax = Salary * 0.05;
                }
                else if (Salary >= 250000)
                {
                    PATETax = Salary * 0.1;
                }
            }
            else if (Country.Equals("India"))
            {
                if (Salary >= 100000 && Salary < 300000)
                {
                    PATETax = Salary * 0.04;
                }
                else if (Salary >= 300000)
                {
                    PATETax = Salary * 0.07;
                }
            }
            if (Country.Equals("Pakistan"))
            {
                if (Salary >= 500000)
                {
                    PATETax = Salary * 0.005;
                }
                else if (Salary > 500000)
                {
                    PATETax = Salary * 0.4;
                }
            }
            return PATETax;
        }
    }
}