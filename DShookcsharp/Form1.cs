using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Binarysharp;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;
using Binarysharp.MemoryManagement.Helpers;
using Binarysharp.MemoryManagement.Native;
using Binarysharp.MemoryManagement.Assembly;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using InputManager;
namespace DShookcsharp
{

    public partial class mainWindow : Form
    {

        MemorySharp MemSharp;
        IntPtr pItemAddr;
        IntPtr caveBase;
        int eaxAddress;
        string[] originalCode = { "mov cl,[eax+22]", "test cl,cl" };
        BackgroundWorker main = new BackgroundWorker();
        static mainWindow mainWin;
        static public List<ItemId> allItems = new List<ItemId>();
        int pItemBaseVal;

        //HIGHLIGHTED ITEM OFFSETS
        int id1Offset = 12; //4 bytes
        int id2Offset = 16; //4 bytes
        int typeOffset = 22; //2 bytes
        int quantityOffset = 24; //4 bytes
        int durabilityOffset = 24; //float
        int damagedOffset = 28; //byte
        int levelOffset = 29; //byte
        int infusionOffset = 30; //byte

        ///DELEGATE DEFS
        ///
        public void addItemToDropDownBox(ItemId itemIn)
        {
            if (ItemInputBox.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemInputBox.Items.Add(itemIn.name);
                }));

            }
        }
        public string getDropDownSelection()
        {
            string itemName = "";
            if (ItemInputBox.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    itemName = ItemInputBox.SelectedItem.ToString();
                }));
                return itemName;
            }
            else
            {
                return ItemInputBox.SelectedItem.ToString();
            }
            
        }
        public void setCaveBase(string value)
        {
            if (CodeCaveAddressLabel.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    CodeCaveAddressLabel.Text = value;
                }));
                
            }
        }
        public void setBase(string value)
        {
            
            if (ItemBaseLabel.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemBaseLabel.Text = value;
                }));

            }           
        }
        public void setItemId1(string value)
        {

            if (ItemId1Label.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemId1Label.Text = value;
                }));

            }
        }
        public void setItemId2(string value)
        {

            if (ItemId2Label.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemId2Label.Text = value;
                }));

            }
        }
        public void setItemTypeLabel(string value)
        {

            if (ItemTypeLabel.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemTypeLabel.Text = value;
                }));

            }
        }
        public void setItemQuantityLabel(string value)
        {

            if (ItemQuantityLabel.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemQuantityLabel.Text = value;
                }));

            }
        }
        public void setItemDamagedLabel(string value)
        {

            if (ItemDamagedLabel.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemDamagedLabel.Text = value;
                }));

            }
        }
        public void setItemDurabilityLabel(string value)
        {

            if (ItemDurabilityLabel.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemDurabilityLabel.Text = value;
                }));

            }
        }
        public void setItemNameLabel(string value)
        {

            if (ItemNameLabel.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemNameLabel.Text = value;
                }));

            }
        }
        public void setItemLevelLabel(string value)
        {

            if (ItemLevelLabel.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemLevelLabel.Text = value;
                }));

            }
        }

        public void setItemInfLabel(string value)
        {

            if (ItemInfLabel.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    ItemInfLabel.Text = value;
                }));

            }
        }
        public void logMessage(string message)
        {
            if (logBox.InvokeRequired)
            {
                logBox.Invoke(new MethodInvoker(() =>
                    {
                        logBox.AppendText(message + "\n");
                        logBox.SelectionStart = logBox.Text.Length;
                        logBox.ScrollToCaret();
                    }));
            }
            else
            {
                logBox.AppendText(message + "\n");
                logBox.SelectionStart = logBox.Text.Length;
                logBox.ScrollToCaret();
            }
            
           
        }

        public class ItemId
        {
            public string name;
            public int hexId;
            public int decId;
        }
         public mainWindow()
        {
            //WINDOW START
            InitializeComponent();
            logMessage ("Hooking...");

            
            main.WorkerSupportsCancellation = false;
            main.WorkerReportsProgress = false;
            main.DoWork += new DoWorkEventHandler(main_DoWork);
            main.RunWorkerAsync();
            mainWin = this;
            logBox.Text = "";

        }

        private bool InjectHook()
         {
           
             eaxAddress = 5278857;
             int returnAddress = 5278862;
             pItemAddr = new IntPtr(caveBase.ToInt64() + 100);

             try
             {
                 MemSharp.Assembly.Inject(
                  new[] {
                    "push edi",
                    "mov edi, " + pItemAddr,
                    "mov [edi], eax",
                    "pop edi",
                    "mov cl,[eax+22]",
                    "test cl,cl",
                    "jmp " + returnAddress//,
                    //"ret"
                }, caveBase);
                 MemSharp.Assembly.Inject("jmp " + caveBase, new IntPtr(eaxAddress));
             }
             catch 
             {
                  return false;
             }

             return true;
         }       
        public class Item
        {
            public string itemName;
            public int itemBase;
            public int itemId1;
            public int itemId2;
            public int itemType;
            public int itemQuantity;
            public float itemDurability;
            public byte itemDamaged;
            public byte itemLevel;
            public byte itemInfusion;
            public Item()
            {
                itemBase = 0;
                itemId1 = 0;
                itemId2 = 0;
                itemType = 0;
                itemQuantity = 0;
                itemDamaged = 0;
                itemDurability = 0.0f;
                itemLevel = 0;
                itemInfusion = 0;
            }

            public void UnknownValues()
            {
                
                mainWin.setBase("unknown");
                mainWin.setItemId1("unknown");
                mainWin.setItemId2("unknown");
                mainWin.setItemTypeLabel("unknown");
                mainWin.setItemQuantityLabel("unknown");
                mainWin.setItemDurabilityLabel("unknown");
                mainWin.setItemDamagedLabel("unknown");
                mainWin.setItemLevelLabel("unknown");
                mainWin.setItemInfLabel("unknown");
            }
            public void UpdateLabels()
            {
                mainWin.setBase(itemBase.ToString("X"));
                mainWin.setItemId1(itemId1.ToString("X"));
                mainWin.setItemId2(itemId2.ToString("X"));
                mainWin.setItemTypeLabel(itemType.ToString("X"));
                mainWin.setItemQuantityLabel(itemQuantity.ToString());
                mainWin.setItemDurabilityLabel(itemDurability.ToString());
                int byteToInt = itemDamaged;
                mainWin.setItemDamagedLabel(byteToInt.ToString());
                byteToInt = itemLevel;
                mainWin.setItemLevelLabel(byteToInt.ToString());
                byteToInt = itemInfusion;
                mainWin.setItemInfLabel(byteToInt.ToString());
                bool itemKnown = false;
                foreach (ItemId item in allItems)
                {
                    if (item.decId == itemId1)
                    {
                        itemName = item.name;
                        mainWin.setItemNameLabel(item.name);
                        itemKnown = true;
                        break;
                    }
                }
                if (itemKnown == false)
                {
                   mainWin.setItemNameLabel("Item unknown");
                }
            }
        };
       


        private void main_DoWork(object sender, DoWorkEventArgs e)
        {
            //KEYBOARD EVENT HANDLERS
            KeyboardHook.InstallHook();
            

            //LOAD AND PARSE ITEMS.XML

            XmlDocument doc = new XmlDocument();
            doc.Load("ITEMS.xml");
            XmlElement rootEle = doc.DocumentElement;
            XmlNodeList nodes = rootEle.SelectNodes("/Item/Item");
            foreach (XmlNode node in nodes)
            {
                string name = (node["Name"].InnerText);
                string hexId = (node["HexId"].InnerText);
                string decId = (node["DecId"].InnerText);
                int hexIdInt = Convert.ToInt32(hexId, 16);
                int decIdInt = Convert.ToInt32(decId, 10);
                ItemId newItem = new ItemId();
                newItem.name = name;
                newItem.hexId = hexIdInt;
                newItem.decId = decIdInt;
                allItems.Add(newItem);
                
            }
            foreach (ItemId item in allItems)
            {
                addItemToDropDownBox(item);
            }

            //Find dark souls ii window
            int pId = 0;

            bool success = false;
            while (success == false)
            {
                try
                {
                    pId = Binarysharp.MemoryManagement.Helpers.ApplicationFinder.FromWindowTitle("DARK SOULS II").First().Id;
                }
                catch
                {
                    logMessage ("Couldn't find DarkSoulsII.exe. Are you sure it's open?\n");
                    System.Threading.Thread.Sleep(2000);
                    continue;
                }
                success = true;
            }
            success = false;
            

            MemSharp = new MemorySharp(pId);
            MemSharp.Memory.Allocate(1000);
            caveBase = MemSharp.Memory.RemoteAllocations.First().BaseAddress;
            setCaveBase(caveBase.ToString("X"));


            if (!InjectHook())
            {
                logMessage("Could not inject hook.");
                return;
            }
            logMessage("Success!");

            

            Item highlightedItem = new Item();
            while (true)
            {
                try
                {

                    pItemBaseVal = MemSharp.Read<Int32>(pItemAddr, false);
                    highlightedItem.itemBase = pItemBaseVal;
                    highlightedItem.itemId1 = MemSharp.Read<Int32>(new IntPtr(pItemBaseVal) + id1Offset, false);
                    highlightedItem.itemId2 = MemSharp.Read<Int32>(new IntPtr(pItemBaseVal) + id2Offset, false);
                    highlightedItem.itemType = MemSharp.Read<ushort>(new IntPtr(pItemBaseVal) + typeOffset, false);
                    highlightedItem.itemQuantity = MemSharp.Read<Int32>(new IntPtr(pItemBaseVal) + quantityOffset, false);
                    highlightedItem.itemDurability = MemSharp.Read<float>(new IntPtr(pItemBaseVal) + durabilityOffset, false);
                    highlightedItem.itemDamaged = MemSharp.Read<byte>(new IntPtr(pItemBaseVal) + damagedOffset, false);
                    highlightedItem.itemLevel = MemSharp.Read<byte>(new IntPtr(pItemBaseVal) + levelOffset, false);
                    highlightedItem.itemInfusion = MemSharp.Read<byte>(new IntPtr(pItemBaseVal) + infusionOffset, false);
                }
                catch
                {
                    ///TOO VERBOSE
                    //("Couldn't read highlighted item base.");
                }
                try
                {
                    highlightedItem.UpdateLabels();
                }
                catch
                {
                    logMessage("Couldn't access main form.");
                }

                //TODO: IS THIS TOO FAST?
                System.Threading.Thread.Sleep(100);
            }
            
        }
        private void mainWindow_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            try
            {
                MemSharp.Assembly.Inject(new[] { "mov cl,[eax+22]", "test cl,cl" }, new IntPtr(eaxAddress));
                MemSharp.Dispose();
            }
            catch
            {
                
            }
            e.Cancel = false;
        }

        public int nameToDecId(string itemName)
        {
            foreach(ItemId item in allItems)
            {
                if (itemName == item.name)
                {
                    return item.decId;
                }
            }
            return 0;
        }


        public void ReplaceItem(string name)
        {
            //highlightedItem.itemId1 = MemSharp.Read<Int32>(new IntPtr(pItemBaseVal) + id1Offset, false);
            int decId = nameToDecId(name);
            ReplaceItem(decId);
            
        }
        public void ReplaceItem(int decId)
        {
            try
            {
                MemSharp.Write<Int32>(new IntPtr(pItemBaseVal + id1Offset), decId, false);
                MemSharp.Write<Int32>(new IntPtr(pItemBaseVal + id2Offset), decId, false);
            }
           catch
            {
                logMessage("Couldn't write new item.");
           }
            logMessage((pItemAddr + id1Offset).ToString());
        }

        private void ItemInfLabel_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void ItemNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void logBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Replacebutton_Click(object sender, EventArgs e)
        {          
            ReplaceItem(getDropDownSelection());
            
        }

        private void ReplaceAllButton_Click(object sender, EventArgs e)
        {
            var window = MemSharp.Windows.MainWindow;
            window.Activate();
            System.Threading.Thread.Sleep(1000);
            if (!window.IsActivated)
            {
                logMessage("Couldn't bring Dark Souls II to focus.");
                return;
            }

            System.Threading.Thread.Sleep(1000);
            //Replace weapon 1
            InputManager.Keyboard.KeyPress(System.Windows.Forms.Keys.Enter, 100);
            System.Threading.Thread.Sleep(1000);
            InputManager.Keyboard.KeyPress(System.Windows.Forms.Keys.Enter, 100);
            System.Threading.Thread.Sleep(1000);
            InputManager.Keyboard.KeyPress(System.Windows.Forms.Keys.Right, 1000);
            System.Threading.Thread.Sleep(1000);
            //replace weapon 2
            InputManager.Keyboard.KeyPress(System.Windows.Forms.Keys.Enter, 100);
            System.Threading.Thread.Sleep(1000);
            InputManager.Keyboard.KeyPress(System.Windows.Forms.Keys.Enter, 100);
            System.Threading.Thread.Sleep(1000);
            InputManager.Keyboard.KeyPress(System.Windows.Forms.Keys.Right, 100);
            System.Threading.Thread.Sleep(1000);

            

        }

    }
}
