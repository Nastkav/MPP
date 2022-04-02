using System;
using System.IO;

namespace task2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] stopWords = new[] { "the", "of", "are", "a", "and", "or", "at", "by", "for", "am", "to", "him", "they", "do", "does", "dont", "i", "you", "your", "yours", "we", "they", "he", "she", "it", "its", "that", "from", "their", "this",
                "is", "with", "about", "like", "as", "was", "were", "have", "has", "had", "if", "but", "in", "on", "will" , "why", "who", "which", "while"};
            int length = 5;
            (string, int)[] words = new (string, int)[length];
            int n = 0;
            int row = 1;
            int page = 1;
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

                    if (symb == '\n')
                    {
                        row++;
                        page = row / 45 + 1;
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

                    words[n].Item1 = word;
                    words[n].Item2 = page;
                    word = "";
                    n++;
                }

                if (length <= n) //if array index out
                {
                    (string, int)[] tmpWords = words;
                    int cur = 0;
                    length *= 2;
                    words = new (string, int)[length];
                    Move:
                    words[cur] = tmpWords[cur];
                    cur++;
                    if (cur != n)
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
            //for (int i = 0; i < length; i++)
            //    Console.WriteLine(words[i]);

            string[] unique = new string[length];
            int[] countUnique = new int[length];
            int[,] pages = new int[length, length];
            bool sovp = false;
            int i = 0;
            int add = 0;
            int j = 0;
            int repeats = 0;
            int k = 0;
            int str = 0;

            // wrapping words in array with unique words
            CheckUnique:
            if (words[i].Item1 == unique[j] && j < length - 1)
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
                    unique[add] = words[i].Item1;
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

            if (unique[i] != words[j].Item1 && j < length - 1)
            {
                j++;
                goto Check1;
            }
            if (unique[i] == words[j].Item1 && j < length - 1 && words[j].Item1 != null)
            {
                repeats++;
                pages[i, str] = words[j].Item2;
                str++;
                j++;
                goto Check1;
            }
            if (j == length - 1)
            {
                countUnique[i] = repeats;
                repeats = 0;
                i++;
                str = 0;
                j = 0;
                if (i == length - 1)
                    goto Exit;


                goto Check1;
            }
            Exit:
            i = 0;
            int index = 0;
            int tmp;
            int ch;
            string temp;

            BubSort:
            i++;
            j = 0;
            ch = 0;
            if (i < length - 1)
            {
                Sort:
                if (j < length - 1 - i)
                {
                    ch = 0;
                    Chars:
                    if (unique[j + 1] != null)
                    {
                        if (unique[j][ch] > unique[j + 1][ch])
                        {
                            temp = unique[j];
                            unique[j] = unique[j + 1];
                            unique[j + 1] = temp;

                            tmp = countUnique[j];
                            countUnique[j] = countUnique[j + 1];
                            countUnique[j + 1] = tmp;

                            index = 0;
                            Swap:
                            if (pages[j + 1, index] != 0 || pages[j, index] != 0)
                            {
                                tmp = pages[j, index];
                                pages[j, index] = pages[j + 1, index];
                                pages[j + 1, index] = tmp;
                                index++;
                                goto Swap;
                            }
                        }
                        if (unique[j][ch] == unique[j + 1][ch] && ch < unique[j + 1].Length - 1 && ch < unique[j].Length - 1)
                        {
                            ch++;
                            goto Chars;
                        }
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
                Console.Write($"{unique[k]} -  ");
                sw.Write($"{unique[k]} - ");
                Str:
                if (str != length && pages[k, str] != 0)
                {
                    if (pages[k, str] != pages[k, str + 1])
                    {
                        Console.Write($"{pages[k, str]} ");
                        sw.Write($"{pages[k, str]} ");
                    }
                    str++;
                    goto Str;
                }
                Console.WriteLine("");
                sw.WriteLine("");
                str = 0;
                k++;
                goto Result;
            }
        }
    }
}
