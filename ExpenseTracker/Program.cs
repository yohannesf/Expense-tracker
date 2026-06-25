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
            Console.WriteLine("Thank you for using the Expense Tracker. Goodbye");
            continueWorking = false;
            break;

        case 1:
            break;
        case 2:
            break;
        case 3:
            while (true)
            {
                Console.Write("Monthly budget: ");
                string? budgetInput = Console.ReadLine();

                if (decimal.TryParse(budgetInput, out decimal parsedBudget))
                    {
                        try
                        {
                            parsedBudget = BudgetRules.ValidateAmount(parsedBudget);
                        }
                        catch (InvalidExpenseException ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                            continue;
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

