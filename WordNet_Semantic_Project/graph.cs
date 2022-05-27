using System;
using System.Collections.Generic;
using System.Text;

namespace WordNet_Semantic_Project
{
    class ansectors
    {
        public char color;
        public int distancefrom1 = 0, distancefrom2 = 0;
    }
    class graph
    {
        private static Dictionary<string,List<int>> ids = new Dictionary<string,List<int>>();
        private static Dictionary<int, ansectors> check;
        private static List<string[]> synsets;
        public static void startUp(List<string[]> synset)           //O(N*n) where N = number of synsets and n = number of nouns
        {
            synsets = synset;                                    // O(1)
            for(int i = 0;i<synset.Count;i++)                    // O(N)  where N = number of synsets
            {
                foreach (string noun in synset[i])               // O(n)   where n = number of nouns in synsets
                {                   
                    if (ids.ContainsKey(noun))                   // O(1)
                    {                        
                        ids[noun].Add(i);                   // O(1)
                    }                    
                    else
                    {                       
                        ids.Add(noun, new List<int>());     // O(1)
                        ids[noun].Add(i);                       // O(1)
                    }
                }
            }          
        }
        static List<int> synsetToIds(string noun)               // O(1)
        {
            return ids[noun];                               // O(1)
        }
        public static string[] idToSynsets(int id)              // O(1)
        {
            return synsets[id];                                 // O(1)
        }
        static int checkParents(int id,char color,List<List<int>> parents)      // O(N)   where N = number of parents for id
        {
            List<int> commparents = new List<int>();        // O(1)
            int distance = 1000000, commid = -1;            // O(1)
            if (!check.ContainsKey(id))                     // O(1)
            {                
                check.Add(id, new ansectors());             // O(1)
                check[id].color = 'w';                      // O(1)
            }   
            Queue<int> q = new Queue<int>();                // O(1)
            List<int> parent;                               
            q.Enqueue(id);                                  // O(1)
            while (q.Count > 0)                             // O(N)    where N = number of parents for id
            {
                int temp = q.Dequeue();                     // O(1)
                parent = parents[temp];                     // O(1)

                if (color == 'w')                           // O(1)
                    check[temp].color = 'b';                // O(1)
                else if (check[temp].color == 'b' && color == 'p')      // O(1)
                {
                    if (!commparents.Contains(temp))        // O(1)
                        commparents.Add(temp);              // O(1)
                }
                 
                for (int i = 0; i < parent.Count; i++)      // O(m)     where m = number of parents for temp id
                {            
                    if (!check.ContainsKey(parent[i]))      // O(1)
                    {
                        check.Add(parent[i], new ansectors());     // O(1)                  
                        if (color == 'w')                   // O(1)
                        {
                            check[parent[i]].color = 'w';   // O(1)
                            if (check[parent[i]].distancefrom1 == 0)        // O(1)
                            {
                                check[parent[i]].distancefrom1 = check[temp].distancefrom1 + 1;     // O(1)
                            }                           
                        }          
                        else
                            check[parent[i]].distancefrom2 = check[temp].distancefrom2 + 1;     // O(1)
                    }
                    else
                    {
                        if (color == 'w')       // O(1)
                        {
                            check[parent[i]].color = 'b';       // O(1)
                            if (check[parent[i]].distancefrom1 == 0)        // O(1)
                            {
                                check[parent[i]].distancefrom1 = check[temp].distancefrom1 + 1;     // O(1)
                            }
                            else if (check[parent[i]].distancefrom1 > check[temp].distancefrom1 + 1)    // O(1)
                            {
                                check[parent[i]].distancefrom1 = check[temp].distancefrom1 + 1;     // O(1)
                            }
                        }                     
                        else
                        {
                            if (check[parent[i]].distancefrom2 == 0)        // O(1)
                            {
                                check[parent[i]].distancefrom2 = check[temp].distancefrom2 + 1;     // O(1)
                            }
                            else if (check[parent[i]].distancefrom2 > check[temp].distancefrom2 + 1)    // O(1)
                            {
                                check[parent[i]].distancefrom2 = check[temp].distancefrom2 + 1;     // O(1)
                            }
                        }
                        
                    }
                    q.Enqueue(parent[i]);       // O(1)
                }              
            }
            for (int i = 0;i < commparents.Count; i++)      // O(n)   where n = number of common parents for both ids
            {
                int tempdis = check[commparents[i]].distancefrom1 + check[commparents[i]].distancefrom2;    // O(1)
                if (tempdis < distance)     // O(1)
                {
                    distance = tempdis;     // O(1)
                    commid = commparents[i];    // O(1)
                }                
            }
            return commid;      // O(1)
        }
        
        public static int[] relatedSemanted(string[] nouns,List<List<int>> parents)     //O(N*M*n*m) where N = number of ids for noun1 and M = number of ids for noun2
        {   // and where n = number of parents for id1, m = number of parents for id2
            check = new Dictionary<int, ansectors>();       //O(1)
            int[] ans = new int[2];                         //O(1)
            ans[0] = 10000000;                              //O(1)
            int distance1, ansector1;                       //O(1)
            List<int> ids1 = synsetToIds(nouns[0]), ids2 = synsetToIds(nouns[1]);   //O(1)

            foreach (int id1 in ids1)               //O(N) where N = number of ids of noun1
            {
                foreach(int id2 in ids2)            //O(M) where M = number of ids of noun1
                {                    
                    if (id1 == id2)             //O(1)
                    {
                        ans[0] = 0;             //O(1)
                        ans[1] = id1;           //O(1)
                        break;
                    }
                    checkParents(id1, 'w', parents);        //O(n) where n = number of parents for id1
                    ansector1 = checkParents(id2, 'p', parents);        //O(m)  where m = number of parents for id2 
                    distance1 = check[ansector1].distancefrom1 + check[ansector1].distancefrom2;          //O(1)          
                    check.Clear();          //O(1)
                    if ( distance1<ans[0])      //O(1)
                    {
                        ans[0] = distance1;         //O(1)
                        ans[1] = ansector1;         //O(1)
                    }                  
                }
            }
            return ans;         //O(1)
        }

        public static string outCast(string[] nouns, List<List<int>> parents)       //O(n^2*N*M)
        {
            int distance = 0;       //O(1)
            string ans = "";        //O(1)
            for (int i = 0; i < nouns.Length; i++)      //O(n)  where n = number of nouns
            {
                int temp = 0;           //O(1)
                for (int y = 0; y < nouns.Length; y++)      //O(n)  where n = number of nouns
                {
                    if (y != i)     //O(1)
                    {
                        string[] comp = { nouns[i], nouns[y] };     //O(1)
                        temp += relatedSemanted(comp, parents)[0];      //O(N*M) where N,M is the number of ids for each noun
                    }                  
                }
                if (temp > distance)        //O(1)
                {
                    distance = temp;        //O(1)
                    ans = nouns[i];         //O(1)
                }
            }
            return ans;     //O(1)
        }

    }
}