using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextDecorder
{
    public class PostandWords
    {
        public BigList Lists { get; set; }
        public PostandWords()
        {
            EntirePost = new List<string>();
            Words = new Dictionary<string, PostandNumber>();
            StartingWords = new Dictionary<string, PostandNumber>();
            EndingWords = new Dictionary<string, PostandNumber>();
        }

        private void Insert(string Item, string Values, int i, Dictionary<string, PostandNumber> List)
        {
            if (!List.ContainsKey(Item))
                List.Add(Item, new PostandNumber() { Add = (Values) });
            else
            {
                if (!List[Item].FollowWords.ContainsKey(Values))
                {
                    List[Item].Add = Values;
                }
                else
                    List[Item].FollowWords[Values]++;
            }
        }

        public string Item
        {
            set
            {
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                value = rgx.Replace(value, "");
                value = value.Replace("  ", " ");
                EntirePost.Add(value);

                

                int i = 0;
                bool Exit = false;

                string Item = value.Substring(i, (i * -1) + (i = (value.IndexOf(" ", i) + 1))).Replace(" ", "");
                if ((Exit = value.IndexOf(" ", i) <= 0))
                    Insert(Item, value.Substring(i).Replace(" ", ""), i, StartingWords);
                else
                    Insert(Item, value.Substring(i, (i * -1) + ((value.IndexOf(" ", i) + 1))).Replace(" ", ""), i, StartingWords);

                
                i = 0;
                do
                {

                    Item = value.Substring(i, (i * -1) + (i = (value.IndexOf(" ", i) + 1))).Replace(" ", "");
                    if ((Exit = value.IndexOf(" ", i) <= 0))
                    {
                        Insert(Item, value.Substring(i).Replace(" ", ""), i, Words);
                        Insert(Item, value.Substring(i).Replace(" ", ""), i, EndingWords);
                    }
                    else
                        Insert(Item, value.Substring(i, (i * -1) + ((value.IndexOf(" ", i) + 1))).Replace(" ", ""), i, Words);

                } while (!Exit);
            }
        }
        public Dictionary<string, PostandNumber> StartingWords { get; set; }

        public List<string> EntirePost { get; set; }

        public Dictionary<string, PostandNumber> Words { get; set; }

        public Dictionary<string, PostandNumber> EndingWords { get; set; }
    }
    public class PostandNumber
    {
        public PostandNumber()
        {
            FollowWords = new Dictionary<string, int>();
            TimesUsed = 0;
        }

        public string Add
        {
            set
            {
                FollowWords.Add(value, 1);
                TimesUsed++;
            }
        }
        public int TimesUsed { get; set; }

        public Dictionary<string, int> FollowWords { get; set; }

    }

    public class BigList
    {
        public BigList()
        {
            Pages = new List<string>();
            Words = new List<string>();
        }

        public List<string> Pages { get; set; }
        public List<string> Words { get; set; }
    }
}
