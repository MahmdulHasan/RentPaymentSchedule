using System;
using System.Collections.Generic;

namespace RentPaymentCalculation
{
    public partial class Program
    {

        public bool FIsLeapYear(int Year)
        {

            bool FIsLeapYear;

            if (Year % 4 > 0)
                FIsLeapYear = false;
            else if (Year % 100 > 0)
                FIsLeapYear = true;
            else if (Year % 400 == 0)
                FIsLeapYear = true;
            else
                FIsLeapYear = false;

            return FIsLeapYear;
        }

        public bool FIsEndOfMonth(int Day, int Month, int Year)
        {

            int month = Month;
            bool FIsEndOfMonth = false;
            switch (month)
            {
                case 1:
                    FIsEndOfMonth = Day == 31;
                    break;

                case 3:
                    FIsEndOfMonth = Day == 31;
                    break;

                case 5:
                    FIsEndOfMonth = Day == 31;
                    break;

                case 7:
                    FIsEndOfMonth = Day == 31;
                    break;

                case 8:
                    FIsEndOfMonth = Day == 31;
                    break;

                case 10:
                    FIsEndOfMonth = Day == 31;
                    break;

                case 12:
                    FIsEndOfMonth = Day == 31;
                    break;


                case 4:
                    FIsEndOfMonth = Day == 30;
                    break;

                case 6:
                    FIsEndOfMonth = Day == 30;
                    break;

                case 9:
                    FIsEndOfMonth = Day == 30;
                    break;

                case 11:
                    FIsEndOfMonth = Day == 30;
                    break;

                case 2:
                    if (FIsLeapYear(Year))
                        FIsEndOfMonth = Day == 29;
                    else
                        FIsEndOfMonth = Day == 28;

                    break;

            }

            return FIsEndOfMonth;

        }
        public int Days360(int StartYear, int EndYear, int StartMonth, int EndMonth, int StartDay,
                int EndDay)
        {

            int Days360 = ((EndYear - StartYear) * 360) + ((EndMonth - StartMonth) * 30) + (EndDay - StartDay);

            return Days360;
        }


        public int TmpDays360Nasd(DateTime StartDate, DateTime EndDate, int Method, bool UseEom)
        {
            int StartDay;
            int StartMonth;
            int StartYear;
            int EndDay;
            int EndMonth;
            int EndYear;


            StartDay = StartDate.Day;
            StartMonth = StartDate.Month;
            StartYear = StartDate.Year;
            EndDay = EndDate.Day;
            EndMonth = EndDate.Month;
            EndYear = EndDate.Year;


            if ((EndMonth == 2 && FIsEndOfMonth(EndDay, EndMonth, EndYear)) &&
                  ((StartMonth == 2 && FIsEndOfMonth(StartDay, StartMonth, StartYear)) || Method == 3))
            {
                EndDay = 30;
            }
            if (EndDay == 31 && (StartDay >= 30 || Method == 3))
            {
                EndDay = 30;
            }

            if (StartDay == 31)
            {
                StartDay = 30;
            }

            if (UseEom && StartMonth == 2 && FIsEndOfMonth(StartDay, StartMonth, StartYear))
            {
                StartDay = 30;
            }


            int TmpDays360Nasd = Days360(StartYear, EndYear, StartMonth, EndMonth, StartDay, EndDay);


            return TmpDays360Nasd;
        }

        public double DateDiff(string d, DateTime StartDate, DateTime EndDate)
        {
            TimeSpan timeSpan = EndDate - StartDate;

            return timeSpan.TotalDays;
        }
        public double TmpDiffDates(DateTime StartDate, DateTime EndDate, int Basis)
        {

            int basis = Basis;
            double TmpDiffDates = 0;

            switch (basis)
            {

                case 0:
                    TmpDiffDates = TmpDays360Nasd(StartDate, EndDate, 0, true);
                    break;

                case 1:
                    TmpDiffDates = DateDiff("d", StartDate, EndDate);
                    break;

                case 2:
                    TmpDiffDates = DateDiff("d", StartDate, EndDate);
                    break;

                case 3:
                    TmpDiffDates = DateDiff("d", StartDate, EndDate);
                    break;

                    //case 4:
                    //    TmpDiffDates = TmpDays360Euro(StartDate, EndDate);
                    //    break;
            }

            return TmpDiffDates;

        }

