using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace GathererImport {
    public class Gatherer {
        public List<string> gatherSets() {
            List<string> cardSets = new List<string>();
            try
            {
                //http://gatherer.wizards.com/Pages/Default.aspx
                //look for id=ctl00_ctl00_MainContent_Content_SearchControls_setAddText
                System.Net.WebRequest webReq = System.Net.WebRequest.Create("http://gatherer.wizards.com/Pages/Default.aspx");
                System.Net.WebResponse webRes = webReq.GetResponse();
                System.IO.Stream mystream = webRes.GetResponseStream();
                if (mystream != null)
                {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.Load(mystream);
                    foreach (HtmlNode cardSet in doc.DocumentNode.SelectNodes("//select[@id='ctl00_ctl00_MainContent_Content_SearchControls_setAddText']/option"))
                    {
                        if (cardSet.Attributes["value"].Value.Length > 0)
                        {
                            cardSets.Add(cardSet.Attributes["value"].Value);
                        }
                    }

                    return cardSets;
                }
            }
            catch (Exception e) { throw new Exception("something went wrong:", e); }
            return cardSets; 
        }


        public SetMeta gatherSetMeta(string Set = "")
        {
            //messy messy
            SetMeta result = new SetMeta();
            Gatherer g = new Gatherer();
            g.onGatherCardProgressEvent += new GatherCardProgressEvent(delegate(CardData cd, int position, int total)
            {
                result.totalCards = total;
                result.cardsPerPage = position + 1;
            });
            g.gatherCardMultiverseIds(0, 0, Set);
            return result;
        }

        /// <summary>
        /// Basically a ready only thing callback for the fetching progress...works with Powershell!
        /// </summary>
        public event GatherCardProgressEvent onGatherCardProgressEvent;
        
        /// <summary>
        /// Basically a ready only thing callback for the fetching progress...works with Powershell!
        /// </summary>
        /// <param name="Card"></param>
        /// <param name="index"></param>
        /// <param name="totalCards"></param>
        public delegate void GatherCardProgressEvent(CardData Card, int index, int totalCards);

        /// <summary>
        /// Basically a ready only thing callback for the fetching progress...works with Powershell!
        /// </summary>
        /// <remarks>
        /// Fires when all loopy activity is complete whether or not the fetch was cancelled or errored out.
        /// </remarks>
        public event GatherCardCompleteEvent onGatherCardCompleteEvent;
        
        /// <summary>
        /// Basically a ready only thing callback for when fetching is finished...works with Powershell!
        /// </summary>
        /// <remarks>
        /// Fires when all loopy activity is complete whether or not the fetch was cancelled or errored out.
        /// </remarks>
        public delegate void GatherCardCompleteEvent();

        /// <summary>
        /// An Errors has happened, we'll let you know so you can guess what and where....works with Powershell!
        /// </summary>
        /// <returns></returns>
        public event GatherCardErrorEvent onGatherCardErrorEvent;
        public delegate void GatherCardErrorEvent(Exception Error);

        /// <summary>
        /// An Errors has happened, we'll let you know so you can guess what and where.
        /// </summary>
        /// <returns></returns>
        public GatherCardError onGatherCardError;
        public delegate bool GatherCardError(Exception Error);

        /// <summary>
        /// Find out what we're up to in fetch land. If you return false, we stop looking at more pages, but will still continue processing the current page.
        /// </summary>
        /// <param name="Card"></param>
        /// <param name="index"></param>
        /// <param name="totalCards"></param>
        /// <returns></returns>
        public GatherCardProgress onGatherCardProgress;
        public delegate bool GatherCardProgress(CardData Card, int index, int totalCards);

        public List<int> gatherCardMultiverseIds(int startPage = 0, int endPage = -1, string Set = "")
        {
            //TODO: add a way to stop the madness. (stop a really long capture.) Semi done...use the asyn method
            
            bool fetch = true;
            List<int> results = new List<int>();
            int cur_page = startPage;
            int last_page = endPage;
            int total_cards = -1;

            for (cur_page = startPage; cur_page <= ((last_page == -1) ? cur_page : last_page) && fetch; cur_page++)
            {
                System.UriBuilder u = new UriBuilder(
                    string.Format("http://gatherer.wizards.com/Pages/Search/Default.aspx?page={0}&output=compact&action=advanced&name=+%5bm%2f.+%2f%5d", cur_page));
                if (Set != "")
                {
                    u.Query = u.Query.Substring(1) + string.Format("&set=+[\"{0}\"]", Set.Replace(" ", "+"));
                }

                try
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    wc.Encoding = System.Text.Encoding.UTF8;
                    string mystream = wc.DownloadString(u.Uri);
                    if (mystream != null)
                    {
                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        try
                        {
                            doc.LoadHtml(mystream);
                        }
                        catch (Exception e) { throw new Exception("can't parse page for some odd reason", e); }
                        if(total_cards == -1)
                        {
                            try
                            {
                                string tc = doc.DocumentNode.SelectSingleNode("//span[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContentHeader_searchTermDisplay']").InnerText;
                                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(tc, "([0-9]+)");
                                if (m.Groups.Count == 2)
                                {
                                    total_cards = int.Parse(m.Groups[1].Value);
                                    if (last_page == -1) last_page = (int)System.Math.Ceiling((double)total_cards / doc.DocumentNode.SelectNodes("//div[@class='cardList']/table/tr").Count - 1);
                                }
                            }
                            catch (Exception e) { throw new Exception("Can't find the total number of cards", e); }
                        }

                        if (last_page == -1)
                        {
                            try
                            {
                                string lp = doc.DocumentNode.SelectSingleNode("//*[@id='ctl00_ctl00_ctl00_MainContent_SubContent_topPagingControlsContainer']/a[last()]").Attributes["href"].Value;
                                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(lp, "page=([0-9]+)\\&");
                                if (m.Groups.Count == 2)
                                {
                                    last_page = int.Parse(m.Groups[1].Value);
                                }
                            }
                            catch (Exception ex) { throw new Exception("can't parse the last page.", ex); }
                        }
                        if (doc.DocumentNode.SelectNodes("//div[@class='cardList']//a").Count > 0)
                        {
                            foreach (HtmlNode card in doc.DocumentNode.SelectNodes("//div[@class='cardList']//a")) //"//div[@class = 'cardList']//td[contains(@class, 'cardItem')]/a"))
                            {
                                try
                                {
                                    if (card.Attributes["href"].Value.ToLower().StartsWith("../card/details.aspx")
                                        && card.Attributes["href"].Value.ToLower().Contains("?multiverseid")
                                        && (card.ParentNode.Name.ToLower().Equals("td") && card.ParentNode.Attributes["class"].Value.ToLower().Contains("name")))
                                    {
                                        string cardUrl = string.Format("http://gatherer.wizards.com/Pages/{0}", card.Attributes["href"].Value.Substring(3));
                                        Match m = Regex.Match(cardUrl, "\\?multiverseid=([0-9]+)", RegexOptions.IgnoreCase);

                                        CardData carddata = (m.Groups.Count == 2) ? new CardData(m.Groups[1].Value) : new CardData();
                                        results.Add(int.Parse(carddata.multiverseID));

                                        if (this.onGatherCardProgress != null)
                                            fetch = this.onGatherCardProgress.Invoke(carddata, results.Count-1, total_cards);
                                        if (this.onGatherCardProgressEvent != null)
                                            this.onGatherCardProgressEvent.Invoke(carddata, results.Count-1, total_cards);
                                    }
                                }
                                catch (Exception e)
                                {
                                    if (this.onGatherCardError != null) this.onGatherCardError.Invoke(e);
                                    if (this.onGatherCardErrorEvent != null) this.onGatherCardErrorEvent.Invoke(e);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    if (this.onGatherCardError != null) this.onGatherCardError.Invoke(e);
                    if (this.onGatherCardErrorEvent != null) this.onGatherCardErrorEvent.Invoke(e);
                }
            }

            if (this.onGatherCardCompleteEvent != null) this.onGatherCardCompleteEvent.Invoke();

            
            
            return results;
        }

        //This is probably waaaayyyy too messy to actually use.
        /// <summary>
        /// Get your card ids asynchronously. 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public IAsyncResult gatherCardMultiverseIdsAsync(AsyncCallback callback, int start = 0, int end = -1, string set = "")
        {
            Func< int, int, string,List<int>> df = delegate(int start2, int end2, string set2)
            {
                return this.gatherCardMultiverseIds(start2, end2, set2);
            };
            ((Action)delegate() { }).BeginInvoke(new AsyncCallback(delegate(IAsyncResult ar) { Console.WriteLine("finished other"); }), new object());
            return df.BeginInvoke(start, end, set, callback, new object());
            
        }

    }
}