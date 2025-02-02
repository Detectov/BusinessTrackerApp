﻿using BusinessTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BusinessTracker.Controllers
{
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            //Last 7 Days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Food)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            //Total Income
            int TotalIncome = SelectedTransactions
                .Where(i => i.Food.Type == "Ingreso")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = String.Format(culture, "{0:C0}", TotalIncome);

            //Total Expense
            int TotalExpense = SelectedTransactions
                .Where(i => i.Food.Type == "Gasto")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = String.Format(culture, "{0:C0}", TotalExpense);

            // put $ in front of the number
            // string TotalIncome = TotalIncome.ToString("C0", CultureInfo.CurrentCulture);


            //Balance
            int Balance = TotalIncome - TotalExpense;
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);



            //Doughnut Chart - Transaccion por Platillo
            ViewBag.DoughnutChartData = SelectedTransactions
                .Where(i => i.Food.Type == "Gasto")
                .GroupBy(j => j.Food.FoodId)
                .Select(k => new
                {
                    foodTitleWithIcon = k.First().Food.Icon + " " + k.First().Food.Title,
                    amount = k.Sum(j => j.Amount),
                    // formattedAmount2 = String.Format(culture, "{0:C0}", k.Sum(j => j.Amount))
                })
                .OrderByDescending(l => l.amount)
                .ToList();

            //Spline Chart - Income vs Expense

            //Income
            List<SplineChartData> IncomeSummary = SelectedTransactions
                .Where(i => i.Food.Type == "Ingreso")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)

                })
                .ToList();

            //Expense
            List<SplineChartData> ExpenseSummary = SelectedTransactions
                .Where(i => i.Food.Type == "Gasto")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            //Combine Income & Expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in Last7Days
                                      join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };
            //Recent Transactions
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Food)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();


            return View();
        }
    }

    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;

    }
}
