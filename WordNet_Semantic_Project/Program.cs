using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace WordNet_Semantic_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string[]> files = new List<string[]>();        //O(1)
            FileStream testsFile, file, file1,fileWriter;       //O(1)
            StreamReader testsReader, sr,sr1;                   //O(1)
            StreamWriter testsWriter;                           //O(1)
            string lineReader, line, lin;                       //O(1)
            int cases, wrongAnswers=0,choice2,choice1;          //O(1)
            List<string[]> synsets = new List<string[]>();      //O(1)
            List<List<int>> parents = new List<List<int>>();    //O(1)
            Dictionary<int, List<int>> children = new Dictionary<int, List<int>>();     //O(1)
            Stopwatch stopwatch = new Stopwatch();      //O(1)
            Console.WriteLine("Hello :)");      //O(1)
            Console.WriteLine();        //O(1)

            Console.WriteLine("-----------------------------------------------------");     //O(1)
            Console.WriteLine("1- Sample test.");   //O(1)
            Console.WriteLine("2- Complete test."); //O(1)
            Console.WriteLine();        //O(1)
            choice1 = int.Parse(Console.ReadLine());    //O(1)
            switch (choice1)        //O(1)
            {
                case 1:
                    #region Sample test cases
                    files = new List<string[]>();       //O(1)
                    testsFile = new FileStream("sampleTests.txt", FileMode.Open, FileAccess.Read);  //O(1)
                    testsReader = new StreamReader(testsFile);      //O(1)
                    while (!testsReader.EndOfStream)        //O(L) where L = number of lines 
                    {
                        lineReader = testsReader.ReadLine();    //O(1)
                        files.Add(lineReader.Split(","));       //O(1)
                    }
                    for(int i = 0; i < files.Count; i++)        //O(f) where f = number of files
                    {
                        Console.WriteLine((i+1)+"- "+files[i][0]);  //O(1)
                    }
                    Console.WriteLine();    //O(1)
                    choice2 = int.Parse(Console.ReadLine());    //O(1)
                    file = new FileStream(files[choice2-1][1]+ files[choice2 - 1][2] + ".txt", FileMode.Open, FileAccess.Read); //O(1)
                    file1 = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][3] + ".txt", FileMode.Open, FileAccess.Read); //O(1)
                    sr = new StreamReader(file);    //O(1)
                    sr1 = new StreamReader(file1);  //O(1)
                    cases = 0;    //O(1)
                    wrongAnswers = 0;   //O(1)
                    while (!sr.EndOfStream)     //O(n) where n = number of synsets and hye
                    {
                        line = sr.ReadLine();   //O(1)
                        lin = sr1.ReadLine();   //O(1)  
                        string[] vs2 = lin.Split(",");  //O(1)
                        string[] vs = line.Split(",");  //O(1)
                        string[] vs1 = vs[1].Split(" ");    //O(1)
                        synsets.Add(vs1);   //O(1)

                        parents.Add(new List<int>());   //O(1)
                        for (int y = 1; y < vs2.Length; y++)    //O(P) where P = number of parents
                        {
                            int id = int.Parse(vs2[y]); //O(1)

                            parents[cases].Add(id);     //O(1)
                        }
                        cases++;    //O(1)
                    }
                    graph.startUp(synsets);     //O(N*n) where N = number of synsets and n = number of nouns
                    sr.Close();         //O(1)
                    file.Close();       //O(1)
                    sr1.Close();        //O(1)
                    file1.Close();      //O(1)
                    fileWriter = new FileStream("testAnswers/"+"Sample" +files[choice2-1][0]+ "result1.txt", FileMode.Create);      //O(1)
                    testsWriter = new StreamWriter(fileWriter); //O(1)

                    file = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][4] + ".txt", FileMode.Open, FileAccess.Read);  //O(1)
                    if (files[choice2-1].Length==8) //O(1)
                        file1 = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][6] + ".txt", FileMode.Open, FileAccess.Read); //O(1)
                    else
                        file1 = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][5] + ".txt", FileMode.Open, FileAccess.Read); //O(1)
                    sr = new StreamReader(file);    //O(1)
                    sr1 = new StreamReader(file1);  //O(1)
                    line = sr.ReadLine();           //O(1)  
                    cases = int.Parse(line);        //O(1)           
                    Console.WriteLine("------------------Relation Semantic------------------"); //O(1)
                    for (int i = 0; i < cases; i++)     //O(N) where N = number of cases
                    {
                        line = sr.ReadLine();
                        lin = sr1.ReadLine();
                        string[] correctAns = lin.Split(",");
                        string[] synArr = correctAns[1].Split(" OR ");
                        string[] correctsyn = synArr[0].Split(" ");
                        string[] vs = line.Split(",");

                        int[] ans = graph.relatedSemanted(vs, parents);
                        testsWriter.Write(ans[0].ToString() + ",");
                        foreach (string noun in graph.idToSynsets(ans[1]))
                        {
                            testsWriter.Write(noun + " ");
                        }
                        testsWriter.WriteLine();
                        if (ans[0] != int.Parse(correctAns[0]) || correctsyn.Length != synsets[ans[1]].Length)
                        {
                            if (synArr.Length > 1)
                            {
                                if (ans[0] != int.Parse(correctAns[0]) || synArr[1].Split(" ").Length != synsets[ans[1]].Length)
                                {
                                    Console.WriteLine(line);
                                    wrongAnswers++;
                                    Console.Write(ans[0] + " ");

                                    foreach (string noun in synsets[ans[1]])
                                    {
                                        Console.Write(noun + " ");
                                    }
                                    Console.WriteLine();
                                    Console.WriteLine("wrong answer at Relation semantic case #" + (i + 1));
                                    Console.WriteLine(lin);
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine(line);
                                wrongAnswers++;
                                Console.Write(ans[0] + " ");

                                foreach (string noun in synsets[ans[1]])
                                {
                                    Console.Write(noun + " ");
                                }
                                Console.WriteLine();
                                Console.WriteLine("wrong answer at Relation semantic case #" + (i + 1));
                                Console.WriteLine(lin);
                                Console.WriteLine();
                            }
                        }

                        if (stopwatch.ElapsedMilliseconds > 60000)
                        {
                            Console.WriteLine("time");
                        }
                    }
                    testsWriter.Close();
                    if (files[choice2 - 1].Length == 8)
                    {
                        
                        file = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][5] + ".txt", FileMode.Open, FileAccess.Read);                    
                        file1 = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][7] + ".txt", FileMode.Open, FileAccess.Read);
                        fileWriter = new FileStream("testAnswers/" + "Sample" + files[choice2 - 1][0] + "result2.txt", FileMode.Create);
                        testsWriter = new StreamWriter(fileWriter);
                        sr = new StreamReader(file);
                        sr1 = new StreamReader(file1);
                        line = sr.ReadLine();
                        cases = int.Parse(line);
                        Console.WriteLine("----------------------Outcast------------------------");
                        for (int i = 0; i < cases; i++)
                        {
                            line = sr.ReadLine();
                            lin = sr1.ReadLine();
                            string[] vs = line.Split(",");
                            List<string> correctAns = lin.Split(" OR ").ToList();
                            stopwatch.Start();
                            string ans = graph.outCast(vs, parents);
                            stopwatch.Stop();
                            testsWriter.WriteLine(ans);
                            if (!correctAns.Contains(ans))
                            {
                                wrongAnswers++;
                                Console.WriteLine("wrong answer at Outcast case #" + (i + 1));
                            }
                            if (stopwatch.ElapsedMilliseconds > 60000)
                            {
                                Console.WriteLine("time");
                            }

                        }
                        testsWriter.Close();
                    }
                    
                    Console.WriteLine("-----------------------------------------------------");
                    if (wrongAnswers == 0)
                        Console.WriteLine("Congratulation");
                    break;
                #endregion
                case 2:
                    #region Complete test cases
                    testsFile = new FileStream("CompleteTests.txt", FileMode.Open, FileAccess.Read);
                    testsReader = new StreamReader(testsFile);
                    while (!testsReader.EndOfStream)
                    {
                        lineReader = testsReader.ReadLine();
                        files.Add(lineReader.Split(","));
                    }
                    for (int i = 0; i < files.Count; i++)
                    {
                        Console.WriteLine((i + 1) + "- " + files[i][0]);
                    }
                    Console.WriteLine();
                    choice2 = int.Parse(Console.ReadLine());
                    synsets = new List<string[]>();
                    parents = new List<List<int>>();
                    children = new Dictionary<int, List<int>>();
                    file = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][2] + ".txt", FileMode.Open, FileAccess.Read);
                    file1 = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][3] + ".txt", FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(file);
                    sr1 = new StreamReader(file1);
                    cases = 0;
                    wrongAnswers = 0;
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        lin = sr1.ReadLine();
                        string[] vs2 = lin.Split(",");
                        string[] vs = line.Split(",");
                        string[] vs1 = vs[1].Split(" ");
                        synsets.Add(vs1);

                        parents.Add(new List<int>());
                        for (int y = 1; y < vs2.Length; y++)
                        {
                            int id = int.Parse(vs2[y]);                          
                            parents[cases].Add(id);
                        }
                        cases++;
                    }
                    stopwatch.Start();
                    graph.startUp(synsets);
                    stopwatch.Stop();
                    Console.WriteLine(stopwatch.ElapsedMilliseconds);
                    sr.Close();
                    file.Close();
                    sr1.Close();
                    file1.Close();
                    fileWriter = new FileStream("testAnswers/" + "Complete" + files[choice2 - 1][0] + "result1.txt", FileMode.Create);
                    testsWriter = new StreamWriter(fileWriter);
                    file = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][4] + ".txt", FileMode.Open, FileAccess.Read);
                    if (files[choice2 - 1].Length == 8)
                        file1 = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][6] + ".txt", FileMode.Open, FileAccess.Read);
                    else
                        file1 = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][5] + ".txt", FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(file);
                    sr1 = new StreamReader(file1);
                    line = sr.ReadLine();
                    cases = int.Parse(line);
                    Console.WriteLine("------------------Relation Semantic------------------");
                    stopwatch.Restart();
                    for (int i = 0; i < cases; i++)
                    {

                        line = sr.ReadLine();
                        lin = sr1.ReadLine();
                        string[] correctAns = lin.Split(",");
                        string[] synArr = correctAns[1].Split(" OR ");
                        string[] correctsyn = synArr[0].Split(" ");
                        string[] vs = line.Split(",");
                                               
                        int[] ans = graph.relatedSemanted(vs, parents);
                       

                        testsWriter.Write(ans[0] + ",");

                        foreach (string noun in graph.idToSynsets(ans[1]))
                        {
                            testsWriter.Write(noun + " ");
                        }
                        testsWriter.WriteLine();
                        if (ans[0] != int.Parse(correctAns[0]) || correctsyn.Length != synsets[ans[1]].Length)
                        {
                            if (synArr.Length > 1)
                            {
                                bool correct = false;
                                for (int y = 1; y < synArr.Length; y++)
                                {
                                    if (ans[0] == int.Parse(correctAns[0]) && synArr[y].Split(" ").Length == synsets[ans[1]].Length)
                                    {
                                        correct = true;
                                    }
                                }
                                if (!correct)
                                {
                                    Console.WriteLine(line);
                                    wrongAnswers++;
                                    Console.Write(ans[0] + " ");

                                    foreach (string noun in synsets[ans[1]])
                                    {
                                        Console.Write(noun + " ");
                                    }
                                    Console.WriteLine();
                                    Console.WriteLine("wrong answer at Relation semantic case #" + (i + 1));
                                    Console.WriteLine(lin);
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine(line);
                                wrongAnswers++;
                                Console.Write(ans[0] + " ");

                                foreach (string noun in synsets[ans[1]])
                                {
                                    Console.Write(noun + " ");
                                }
                                Console.WriteLine();
                                Console.WriteLine("wrong answer at Relation semantic case #" + (i + 1));
                                Console.WriteLine(lin);
                                Console.WriteLine();
                            }
                        }                  
                    }
                    testsWriter.Close();
                    stopwatch.Stop();
                    Console.WriteLine("Time for SCA in milliseconds: " + stopwatch.ElapsedMilliseconds + ", in seconds: " + stopwatch.ElapsedMilliseconds/1000);
                    if (files[choice2 - 1].Length == 8)
                    {
                        file = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][5] + ".txt", FileMode.Open, FileAccess.Read);
                        file1 = new FileStream(files[choice2 - 1][1] + files[choice2 - 1][7] + ".txt", FileMode.Open, FileAccess.Read);
                        fileWriter = new FileStream("testAnswers/" + "Complete" + files[choice2 - 1][0] + "result2.txt", FileMode.Create);
                        testsWriter = new StreamWriter(fileWriter);
                        sr = new StreamReader(file);
                        sr1 = new StreamReader(file1);
                        line = sr.ReadLine();
                        cases = int.Parse(line);
                        Console.WriteLine("----------------------Outcast------------------------");
                        stopwatch.Restart();
                        for (int i = 0; i < cases; i++)
                        {
                            line = sr.ReadLine();
                            lin = sr1.ReadLine();
                            string[] vs = line.Split(",");
                            List<string> correctAns = lin.Split(" OR ").ToList();
                            stopwatch.Start();
                            string ans = graph.outCast(vs, parents);
                            stopwatch.Stop();
                            testsWriter.WriteLine(ans);
                            if (!correctAns.Contains(ans))
                            {
                                wrongAnswers++;
                                Console.WriteLine("wrong answer at Outcast case #" + (i + 1));
                            }
                            if (stopwatch.ElapsedMilliseconds > 60000)
                            {
                                Console.WriteLine("time");
                            }

                        }
                        testsWriter.Close();
                        stopwatch.Stop();
                        Console.WriteLine("Time for Outcast in milliseconds: " + stopwatch.ElapsedMilliseconds + ", in seconds: " + stopwatch.ElapsedMilliseconds / 1000);
                    }
                    Console.WriteLine("-----------------------------------------------------");
                    if (wrongAnswers == 0)
                        Console.WriteLine("Congratulation");
                    break;
                    #endregion
            }

        }
    }
}