        public double TmpCalcAnnualBasis(DateTime StartDate, DateTime EndDate, int Basis)
        {

            int StartDay;
            int StartMonth;
            int StartYear;
            int EndDay;
            int EndMonth;
            int EndYear;
            int iYear;

            int basis = Basis;

            int TmpCalcAnnualBasis = 0;

            switch (basis)
            {

                case 0:
                    TmpCalcAnnualBasis = 360;
                    break;
                case 2:
                    TmpCalcAnnualBasis = 360;
                    break;
                case 4:
                    TmpCalcAnnualBasis = 360;
                    break;


                case 3:
                    TmpCalcAnnualBasis = 365;
                    break;
                case 1:
                    StartDay = StartDate.Day;
                    StartMonth = StartDate.Month;
                    StartYear = StartDate.Year;
                    EndDay = EndDate.Day;
                    EndMonth = EndDate.Month;
                    EndYear = EndDate.Year;


                    if (StartYear == EndYear)
                        if (FIsLeapYear(StartYear))
                            TmpCalcAnnualBasis = 366;
                        else
                            TmpCalcAnnualBasis = 365;

                    else if (((EndYear - 1) == StartYear) && ((StartMonth > EndMonth) || ((StartMonth == EndMonth) && StartDay >= EndDay)))
                        if (FIsLeapYear(StartYear))
                            if (StartMonth < 2 || (StartMonth == 2 && StartDay <= 29))
                                TmpCalcAnnualBasis = 366;
                            else
                                TmpCalcAnnualBasis = 365;

                        else if (FIsLeapYear(EndYear))
                            if (EndMonth > 2 || (EndMonth == 2 && EndDay == 29))
                                TmpCalcAnnualBasis = 366;
                            else
                                TmpCalcAnnualBasis = 365;

                        else
                            TmpCalcAnnualBasis = 365;

                    else
                        for (iYear = StartYear; iYear <= EndYear; iYear++)
                            if (FIsLeapYear(iYear))
                                TmpCalcAnnualBasis = TmpCalcAnnualBasis + 366;
                            else
                                TmpCalcAnnualBasis = TmpCalcAnnualBasis + 365;


                    TmpCalcAnnualBasis = TmpCalcAnnualBasis / (EndYear - StartYear + 1);




                    break;
            }
            return TmpCalcAnnualBasis;

        }

        public double TmpYearFrac(DateTime StartDate, DateTime EndDate, int Basis)
        {
            double nNumerator;
            double nDenom;


            nNumerator = TmpDiffDates(StartDate, EndDate, Basis);
            nDenom = TmpCalcAnnualBasis(StartDate, EndDate, Basis);

            double TmpYearFrac = nNumerator / nDenom;

            return TmpYearFrac;
        }

        public  double YearFrac(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentOutOfRangeException("endDate cannot be less than startDate");
            }

            int endDay = endDate.Day;
            int startDay = startDate.Day;

            switch (startDay)
            {
                case 31:
                    {
                        startDay = 30;
                        if (endDay == 31)
                        {
                            endDay = 30;
                        }
                    }
                    break;

                case 30:
                    {
                        if (endDay == 31)
                        {
                            endDay = 30;
                        }
                    }
                    break;

                case 29:
                    {
                        if (startDate.Month == 2)
                        {
                            startDay = 30;
                            if ((endDate.Month == 2) && (endDate.Day == 28 + (DateTime.IsLeapYear(endDate.Year) ? 1 : 0)))
                            {
                                endDay = 30;
                            }
                        }
                    }
                    break;

                case 28:
                    {
                        if ((startDate.Month == 2) && (!DateTime.IsLeapYear(startDate.Year)))
                        {
                            startDay = 30;
                            if ((endDate.Month == 2) && (endDate.Day == 28 + (DateTime.IsLeapYear(endDate.Year) ? 1 : 0)))
                            {
                                endDay = 30;
                            }
                        }
                    }
                    break;
            }

            return ((endDate.Year - startDate.Year) * 360 + (endDate.Month - startDate.Month) * 30 + (endDay - startDay)) / 360.0;
        }
        static void Main(string[] args)
        {

            var obj = new Program();
            DateTime startDate = Convert.ToDateTime(" 09 / 01 / 2019");

            DateTime endDate;

            IList<double> rents = new List<double>();

            var yearFrac = obj.TmpYearFrac(startDate, startDate.AddMonths(35), 1)/3;

            var power = 0;
            decimal div = 0.0M;

            double div2 = 0.0;

            for (int i = 0; i < 120; i++)
            {
                endDate = startDate.AddMonths(i);

                decimal YearFrac = Convert.ToDecimal((Convert.ToDecimal(((TimeSpan)
                    (Convert.ToDateTime(endDate).Subtract(startDate))).Days)) / Convert.ToDecimal(365));

                div = YearFrac / 3;

                div2 = obj.TmpYearFrac(startDate, endDate, 1) / 3;

                power = (int) Math.Floor(div2);
               
                       

                var amount = 67000.00 * Math.Pow((1 + 0.1),power);

                rents.Add(amount);
            }

            Console.WriteLine("Hello World!");
        }
    }
}
