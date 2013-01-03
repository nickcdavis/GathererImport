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
        GathererImport.Gatherer g = new Gatherer();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Action bgload = delegate()
            {
                
                this.checkedListBox1.Invoke((Action)delegate()
                {
                    this.checkedListBox1.Items.AddRange(g.gatherSets().ToArray());
                });
            };
            bgload.Invoke();
        }
        private void cmdGrabPrices_Click(object sender, EventArgs e)
        {
            ((Action) delegate()
            {
                Gatherer g2 = new Gatherer();
                g2.onGatherCardProgress += new Gatherer.GatherCardProgress(g2_onGatherCardProgressEvent);
                g2.onGatherCardCompleteEvent += new Gatherer.GatherCardCompleteEvent(g2_onGatherCardCompleteEvent);
                List<string> sets = new List<string>();
                if (this.checkedListBox1.CheckedItems.Count > 0)
                {
                    foreach (string set in this.checkedListBox1.CheckedItems)
                    {
                        SetMeta sm = g2.gatherSetMeta(set);
                        this.Invoke((Action)delegate()
                        {
                            this.progressBar1.Value = 0;
                            this.progressBar1.Maximum = sm.totalCards;
                        });
                        g2.gatherCardMultiverseIds(Set: set);
                    }
                }
            }).BeginInvoke(new AsyncCallback(delegate(IAsyncResult ar) { }), new object());
            
        }

        void g2_onGatherCardCompleteEvent()
        {
            this.Invoke((Action)delegate()
            {
                this.progressBar1.Value = this.progressBar1.Maximum;
            });
        }

        bool g2_onGatherCardProgressEvent(CardData Card, int index, int totalCards)
        {
            ((Action)delegate()
            {
                Card = CardData.gather(Card.multiverseID);
                this.Invoke((Action)delegate()
                {
                    System.Web.Script.Serialization.JavaScriptSerializer j = new System.Web.Script.Serialization.JavaScriptSerializer();

                    this.txtError.Text += j.Serialize(Card) + Environment.NewLine;
                    this.progressBar1.PerformStep();
                });
            }).BeginInvoke(new AsyncCallback((Action<IAsyncResult>)delegate(IAsyncResult ar) { }), new object());
            return true;
        }

        private void gotoGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/nickcdavis/GathererImport");
        }
    }
}
