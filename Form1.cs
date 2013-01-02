using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;
using System.Net;

namespace GathererImport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void cmdGrabPrices_Click(object sender, EventArgs e)
        {
            gather();
        }

        private void gather()
        {

            StringBuilder jsonItems = new StringBuilder();
            
            StringBuilder jsonResults = new StringBuilder();
            jsonResults.Append("[");
            //Split the list of sets into an array.
            string[] setArray = txtSets.Text.Split('\n');

            //Get Gatherer cards and turn them into JSON

            txtError.Text += "Receiving cards from Gatherer..." + "\r\n";

            //Loop through the array of sets.
            foreach (string set in setArray)
            {
                jsonItems = new StringBuilder();

                string[] setOptions = set.Split('|');

                txtError.Text += "Receiving " + setOptions[0] + "..." + "\r\n";

                try
                {
                    //Web Request to Gatherer's Checklist. Unfortunately, this checklist 
                    //does not display all card information. However, it's the 
                    //easiest to pull from. 
                    System.Net.WebRequest webReq = System.Net.WebRequest.Create("http://gatherer.wizards.com/Pages/Search/Default.aspx?output=checklist&action=advanced&set=%5b%22" + setOptions[0].Replace(" ", "+") + "%22%5d");
                    System.Net.WebResponse webRes = webReq.GetResponse();
                    System.IO.Stream mystream = webRes.GetResponseStream();
                    if (mystream != null)
                    {
                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.Load(mystream);

                        jsonItems.Append("{\"items\": {");

                        //Loop through each card and pull information.
                        foreach (HtmlNode parentNode in doc.DocumentNode.SelectNodes("//table")[0].SelectNodes("tr[@class='cardItem']"))
                        {
                            try
                            {
                                string cardParent = "Magic: The Gathering";
                                string cardSchemaType = "card";
                                string cardTitle = parentNode.SelectNodes("td[@class='name']")[0].InnerText.ToString();
                                string cardExpansionAbreviation = setOptions[1].Trim();
                                string cardManaCost = "";
                                string cardText = "";
                                string cardFlavorText = "";
                                string cardPT = "";
                                string cardRarity = getRarity(parentNode.SelectNodes("td[@class='rarity']")[0].InnerText.ToString());
                                string cardNumber = parentNode.SelectNodes("td[@class='number']")[0].InnerText.ToString();
                                string cardArtist = parentNode.SelectNodes("td[@class='artist']")[0].InnerText.ToString();
                                string cardFoil = "";

                                string cardSoftID = cardParent + "|" + cardExpansionAbreviation + "|" + cardTitle + "|" + cardFoil;

                                //Use the jsonItem Schema to create a new card item.
                                jsonItems.Append(string.Format(jsonItem
                                    , cardSoftID
                                    , cardParent
                                    , cardSchemaType
                                    , cardTitle
                                    , cardExpansionAbreviation
                                    , cardManaCost
                                    , ""
                                    , cardText
                                    , cardFlavorText
                                    , cardPT
                                    , cardRarity
                                    , cardNumber
                                    , cardArtist
                                    , cardFoil));
                                
                                
                            }
                            catch (Exception ex)
                            {
                                //Generate an error if a card does not come through correctly.
                                //txtError.Text += "ERROR WITH RECORDING PRICE\r\n";
                            }
                        }

                        jsonItems.Append("}}");
                        jsonResults.AppendLine(",");
                        jsonResults.Append(jsonItems);
                        ///////////////////////////////////////////////////
                        ///////////////////////////////////////////////////
                        //DO SOMETHING WITH JSON. USE YOUR IMAGINATION!!!//
                        ///////////////////////////////////////////////////
                        ///////////////////////////////////////////////////


//                                               ,---.
//                                              /    |
//                                             /     |
//                                            /      |
//                                           /       |
//                                      ___,'        |
//                                    <  -'          :
//                                     `-.__..--'``-,_\_
//                                        |o/ <o>` :,.)_`>
//                                        :/ `     ||/)
//                                        (_.).__,-` |\
//                                        /( `.``   `| :
//                                        \'`-.)  `  ; ;
//                                        | `       /-<
//                                        |     `  /   `.
//                        ,-_-..____     /|  `    :__..-'\
//                       /,'-.__\\  ``-./ :`      ;       \
//                       `\ `\  `\\  \ :  (   `  /  ,   `. \
//                         \` \   \\   |  | `   :  :     .\ \
//                          \ `\_  ))  :  ;     |  |      ): :
//                         (`-.-'\ ||  |\ \   ` ;  ;       | |
//                          \-_   `;;._   ( `  /  /_       | |
//                           `-.-.// ,'`-._\__/_,'         ; |
//                              \:: :     /     `     ,   /  |
//                               || |    (        ,' /   /   |
//                               ||                ,'   /    |


                    }
                }
                catch (Exception ex)
                {
                    txtError.Text += "ERROR WITH RECORDING SET: " + setOptions[0] + "\r\n";
                }
            }
        }

        private string getRarity(string rareIn)
        {
            switch (rareIn)
            {
                case "M":
                    return "Mythic Rare";
                case "R":
                    return "Rare";
                case "U":
                    return "Uncommon";
                case "C":
                    return "Common";
                default:
                    return "";
            }
        }
        private string jsonItem = @"
		        ""item"": {{
			        ""title"": ""{0}"",
			        ""parent"": ""{1}"",
			        ""schemaType"": ""{2}"",
			        ""details"": {{
					        ""title"": ""{3}"",
					        ""expansionAbreviation"": ""{4}"",
					        ""manaCost"": ""{5}"",
					        ""convertedManaCost"": ""{6}"",
					        ""cardText"": ""{7}"",
					        ""flavorText"": ""{8}"",
					        ""pt"": ""{9}"",
					        ""rarity"": ""{10}"",
					        ""cardNumber"": ""{11}"",
					        ""artist"": ""{12}"",
					        ""foil"": ""{13}""
				        }}
		        }},";

        private void gotoGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/nickcdavis/GathererImport");
        }
    }
}
