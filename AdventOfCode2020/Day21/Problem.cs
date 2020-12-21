using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day21
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("daytwentyone.txt");
            var test = InputParser.GetLines("daytwentyone-test.txt");
            var (allergens, allIngredients, ingredientLists) = ParseAllergens(actual);

            allergens.ForEach(a => a.PopulateAllergenCandidates());

            List<string> notAllergens = new List<string>();

            foreach (string ingredient in allIngredients)
            {
                bool doesAppear = false;
                foreach (Allergen allergen in allergens)
                {
                    doesAppear = allergen.AllergenCandidates.Any(c => c.Equals(ingredient));
                    if (doesAppear is true)
                        break;
                }
                if (doesAppear is false)
                    notAllergens.Add(ingredient);
            }

            int count = 0;

            foreach (string ingredient in notAllergens)
            {
                foreach (List<string> ingredientsList in ingredientLists)
                {
                    if (ingredientsList.Any(l => l.Equals(ingredient)))
                        count++;
                }
            }

            //Console.WriteLine(count);
            CompileCanonicalList(allergens);
        }

        class Allergen
        {
            public string Name { get; set; }
            public List<List<string>> IngredientLists { get; set; } = new List<List<string>>();
            public List<string> AllergenCandidates { get; set; }

            public void PopulateAllergenCandidates()
            {
                List<string> intersection = IngredientLists[0];
                for (int i = 1; i < IngredientLists.Count(); i++)
                {
                    intersection = intersection.Intersect(IngredientLists[i]).ToList();
                }
                AllergenCandidates = intersection;
            }
        }

        private static (List<Allergen>, List<string>, List<List<string>>) ParseAllergens(string[] input)
        {
            List<Allergen> allergens = new List<Allergen>();
            List<string> allIngredients = new List<string>();
            List<List<string>> ingredientLists = new List<List<string>>();
            foreach(string line in input)
            {
                var split = line.Split('(');

                List<string> ingredients = new List<string>();
                var foods = split[0].Split(' ');
                foreach (string food in foods)
                {
                    if (string.IsNullOrEmpty(food))
                        continue;
                    ingredients.Add(food.Trim());
                }

                allIngredients.AddRange(ingredients);
                ingredientLists.Add(ingredients);
                
                var allergenNames = split[1].Split(',');
                foreach (string name in allergenNames)
                {
                    var cleanedName = name;
                    if (cleanedName.Contains("contains"))
                        cleanedName = cleanedName.Replace("contains", string.Empty).Trim();

                    if (cleanedName.Contains(")"))
                        cleanedName = cleanedName.Replace(")", string.Empty).Trim();

                    var allergen = allergens.FirstOrDefault(a => a.Name.Equals(cleanedName.Trim()));

                    if (allergen is null)
                    {
                        allergen = new Allergen
                        {
                            Name = cleanedName.Trim()
                        };
                        allergens.Add(allergen);
                    }

                    allergen.IngredientLists.Add(ingredients);
                }
            }

            allIngredients = allIngredients.Distinct().ToList();

            return (allergens, allIngredients, ingredientLists);
        }

        private static void CompileCanonicalList(List<Allergen> allergens)
        {
            List<string> allCandidates = new List<string>();

            while (IsThereOverlap(allergens))
            {
                WhittleDown(allergens);
            }

            var candidateLists = allergens.OrderBy(a => a.Name).Select(a => a.AllergenCandidates).ToList();

            foreach (List<string> candidates in candidateLists)
            {
                foreach (string candidate in candidates)
                {
                    allCandidates.Add(candidate);
                }
            }

            var sol = string.Join(',', allCandidates);
            Console.WriteLine(sol);
        }

        private static bool IsThereOverlap(List<Allergen> allergens)
        {
            foreach (Allergen allergen in allergens)
            {
                foreach (Allergen sub in allergens)
                {
                    if (allergen.Name.Equals(sub.Name))
                        continue;

                    if (allergen.AllergenCandidates.Intersect(sub.AllergenCandidates).Any())
                        return true;
                }
            }
            return false;
        }

        private static void WhittleDown(List<Allergen> allergens)
        {
            var guaranteed = allergens.Where(a => a.AllergenCandidates.Count() == 1).ToList();
            var allOthers = allergens.Where(a => a.AllergenCandidates.Count() > 1).ToList();

            foreach (Allergen hasSingle in guaranteed)
            {
                foreach (Allergen hasMultiple in allOthers)
                {
                    hasMultiple.AllergenCandidates.Remove(hasSingle.AllergenCandidates.First());
                }
            }
        }
    }
}
