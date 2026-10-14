
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ASSLINQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var products = ListGenerators.ProductList;
            var customers = ListGenerators.CustomerList;

            int[] Arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            string[] words = File.ReadAllLines(
                Path.Combine(AppContext.BaseDirectory, "dictionary_english.txt"));

            // ELEMENT OPERATORS

            // Q1: First product out of stock
            var e1 = products.FirstOrDefault(p => p.UnitsInStock == 0);
            Console.WriteLine("Element Q1:");
            Console.WriteLine(e1);

            // Q2: First product whose price > 1000
            var e2 = products.FirstOrDefault(p => p.UnitPrice > 1000);
            Console.WriteLine("\nElement Q2:");
            Console.WriteLine(e2);

            // Q3: Second number greater than 5
            var e3 = Arr.Where(n => n > 5).Skip(1).FirstOrDefault();
            Console.WriteLine("\nElement Q3:");
            Console.WriteLine(e3);

            // AGGREGATE OPERATORS

            // Q1: Count odd numbers
            var a1 = Arr.Count(n => n % 2 != 0);
            Console.WriteLine("\nAggregate Q1:");
            Console.WriteLine(a1);

            // Q2: Customers and number of orders
            var a2 = customers.Select(c => new
            {
                c.CustomerName,
                OrdersCount = c.Orders.Length
            });

            Console.WriteLine("\nAggregate Q2:");
            foreach (var c in a2)
                Console.WriteLine($"{c.CustomerName}: {c.OrdersCount}");

            // Q3: Categories and number of products
            var a3 = products.GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                });

            Console.WriteLine("\nAggregate Q3:");
            foreach (var c in a3)
                Console.WriteLine($"{c.Category}: {c.Count}");

            // Q4: Sum of numbers
            var a4 = Arr.Sum();
            Console.WriteLine("\nAggregate Q4:");
            Console.WriteLine(a4);

            // Q5: Total characters in dictionary
            var a5 = words.Sum(w => w.Length);
            Console.WriteLine("\nAggregate Q5:");
            Console.WriteLine(a5);

            // Q6: Length of shortest word
            var a6 = words.Min(w => w.Length);
            Console.WriteLine("\nAggregate Q6:");
            Console.WriteLine(a6);

            // Q7: Length of longest word
            var a7 = words.Max(w => w.Length);
            Console.WriteLine("\nAggregate Q7:");
            Console.WriteLine(a7);

            // Q8: Average word length
            var a8 = words.Average(w => w.Length);
            Console.WriteLine("\nAggregate Q8:");
            Console.WriteLine(a8);

            // Q9: Total units in stock per category
            var a9 = products.GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalUnits = g.Sum(p => p.UnitsInStock)
                });

            Console.WriteLine("\nAggregate Q9:");
            foreach (var c in a9)
                Console.WriteLine($"{c.Category}: {c.TotalUnits}");

            // Q10: Cheapest price per category
            var a10 = products.GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    CheapestPrice = g.Min(p => p.UnitPrice)
                });

            Console.WriteLine("\nAggregate Q10:");
            foreach (var c in a10)
                Console.WriteLine($"{c.Category}: {c.CheapestPrice}");

            // Q11: Cheapest products per category using let
            var a11 =
                from p in products
                group p by p.Category into categoryGroup
                let minPrice = categoryGroup.Min(p => p.UnitPrice)
                select new
                {
                    Category = categoryGroup.Key,
                    CheapestProducts = categoryGroup
                        .Where(p => p.UnitPrice == minPrice)
                };

            Console.WriteLine("\nAggregate Q11:");
            foreach (var c in a11)
            {
                Console.WriteLine(c.Category);
                foreach (var p in c.CheapestProducts)
                    Console.WriteLine(p);
            }

            // Q12: Most expensive price per category
            var a12 = products.GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    HighestPrice = g.Max(p => p.UnitPrice)
                });

            Console.WriteLine("\nAggregate Q12:");
            foreach (var c in a12)
                Console.WriteLine($"{c.Category}: {c.HighestPrice}");

            // Q13: Most expensive products per category
            var a13 = products.GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    ExpensiveProducts = g.Where(
                        p => p.UnitPrice == g.Max(x => x.UnitPrice))
                });

            Console.WriteLine("\nAggregate Q13:");
            foreach (var c in a13)
            {
                Console.WriteLine(c.Category);
                foreach (var p in c.ExpensiveProducts)
                    Console.WriteLine(p);
            }

            // Q14: Average price per category
            var a14 = products.GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    AveragePrice = g.Average(p => p.UnitPrice)
                });

            Console.WriteLine("\nAggregate Q14:");
            foreach (var c in a14)
                Console.WriteLine($"{c.Category}: {c.AveragePrice:F2}");

            // SET OPERATORS

            // Q1: Unique category names
            var s1 = products.Select(p => p.Category).Distinct();

            Console.WriteLine("\nSet Q1:");
            foreach (var category in s1)
                Console.WriteLine(category);

            var productLetters = products.Select(p => p.ProductName[0]);
            var customerLetters = customers.Select(c => c.CustomerName[0]);

            // Q2: Unique first letters
            var s2 = productLetters.Union(customerLetters);

            Console.WriteLine("\nSet Q2:");
            foreach (var letter in s2)
                Console.WriteLine(letter);

            // Q3: Common first letters
            var s3 = productLetters.Intersect(customerLetters);

            Console.WriteLine("\nSet Q3:");
            foreach (var letter in s3)
                Console.WriteLine(letter);

            // Q4: Product first letters not in customer first letters
            var s4 = productLetters.Except(customerLetters);

            Console.WriteLine("\nSet Q4:");
            foreach (var letter in s4)
                Console.WriteLine(letter);

            // Q5: Last three characters, including duplicates
            var customerEndings = customers.Select(c =>
                c.CustomerName.Length >= 3
                    ? c.CustomerName.Substring(c.CustomerName.Length - 3)
                    : c.CustomerName);

            var productEndings = products.Select(p =>
                p.ProductName.Length >= 3
                    ? p.ProductName.Substring(p.ProductName.Length - 3)
                    : p.ProductName);

            var s5 = customerEndings.Concat(productEndings);

            Console.WriteLine("\nSet Q5:");
            foreach (var ending in s5)
                Console.WriteLine(ending);

            // PARTITIONING OPERATORS

            // Q1: First 3 orders from customers in Washington
            var washingtonOrders = customers
                .Where(c => c.City == "Washington")
                .SelectMany(c => c.Orders)
                .Take(3);

            Console.WriteLine("\nPartition Q1:");
            foreach (var order in washingtonOrders)
                Console.WriteLine(order);

            // Q2: All but the first 2 orders from Washington
            var washingtonOrders2 = customers
                .Where(c => c.City == "Washington")
                .SelectMany(c => c.Orders)
                .Skip(2);

            Console.WriteLine("\nPartition Q2:");
            foreach (var order in washingtonOrders2)
                Console.WriteLine(order);

            // Q3: Take elements until number is less than its index
            var p3 = Arr.TakeWhile((n, index) => n >= index);

            Console.WriteLine("\nPartition Q3:");
            foreach (var n in p3)
                Console.WriteLine(n);

            // Q4: Start from the first number divisible by 3
            var p4 = Arr.SkipWhile(n => n % 3 != 0);

            Console.WriteLine("\nPartition Q4:");
            foreach (var n in p4)
                Console.WriteLine(n);

            // Q5: Start from first number less than its index
            var p5 = Arr.SkipWhile((n, index) => n >= index);

            Console.WriteLine("\nPartition Q5:");
            foreach (var n in p5)
                Console.WriteLine(n);

            // QUANTIFIERS

            // Q1: Check if any word contains "ei"
            var q1 = words.Any(w => w.Contains("ei"));

            Console.WriteLine("\nQuantifiers Q1:");
            Console.WriteLine(q1);

            // Q2: Categories with at least one product out of stock
            var q2 = products.GroupBy(p => p.Category)
                .Where(g => g.Any(p => p.UnitsInStock == 0));

            Console.WriteLine("\nQuantifiers Q2:");
            foreach (var category in q2)
            {
                Console.WriteLine(category.Key);
                foreach (var p in category)
                    Console.WriteLine(p);
            }

            // Q3: Categories where all products are in stock
            var q3 = products.GroupBy(p => p.Category)
                .Where(g => g.All(p => p.UnitsInStock > 0));

            Console.WriteLine("\nQuantifiers Q3:");
            foreach (var category in q3)
            {
                Console.WriteLine(category.Key);
                foreach (var p in category)
                    Console.WriteLine(p);
            }

            // GROUPING OPERATORS

            // Q1: Group numbers by remainder when divided by 5
            List<int> numbers = new List<int>
            {
                0, 1, 2, 3, 4, 5, 6, 7,
                8, 9, 10, 11, 12, 13, 14, 15
            };

            var g1 = numbers.GroupBy(n => n % 5);

            Console.WriteLine("\nGrouping Q1:");
            foreach (var group in g1)
            {
                Console.WriteLine(
                    $"Numbers with a remainder of {group.Key} when divided by 5:");

                foreach (var n in group)
                    Console.WriteLine(n);
            }

            // Q2: Group dictionary words by first letter
            var g2 = words.Where(w => !string.IsNullOrEmpty(w))
                .GroupBy(w => char.ToLowerInvariant(w[0]));

            Console.WriteLine("\nGrouping Q2:");
            foreach (var group in g2)
            {
                Console.WriteLine($"Letter {group.Key}:");
                foreach (var word in group)
                    Console.WriteLine(word);
            }

            // Q3: Group words containing the same characters
            string[] ArrWords =
            {
                "from", "salt", "earn", "last", "near", "form"
            };

            var g3 = ArrWords.GroupBy(w => w, new AnagramComparer());

            Console.WriteLine("\nGrouping Q3:");
            foreach (var group in g3)
            {
                foreach (var word in group)
                    Console.WriteLine(word);

                Console.WriteLine("...");
            }
        }
    }

    // Comparer for words containing the same characters
    class AnagramComparer : IEqualityComparer<string>
    {
        private string SortCharacters(string word)
        {
            return new string(word.OrderBy(c => c).ToArray());
        }

        public bool Equals(string x, string y)
        {
            if (x == null || y == null)
                return x == y;

            return SortCharacters(x) == SortCharacters(y);
        }

        public int GetHashCode(string word)
        {
            return SortCharacters(word).GetHashCode();
        }
    }
}
