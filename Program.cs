using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h2_referencenumber_fi
{
    class Program
    {
        static void Main(string[] args)
        {
            int menuChoice = 0;

            do
            {
                menuChoice = openMenu();


                if (menuChoice == 1)
                {
                    ValidityCheck();
                }
                if (menuChoice == 2)
                {
                    CreateNewRef();
                }
                if (menuChoice == 3)
                {
                    CreateMultiNewRef();
                }
            } while (menuChoice != 4); // menu 4 for exit
            Console.ReadKey();
        }

        private static int openMenu()
        {
            Console.WriteLine("\nFinnish Reference number creator.\nMenu: press number for operation:\n");
            Console.WriteLine("1. Check validity of finnish reference number.");
            Console.WriteLine("2. Create new finnish reference number.");
            Console.WriteLine("3. Create multiple finnish reference numbers.");
            Console.WriteLine("4. Exit");
            var result = Console.ReadLine();
            return Convert.ToInt32(result); // don't select empty line
        }

        private static void ValidityCheck()
        {
            Console.WriteLine("Please input reference number for validation (4-20 digits): ");
            string referenceNumber = NumberInput(4, 20);
            int checksum10 = calculateChecksum(referenceNumber,1); // call method for calculating checksum
            referenceNumber = FormatReference(referenceNumber, checksum10, 1); // if 2 then add, if 1 then no adding
        }

        private static string FormatReference(string referenceNumber, int checksum4, int v)
        {
            if (v == 2)
            {
                referenceNumber = referenceNumber + checksum4;
            }
            // Console.WriteLine(referenceNumber);

            var referenceInt = referenceNumber.Select(ch => ch - '0').ToArray();
            if (referenceInt[referenceInt.Length - 1] == checksum4) // compare last number
            {
                // for (int j = 0; j < (referenceInt.Length - 1); j++)
                for (int j = (referenceInt.Length); j > 0 ; j--)
                {
                    j -= 5;
                    if(j < 1) goto o2;
                    referenceNumber = referenceNumber.Insert(j, " ");
                }
                o2:
                Console.WriteLine("{0} OK", referenceNumber);
            }
            else
                Console.WriteLine("Referencenumber Incorrect. {0} != {1}", (referenceInt[referenceInt.Length - 1]), checksum4);
            
            return referenceNumber;
        }

        private static int calculateChecksum(string referenceNumber, int v)
        {
            int calculationSum = 0;
            int[] weightedMultipler = new int[] { 7, 3, 1, 7, 3, 1, 7, 3, 1, 7, 3, 1, 7, 3, 1, 7, 3, 1, 7 }; // new int[19] 

            if (v == 1)
            {
                referenceNumber = referenceNumber.Remove(referenceNumber.Length - 1);
                    // First remove checksum from the end
            }
            var referenceInt = referenceNumber.Select(ch => ch - '0').ToArray();
            Array.Reverse(referenceInt); // reverse array for right to left calcs

            for (int i = 0; i < referenceInt.Length; i++) // calc 
            {
                calculationSum += (referenceInt[i] * weightedMultipler[i]); // from right to left multiply as the array is reversed and shortened
            }

            int sumCeil = (int)(Math.Ceiling(calculationSum / 10.0d) * 10);
            int checksum1 = sumCeil - calculationSum;

            return checksum1;
        }

        private static void CreateNewRef()
        {
            Console.WriteLine("Please input number for creating reference number (3-19 digits): ");
            string referenceNumber = NumberInput(3, 19);
            int checksum6 = calculateChecksum(referenceNumber, 2);
            referenceNumber = FormatReference(referenceNumber, checksum6, 2); // if 2 then add, if 1 then no adding
            //Console.WriteLine(referenceNumber);
        }

        private static string NumberInput(int v1, int v2)
        {
            string referenceNumber;
        b2:
            {
                referenceNumber = Console.ReadLine();
                referenceNumber = referenceNumber.Replace(" ", "");

                if (referenceNumber.Length < v1 | referenceNumber.Length > v2 | !referenceNumber.All(Char.IsNumber))
                {
                    Console.WriteLine("Please input correct number: ");
                    goto b2;
                }
                else goto o1;
            }
        o1: // return referenceNumber
            return referenceNumber;
        }

        private static void CreateMultiNewRef()
        {
            
            Console.WriteLine("Please input amount of reference numbers you want to create:");
            string amountNumber = Console.ReadLine();
            if (amountNumber.Length < 1 | amountNumber.Length > 19 | !amountNumber.All(Char.IsNumber)) // is the logic correct?
            {
                Console.WriteLine("Please input correct number: ");
            }
            
            Console.WriteLine("Please input basenumber for creating {0} reference numbers (3-{1} digits): ", amountNumber, (19- amountNumber.Length));
            int amount = int.Parse(amountNumber);
            // loop for amountNumber
                string referenceNumber = NumberInput(3, (19 - amountNumber.Length));

            for (int k = 0; k < amount; k++)
            {
                // add amountNumber to referenceNumber k times
                string referenceNumber2 = referenceNumber + k;
                int checksum6 = calculateChecksum(referenceNumber2, 2);
                referenceNumber2 = FormatReference(referenceNumber2, checksum6, 2); // if 2 then add, if 1 then no adding
            }
        }
    }
}
