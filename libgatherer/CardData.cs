namespace GathererImport {
    public class CardData {
        //public string cardParent = "Magic: The Gathering";
        public string multiverseID = "";
        public string cardSchemaType = "card";
        public string cardTitle = ""; //parentNode.SelectNodes("td[@class='name']")[0].InnerText.Topublic string();
        public string cardExpansion = ""; //setOptions[1].Trim();
        public string cardManaCost = "";
        public string cardText = "";
        public string cardFlavorText = "";
        public string cardPT = "";
        public string cardRarity = ""; //getRarity(parentNode.SelectNodes("td[@class='rarity']")[0].InnerText.Topublic string());
        public string cardNumber = ""; //parentNode.SelectNodes("td[@class='number']")[0].InnerText.Topublic string();
        public string cardArtist = ""; //parentNode.SelectNodes("td[@class='artist']")[0].InnerText.Topublic string();
        public string cardFoil = "";
        public string cardSoftID = ""; //cardParent + "|" + cardExpansionAbreviation + "|" + cardTitle + "|" + cardFoil;
        public System.Collections.Generic.List<string> cardTypes = new System.Collections.Generic.List<string>();

        public CardData() { }
        public CardData(string multiverseID)
        {
            this.multiverseID = multiverseID;
        }
        public static CardData gather(string multiverseId) { return CardData.gather(int.Parse(multiverseId)); }
        public static CardData gather(int multiverseId)
        {
            string url = string.Format("http://gatherer.wizards.com/Pages/Card/Details.aspx?printed=true&multiverseid={0}", multiverseId);
            System.Net.WebClient wc = new System.Net.WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(wc.DownloadString(url));

            CardData result = new CardData(multiverseId.ToString());

            try
            {
                result.cardTitle = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_nameRow']/div[@class='value']").InnerText.Trim();
            } catch{}
            try
            {
                result.cardExpansion = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_setRow']/div[@class='value']").InnerText.Trim();
            }
            catch { }
            try
            {
                result.cardManaCost = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_cmcRow']/div[@class='value']").InnerText.Trim();
            }
            catch { }
            try{ 
                result.cardText = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_textRow']/div[@class='value']/div").InnerText.Trim();
            } catch{}
            try
            {
                result.cardFlavorText = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_flavorRow']/div[@class='value']").InnerText.Trim();
            } catch { }
            try
            {
                result.cardPT = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_ptRow']/div[@class='value']").InnerText.Trim();
            }
            catch { }
            try
            {
                result.cardRarity = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_rarityRow']/div[@class='value']").InnerText.Trim();
            }
            catch { }
            try
            {
                result.cardNumber = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_numberRow']/div[@class='value']").InnerText.Trim();
            }
            catch { }
            try
            {
                result.cardArtist = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_artistRow']/div[@class='value']").InnerText.Trim();
            }
            catch { }
            try
            {
                result.cardFoil = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_ArtistCredit']/div[@class='value']").InnerText.Trim();
            }
            catch { }
            try
            {
                result.cardTypes.AddRange(doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ctl00_ctl00_MainContent_SubContent_SubContent_typeRow']/div[@class='value']").InnerText.Trim().Split('—'));
                for (int i = 0; i < result.cardTypes.Count; i++)
                {
                    result.cardTypes[i] = result.cardTypes[i].Trim();
                }
            }
            catch { }

            return result;
        }
    }
}