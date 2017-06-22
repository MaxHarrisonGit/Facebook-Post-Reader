using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextDecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            load();
        }
        JobjectPad JObjectLoad = new JobjectPad();
        APIpossible API = new APIpossible();
        XMLConverter XML = new XMLConverter();
        string Location = "Z:\\StringData\\TotalGroup";
        //private TotalGroup XMLFile = new TotalGroup() { Words = new List<KeyValuePair<string, DictionResonse>>() };
        //private TotalGroupMain FinalFile = new TotalGroupMain() { Words = new Dictionary<string, DictionResonse>() };

        private void load()
        {
            if (!File.Exists(Location))
            {
                JObjectLoad.Set(Location, new PostandWords());
            }
            else
            {
                Data = ((PostandWords)JObjectLoad.Get(Location, new PostandWords()));
            }
            ListData = Data.Lists;
            //Result();
        }



        protected PostandWords Data = new PostandWords();
        protected BigList ListData = new BigList();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (string Page in ListData.Pages)
            {
                foreach (string Post in API.readFB(Page))
                    Data.Item = Post;


            }

            foreach(string Item in ListData.Words)
                Remove(Item);

            Result();
        }
        private void Remove(string word)
        {

            if (Data.Words.ContainsKey(word))
                Data.Words.Remove(word);
            if (Data.StartingWords.ContainsKey(word))
                Data.StartingWords.Remove(word);
            if (Data.EndingWords.ContainsKey(word))
                Data.EndingWords.Remove(word);

            foreach (var LItem in Data.Words)
                if (LItem.Value.FollowWords.ContainsKey(word))
                    LItem.Value.FollowWords.Remove(word);

            foreach (var LItem in Data.StartingWords)
                if (LItem.Value.FollowWords.ContainsKey(word))
                    LItem.Value.FollowWords.Remove(word);

            foreach (var LItem in Data.EndingWords)
                if (LItem.Value.FollowWords.ContainsKey(word))
                    LItem.Value.FollowWords.Remove(word);

        }

        protected string WordTest = "";
        protected int Times = 0;
        protected int MinWord = 25;

        private void CreateMostPopularString()
        {
            string LastWord = "";
            string PrePreWord = "";
            string PreWord = "";
            bool Finish = false;
            WordTest = (LastWord = FindMostMain(Data.StartingWords));
            do
            {
                if (!Data.Words.ContainsKey(LastWord))
                {
                    ListData.Words.Add(LastWord);
                    Remove(LastWord);
                    WordTest = WordTest.Substring(0, WordTest.IndexOf(LastWord) - 1);
                    ListData.Words.Add(LastWord);
                    LastWord = PreWord;
                    Result();
                }
                else
                {
                    WordTest += " " + (LastWord = FindMostFollow(Data.Words[LastWord].FollowWords,PreWord,PrePreWord));
                    Times++;
                    PrePreWord = PreWord;
                    PreWord = LastWord;

                }
                if (Times > MinWord && Data.EndingWords.ContainsKey(LastWord))
                    Finish = true;
            } while (!Finish);


            //simple
            Random rnd = new Random();
            int RandomNumber = 0;
            Finish = false;
            Times = 0;
            WordTest = (LastWord = FindMostMain(Data.StartingWords));
            do
            {
                if (!Data.Words.ContainsKey(LastWord))
                {
                    Remove(LastWord);
                    ListData.Words.Add(LastWord);
                    Result();
                    WordTest = WordTest.Substring(0, WordTest.IndexOf(LastWord) - 1);
                    WordTest += " " + (LastWord = Data.Words[PreWord].FollowWords.Last(o => o.Key != LastWord).Key);

                    if (Data.Words.ContainsKey(LastWord))
                        PreWord = LastWord;

                }
                else
                {
                    RandomNumber = rnd.Next(0, Data.Words[LastWord].FollowWords.Count - 1);
                    int IntCount = 0;
                    do
                    {
                        if (Data.Words.ContainsKey((Data.Words[LastWord].FollowWords.Keys.ElementAt(RandomNumber))))
                        {
                            if ((IntCount = Data.Words[(Data.Words[LastWord].FollowWords.Keys.ElementAt(RandomNumber))].FollowWords.Count) == 0)
                                RandomNumber = rnd.Next(0, Data.Words[LastWord].FollowWords.Count - 1);
                        }
                        else
                            RandomNumber = rnd.Next(0, Data.Words[LastWord].FollowWords.Count - 1);

                    } while (IntCount < 0);

                    PreWord = LastWord;
                    WordTest += " " + (LastWord = Data.Words[LastWord].FollowWords.Keys.ElementAt(RandomNumber));
                    
                }
                Times++;
                if (Times > MinWord && Data.EndingWords.ContainsKey(LastWord))
                    Finish = true;
            } while (!Finish);
            //test

        }


        private string FindMostFollow(Dictionary<string, int> List, string PreWord, string PrePreWord)
        {
            string Stringreturn = "";
            int Ammount = 0;
            foreach (var Item in List)
                if (Item.Value > Ammount)
                {
                    if (WordTest.IndexOf(/*PrePreWord + " " + */PreWord + " " + Item.Key) <= 0)
                    {
                        if (Data.Words.ContainsKey(Item.Key))
                        {
                            if (Data.Words[Item.Key].FollowWords.Count != 0)
                            {
                                Ammount = Item.Value;
                                Stringreturn = Item.Key;
                            }
                            else if (Times > MinWord)
                            {
                                Ammount = Item.Value;
                                Stringreturn = Item.Key;
                            }
                        }
                    }
                }

            return Stringreturn;
        }

        private string FindMostMain(Dictionary<string, PostandNumber> List)
        {
            string Stringreturn = "";
            int Ammount = 0;
            foreach (var Item in List)
                if (Item.Value.TimesUsed > Ammount)
                {
                    Ammount = Item.Value.TimesUsed;
                    Stringreturn = Item.Key;
                }
            return Stringreturn;
        }

        private void Result()
        {

            JObjectLoad.Set(Location, Data);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (PageName.Text != "")
            {
                ListData.Pages.Add(PageName.Text);
                Result();
            }
            //this

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Result();
            CreateMostPopularString();
        }
    }
}
