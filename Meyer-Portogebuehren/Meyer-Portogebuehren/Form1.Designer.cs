using System;
using System.Windows.Forms;

namespace Meyer_Portogebuehren
{
    partial class frmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblWeight = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblWeight
            // 
            this.lblWeight.BackColor = System.Drawing.Color.Black;
            this.lblWeight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 42F);
            this.lblWeight.ForeColor = System.Drawing.Color.Lime;
            this.lblWeight.Location = new System.Drawing.Point(40, 31);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(564, 118);
            this.lblWeight.TabIndex = 0;
            this.lblWeight.Text = "0";
            this.lblWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1651, 1078);
            this.Controls.Add(this.lblWeight);
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.Text = "Meyer-Portogebühren - Maschinenfabrik Herbert Meyer GmbH";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblWeight;
    }
}

