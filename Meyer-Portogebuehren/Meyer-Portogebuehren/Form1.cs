using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HidLibrary;
using Scale;
using System.Threading;
using System.Xml.Linq;
using System.Collections;
using System.Linq;
using System.Configuration;

namespace Meyer_Portogebuehren
{
    public partial class frmMain : Form
    {
        private bool programmRunning = true;
        private int oldweight = -1;
        private int labelnumber = 0;
        List<Briefporto> portos = new List<Briefporto>();
       
        /// <summary>
        /// Author: Andreas Kestler
        /// Company: Maschinenfabrik Herbert Meyer GmbH
        /// Todo: Errorhandling on xml-errors and errors in general
        /// </summary>

        public frmMain()
        {
            InitializeComponent();

            XElement xelement = XElement.Load("portotab.xml");
            IEnumerable<XElement> briefporto = xelement.Elements();
            // Read the entire XML
            int i = 0;
            
            foreach (XElement briefart in briefporto)
            {
                // GENERATE BRIEFART

                //Create label
                i = i + 1;
                Label label = new Label();
                //Make label identifable
                label.Text = String.Format(briefart.Element("Name").Value);
                label.Location = new System.Drawing.Point(20, 30 + (i * 65));
                label.Font = new Font("Arial", 9, FontStyle.Bold);
                label.Size = new Size(85, 50);
                label.TextAlign = ContentAlignment.MiddleLeft;

                //Draw seperator line
                Label labelSeperator = new Label();
                labelSeperator.AutoSize = false;
                labelSeperator.Height = 2;
                labelSeperator.Width = 1200;
                labelSeperator.BorderStyle = BorderStyle.Fixed3D;
                labelSeperator.Location = new System.Drawing.Point(20, 87 + (i * 65));

                this.Controls.Add(labelSeperator);
                this.Controls.Add(label);


                //GENERATE BLINKLABELS

                int g = 0;
                IEnumerable<XElement> porto = briefart.Descendants("porto");
                foreach (XElement item in porto)
                {
                    //Create label
                    g = g + 1;
                    labelnumber = labelnumber + 1;
                    Label labelporto = new Label();
                    labelporto.Name = "Portolabel" + labelnumber.ToString();
                    labelporto.Text = String.Format(item.Element("gewichtvon").Value + " - " + item.Element("gewichtbis").Value + " " + ConfigurationSettings.AppSettings["weightunit"] + Environment.NewLine + Environment.NewLine + item.Element("preis").Value);
                    labelporto.Location = new System.Drawing.Point(20 + (g * 88), 30 + (i * 65));
                    labelporto.Size = new Size(85, 50);
                    labelporto.AutoSize = false;
                    labelporto.TextAlign = ContentAlignment.MiddleCenter;
                    //labeldesign
                    labelporto.Font = new Font("Arial", 9, FontStyle.Bold);
                    labelporto.BackColor = Color.White;

                    //add to portolist
                    portos.Add(new Briefporto { ID = labelnumber, WeightMin = Convert.ToInt16(item.Element("gewichtvon").Value), WeightMax = Convert.ToInt16(item.Element("gewichtbis").Value) });
                    
                    this.Controls.Add(labelporto);
                }
            }

            //Start Background thread for getting the weight and updating UI
            Thread backgroundThread = new Thread(BackgroundWork);
            backgroundThread.Start();

        }//END:frmMain

        private void BackgroundWork()
        {
            while (programmRunning == true)
            {
                decimal? weightInLb, weightInG, weightInOz;
                bool? isStable;

                USBScale s = new USBScale();
                s.Connect();
                int weightinInt = 0;
                if (s.IsConnected)
                {
                    s.GetWeight(out weightInLb, out weightInG, out weightInOz, out isStable);
                    //s.DebugScaleData();
                    if(ConfigurationSettings.AppSettings["weightunit"] == "g")
                        weightinInt = Convert.ToInt32(weightInG);
                    else if(ConfigurationSettings.AppSettings["weightunit"] == "oz")
                        weightinInt = Convert.ToInt32(weightInOz);
                    else if (ConfigurationSettings.AppSettings["weightunit"] == "lb")
                        weightinInt = Convert.ToInt32(weightInLb);

                    DoWorkOnUI(weightinInt);
                    s.Disconnect();
                } else
                {
                    DoWorkOnUI(0);
                }
                Thread.Sleep(1000);
            } 
        }//END:BackgroundWork

        private void DoWorkOnUI(int weight)
        {
            MethodInvoker methodInvokerDelegate = delegate ()
            {
            //Skip Updating the Labels, if value didnt change
            if (weight == oldweight)
                return;
            oldweight = weight;

                //Show weight (in green text on top)
                lblWeight.Text = Convert.ToString(weight);
                foreach (var porto in portos)
                {
                    var labelporto = Controls.Find("Portolabel" + Convert.ToString(porto.ID), true).FirstOrDefault() as Label;
                    if (labelporto != null)
                    {
                        if (weight >= porto.WeightMin && weight <= porto.WeightMax)
                        {
                            if (null != labelporto && labelporto is Label)
                            {
                                (labelporto as Label).BackColor = System.Drawing.Color.Lime;
                            }
                        }
                        else
                        {
                            if (null != labelporto && labelporto is Label)
                            {
                                (labelporto as Label).BackColor = System.Drawing.Color.White;
                            }
                        }
                    }
                }
            };

            //Invoke(methodInvokerDelegate);
            //This will be true if Current thread is not UI thread.
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(methodInvokerDelegate);
                }
                catch
                { 
                    //Catch Dispose on closing main form, ugly way 
                }
            }
                
        }//END:DoWorkOnUI

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            programmRunning = false;
        }
        private void Button2_Click_1(object sender, EventArgs e)
        {

        }
    }
    class Briefporto
    {
        public int ID { get; set; }
        public int WeightMin { get; set; }
        public int WeightMax { get; set; }
    }
}
