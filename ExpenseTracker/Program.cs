// =====================================================================
//  Program.cs  —  the interactive console UI for MyBudget (Assignment 1).
//  Target framework: .NET 10 (LTS), language C# 14.
//
//  >>> BUILD THE MENU-DRIVEN UI HERE (Modules 1-3). <<<
//
//  Once you have implemented BudgetRules.cs (so the unit tests pass), wire it
//  up to a console interface that meets the assignment brief:
//
//    * Print a banner (try a raw string literal).
//    * Loop a menu until the user exits, using a switch on the choice:
//        1) Add an expense   2) View summary   3) Set monthly budget   4) Exit
//    * Read and VALIDATE input, re-prompting on bad data (decimal.TryParse,
//      BudgetRules.NormalizeCategory, a date parse, non-empty text).
//    * Keep running totals in simple variables (no collections / no classes).
//    * Use BudgetRules.ValidateAmount / ClassifyAmount / BudgetStatus /
//      FormatCurrency for all logic and formatting.
//    * Handle bad input with try / catch / finally and InvalidExpenseException.
//
//  See section 6 of the assignment brief for a sample run to aim for.
// =====================================================================
using ExpenseTracker;


//declare variables

decimal monthlyBudget = 0.0m;
bool isBudgetSet = false; //users need to enter budget before they start recording expenses.

decimal totalExpense = 0.0m;

//Category - running totals for each category
decimal foodExpense = 0.0m;
decimal transportExpense = 0.0m;
decimal utilitiesExpense = 0.0m;
decimal entertainmentExpense = 0.0m;
decimal otherExpense = 0.0m;

int selectedMenu;
bool continueWorking = true; //the status of the main while loop - choosing opiton 4 will set continueWorking to false and exits the loop
string status; //status of Budget

string  expenseDate;  //because we'll be converting the DateTime to string (striping the time component)

string? noteInput;

//--------------Main Menu (loop)-----------------------

