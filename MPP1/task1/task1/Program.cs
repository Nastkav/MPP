using System;
using System.IO;

namespace task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] stopWords = new[] { "the", "of", "are", "a", "and", "or", "at", "by", "for", "am", "to", "him", "they", "do", "i", "you", "we", "they", "he", "she", "it", "its", "that", "from", "their", "this",
                "is", "with", "about", "like", "as", "was", "were", "have", "has", "had", "if", "but", "in", "on", "will" };
            int length = 5;
            string[] words = new string[length];
            int[] pages = new int[length];
            int n = 0;
            string path = "text.txt";

            using (StreamReader sr = new StreamReader(path)) // read symbols for a word
            {
                string word = "";
                Symbols:
                char symb = (char)sr.Read();

                if (symb >= 'A' && symb <= 'Z')
                    symb += (char)32;
                if (symb >= 'a' && symb <= 'z')
                {
                    word = word + symb;
                    goto Symbols;
                }

                if (symb == '\n' || symb == ' ')
                {
                    if (word == "")
                    {
                        goto Symbols;
                    }
                    int ind = 0;
                    Check:
                    if (ind != stopWords.Length)
                    {
                        if (stopWords[ind] == word)
                        {
                            word = "";
                            goto Symbols;
                        }
                        ind++;
                        goto Check;
                    }

                    words[n] = word;
                    word = "";
                    n++;
                }

                if (length <= n) //if array index out
                {
                    string[] tmpWords = words;
                    int currIndex = 0;
                    length *= 2;
                    words = new string[length];
                    Move:
                    words[currIndex] = tmpWords[currIndex];
                    currIndex++;
                    if (currIndex != n)
                    {
                        goto Move;
                    }
                    goto Symbols;
                }


                if (!sr.EndOfStream)
                {
                    goto Symbols;
                }
            }

            string[] unique = new string[length];
            int[] countUnique = new int[length];
            bool sovp = false;
            int i = 0;
            int add = 0;
            int j = 0;
            int repeats = 0;
            int k = 0;

            // wrapping words in array with unique words
            CheckUnique:
            if (words[i] == unique[j] && j < length - 1)
            {
                sovp = true;
            }
            if (j < length - 1)
            {
                j++;
                goto CheckUnique;
            }
            else
            {
                if (!sovp)
                {
                    unique[add] = words[i];
                    add++;
                }
                sovp = false;
                i++;
                j = 0;
                if (i == length - 1)
                {
                    i = 0;
                    goto Check1;
                }
                goto CheckUnique;
            }



            // count of repeats
            Check1:

            if (unique[i] != words[j] && j < length - 1)
            {
                j++;
                goto Check1;
            }
            if (unique[i] == words[j] && j < length - 1 && words[j] != null)
            {
                repeats++;
                j++;
                goto Check1;
            }
            if (j == length - 1)
            {
                countUnique[i] = repeats;
                repeats = 0;
                i++;
                j = 0;
                if (i == length - 1)
                    goto Exit;


                goto Check1;
            }
            Exit:
            i = 0;
            int tmp;
            string temp;

            BubSort:
            i++;
            j = 0;
            if (i < length - 1)
            {
                Sort:
                if (j < length - 1 - i)
                {
                    if (countUnique[j] < countUnique[j + 1])
                    {
                        tmp = countUnique[j];
                        countUnique[j] = countUnique[j + 1];
                        countUnique[j + 1] = tmp;
                        tmp = 0;
                        temp = unique[j];
                        unique[j] = unique[j + 1];
                        unique[j + 1] = temp;
                    }
                    j++;
                    goto Sort;
                }
                else
                    goto BubSort;
            }

            using StreamWriter sw = new StreamWriter("result.txt");
            Result:
            if (length - 1 != k && unique[k] != null)
            {
                Console.WriteLine($"{unique[k]} - {countUnique[k]}");
                sw.WriteLine($"{unique[k]} - {countUnique[k]}");
                k++;
                goto Result;
            }


        }
    }
}
