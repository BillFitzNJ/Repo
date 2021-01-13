using System.Data;
using System;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using WSGUtilitieslib;
using System.Configuration;
using System.Drawing;
using System.Data.SqlClient;

namespace BusinessTransactions
{
  public class ExtractBankFiles
  {
    public FrmExtractBankFiles parentForm = new FrmExtractBankFiles();
    WSGUtilities wsgUtilities = new WSGUtilities("Maintain Event");
    public ExtractBankFiles()
    {
      SetEvents();
      parentForm.dateTimePickerProcessDate.Value = DateTime.Now;
      parentForm.ShowDialog();

    }
    public void SetEvents()
    {
      parentForm.buttonReturn.Click += new System.EventHandler(CloseparentForm);
      parentForm.buttonGetFiles.Click += new System.EventHandler(GetFiles);

    }
    public void CloseparentForm(object sender, EventArgs e)
    {
      parentForm.Close();
    }

    public void GetFiles(object sender, EventArgs e)
    {
    
      FolderBrowserDialog fd = new FolderBrowserDialog();
      fd.ShowNewFolderButton = false;
      fd.Description = "Select Destination for files to be copied";
      DialogResult fdResult = fd.ShowDialog();

      if (fdResult == DialogResult.OK)
      {
        string destnationPathName = fd.SelectedPath;
        string ssFileName = "ss" + String.Format("{0:MMddyy}", parentForm.dateTimePickerProcessDate.Value) + ".txt";
        string psFileName = "ps" + String.Format("{0:MMddyy}", parentForm.dateTimePickerProcessDate.Value) + ".pdf";
        string trFileName = "tr" + String.Format("{0:MMddyy}", parentForm.dateTimePickerProcessDate.Value) + ".pdf";
        string ssSourcePath = ConfigurationManager.AppSettings["NHPBankPath"] + "sheets\\";
        string pdfSourcePath = ConfigurationManager.AppSettings["NHPBankPath"] + "pdffiles\\";
        string ssSourceFile = System.IO.Path.Combine(ssSourcePath,ssFileName);
        
        // Copy bank text file ssMMDDYY.txt
        if (System.IO.File.Exists(ssSourceFile))
         {
          string ssdestFile = System.IO.Path.Combine(destnationPathName, ssFileName);
          System.IO.File.Copy(ssSourceFile, ssdestFile, true);
         wsgUtilities.wsgNotice("SS bank text file copied");
         }
         else
         {
           wsgUtilities.wsgNotice("There is no SS extract file for that date.");
         }
        // Copy proof sheet file psMMDDYY.pdf
         string psSourceFile  = System.IO.Path.Combine(pdfSourcePath,psFileName);
         if (System.IO.File.Exists(psSourceFile))
         {
           string psdestFile = System.IO.Path.Combine(destnationPathName, psFileName);
           System.IO.File.Copy(psSourceFile, psdestFile, true);
           wsgUtilities.wsgNotice("Proof Sheet file copied");
         }
         else
         {
           wsgUtilities.wsgNotice("There is no Proof Sheet file for that date.");
         }
         // Copy proof sheet file psMMDDYY.pdf
         string trSourceFile = System.IO.Path.Combine(pdfSourcePath, trFileName);
         if (System.IO.File.Exists(trSourceFile))
         {
           string trdestFile = System.IO.Path.Combine(destnationPathName, trFileName);
           System.IO.File.Copy(trSourceFile, trdestFile, true);
           wsgUtilities.wsgNotice("Transmission PDF file copied");
         }
         else
         {
           wsgUtilities.wsgNotice("There is no Transmission PDF file for that date.");
         }
       }
     else
     {
       wsgUtilities.wsgNotice("Extract Cancelled");
     }

    }
  }


}