while (continueWorking)
{
    Console.WriteLine();
    Console.WriteLine("============Expense Tracker ==============");
    Console.WriteLine();

    Console.WriteLine("1) Add Expense  2) View Summary     3) Set Monthly Budget   4) Exit");

    string? option = Console.ReadLine();

    if (!int.TryParse(option, out selectedMenu)) { Console.WriteLine("Please Select the Correct Number from the Menu (1,2,3 or 4)"); }

    switch (selectedMenu)
    {

        case 4:
            //Leave the loop cleanly and print a closing message
            Console.WriteLine("Thank you for using the Expense Tracker. Goodbye");
            continueWorking = false;
            break;

        case 1:

            Console.WriteLine(" -----------Adding Expensens---------\n");

            

            //Accept description from user. Description cannot be empty!
            string description = "";
            while(true)
            {
                Console.Write("Enter Description:       ");
                description = Console.ReadLine();
                if (description == null || description == "")
                {
                    Console.WriteLine("Description cannot be empty. Please enter a valid description.\n ");
                }
                else
                {
                    break;
                }
            }

            //Accept Expense amount from the user and validate it using the ValidateAmount method of the BudgetRules class
            decimal validatedAmount = 0.0m;
            while (true)
            {
                Console.Write("Amount:      ");
                string? userInput = Console.ReadLine();

                if (decimal.TryParse(userInput, out validatedAmount))
                {
                    try
                    {
                        validatedAmount = BudgetRules.ValidateAmount(validatedAmount);
                        break;
                    }
                    catch (InvalidExpenseException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Amount must be a valid positive number. Please enter amount again.");
                }
            }

            //Map categories into Normalized category using the NormalizeCategory method of the BudgetRules class

            string normalizedCategory = "";
            string inputCategory = "";
            while (true)
            {
                Console.Write("Enter Category: [Food/Transport/Utilities/Entertainment/Other]:      ");
                inputCategory = Console.ReadLine();

                normalizedCategory = BudgetRules.NormalizeCategory(inputCategory);

                if (normalizedCategory != null)
                {
                    inputCategory = normalizedCategory;
                    //Console.WriteLine($"Added: {inputCategory}");  //added just for test
                    break;

                }
                else
                {
                    Console.WriteLine("Please Enter a correct category.");
                }
            }

            //Accept date from user. If data is blank, default to Datetime.today()
            while (true)
            {
                Console.Write("Enter date (YYYY-MM-DD) or press Enter for today: ");
                
                string inputDate = Console.ReadLine();
                DateTime parsedDate;

                if (inputDate == "")
                {
                    expenseDate = DateTime.Today.ToString("yyyy-MM-dd");
                    //Console.WriteLine($"Your Date is {expenseDate}");  //For testing only
                    break;

                }
                else if (DateTime.TryParse(inputDate, out parsedDate))
                {
                    expenseDate = parsedDate.ToString("yyyy-MM-dd");
                    //Console.WriteLine($"Your Date is {expenseDate}");   //For testing only
                    break;
                }
                else
                {
                    Console.WriteLine("Please Enter a Valid Date. Date format should be YYYY-MM-DD ");
                }

            }


            // Optional Note 
            Console.Write("Note (optional): ");
            noteInput = Console.ReadLine();


            //Calculate running total of categories and total expense

            totalExpense += validatedAmount;

            if (normalizedCategory == "Food") {foodExpense += validatedAmount;}
            else if (normalizedCategory == "Transport") {transportExpense +=validatedAmount;}
            else if (normalizedCategory == "Utilities") {utilitiesExpense +=validatedAmount; }
            else if (normalizedCategory == "Entertainment") {entertainmentExpense += validatedAmount;}
            else if (normalizedCategory == "Other") {otherExpense += validatedAmount;}

            //Inform the user that the expenses is recorded
            string formattedAmount = BudgetRules.FormatCurrency(validatedAmount); 
            Console.WriteLine($"Recorded: {description} | {formattedAmount} | {normalizedCategory} | {expenseDate:yyyy-MM-dd}");

            // If a budget is configured, show live remaining summary
            if (isBudgetSet)
            {
                decimal remaining = monthlyBudget - totalExpense;
                status = BudgetRules.BudgetStatus(remaining, monthlyBudget);
                Console.WriteLine($"  Budget: {BudgetRules.FormatCurrency(remaining)} remaining of {BudgetRules.FormatCurrency(monthlyBudget)} -> {status}");
            }

            break;

        case 2:
            Console.WriteLine("====================================================");
            Console.WriteLine("--- Expense Summary ---");
            if (isBudgetSet)
            {
                Console.WriteLine($"  Monthly Budget: {monthlyBudget}");
            }
            Console.WriteLine($"  Total Expenses: {totalExpense}");
            
            Console.WriteLine();
            Console.WriteLine("--- Spending by Category ---");
            Console.WriteLine($"  Food          : {BudgetRules.FormatCurrency(foodExpense)}");
            Console.WriteLine($"  Transport     : {BudgetRules.FormatCurrency(transportExpense)}");
            Console.WriteLine($"  Utilities     : {BudgetRules.FormatCurrency(utilitiesExpense)}");
            Console.WriteLine($"  Entertainment : {BudgetRules.FormatCurrency(entertainmentExpense)}");
            Console.WriteLine($"  Other         : {BudgetRules.FormatCurrency(otherExpense)}");
            break;

        case 3:
            while (true)
            {
                Console.Write("Monthly budget: ");
                string? budgetInput = Console.ReadLine();
                decimal parsedBudget = 0.0m;

                if (decimal.TryParse(budgetInput, out parsedBudget))
                    {
                        try
                        {
                            parsedBudget = BudgetRules.ValidateAmount(parsedBudget);
                        }
                        catch (InvalidExpenseException ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                            continue;   //jump to the while loop under case 3 
                        }

                    monthlyBudget = parsedBudget;
                    isBudgetSet = true;  //When expense is added - we will check if budget is set before we do any calculation against budget
                    Console.WriteLine($"Budget set to {BudgetRules.FormatCurrency(monthlyBudget)}.");
                    
                    decimal remaining = monthlyBudget - totalExpense;
                    status = BudgetRules.BudgetStatus(remaining, monthlyBudget);  //from the BudgetRules class
                    
                    //print something like this ---- Budget: $500.00 remaining of $500.00 -> On track
                    Console.WriteLine($"  Budget: {BudgetRules.FormatCurrency(remaining)} remaining of {BudgetRules.FormatCurrency(monthlyBudget)} -> {status}");
                    break;
                }
                else
                {
                    Console.WriteLine("   Error: Budget must be a valid positive number.");
                }
                
            }
            break;
        default:
            Console.WriteLine();
            Console.WriteLine("Please make the correct choice");
            break;

    };
}

