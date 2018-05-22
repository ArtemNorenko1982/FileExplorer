using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace Explorer
{
    public partial class Form1 : Form
    {
        const int limit = 5;
        ImageList fileImages;
        ImageList folderImages;
        //variable for work with system drives
        GetSystemDrives sysDrive = new GetSystemDrives();
        //variable for work with folders/subfolders
        GetFolders driveFolders;
        //variable for work with files
        GetFiles foldersFile;
        //list for work with files/folders/drives - search, copy, delete, rename
        List<string> itemsList = new List<string>();
                
        public Form1()
        {
            InitializeComponent();
        }
        //filling tree view 
        private void BeforeFormLoaded()
        {
            itemsList.Clear();
            foreach (var item in sysDrive.GetDrives)
            {
                itemsList.Add(item.Name);
                TreeNode node = this.treeView1.Nodes[0].Nodes.Add(item.Name);
                //if current drive is floppy
                if (item.DriveType.ToString() == DriveType.Removable.ToString())
                {
                    node.ImageIndex = node.SelectedImageIndex = 1;
                }
                //if current drive is fixed
                if (item.DriveType.ToString() == DriveType.Fixed.ToString())
                {
                    node.ImageIndex = node.SelectedImageIndex = 2;
                }
                //if current drive is cd
                if (item.DriveType.ToString() == DriveType.CDRom.ToString())
                {
                    node.ImageIndex = node.SelectedImageIndex = 3;
                }
                //if current drive is ready - filling it by their folders
                if (item.IsReady)
                {
                    driveFolders = new GetFolders(item.ToString());
                    foreach (var folders in driveFolders.GetDirectories)
                    {
                        TreeNode nodes = this.treeView1.Nodes[0].Nodes[node.Index].Nodes.Add(folders.Name);
                        nodes.ImageIndex = nodes.SelectedImageIndex = 4;
                    }
                }
            }
        }
        //when form is load we are filling it their disks and folders
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                BeforeFormLoaded();
                this.ExplorerBox.BackColor = System.Drawing.Color.AntiqueWhite;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //filling items into Explorer box
        private void FillingItems(string newPath="")
        {
            fileImages = new ImageList();
            folderImages = new ImageList();
            string newFoldersPath = "";
            try
            {
                itemsList.Clear();
                this.ExplorerBox.Clear();
                //get main node in the tree view
                int par = this.treeView1.Nodes[0].FullPath.Length + 1;
                //get full path of current item
                string treePath = this.treeView1.SelectedNode.FullPath.ToString();

                if (newPath=="")
                {
                    //generate new path from tree view for select folders and subfolders into explorer bar
                    newFoldersPath = treePath.Substring(par);
                }
                else
                {
                    //use newPath for work with items in current drive/folder/subfolder/...
                    newFoldersPath = newPath;
                }

                driveFolders = new GetFolders(newFoldersPath);
                foreach (var item in driveFolders.GetDirectories)
                {
                    itemsList.Add(item.FullName);
                    this.ExplorerBox.Items.Add(item.Name, this.ExplorerImageList.Images.Count-1);
                }

                foldersFile = new GetFiles(newFoldersPath);
                foreach (var filesItem in foldersFile.GetAllFiles)
                {
                    itemsList.Add(filesItem.FullName);
                    fileImages.Images.Add(System.Drawing.Icon.ExtractAssociatedIcon(filesItem.FullName));
                    this.ExplorerBox.Items.Add(filesItem.Name, fileImages.Images.Count - 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //filling all drivers into Explorer box
        private void FillingDriversIntoExplorerBox()
        {
            this.ExplorerBox.Clear();
            itemsList.Clear();
            foreach (var item in sysDrive.GetDrives)
            {
                itemsList.Add(item.Name);
                //if current drive is floppy
                if (item.DriveType.ToString()== DriveType.Removable.ToString())
                {
                    this.ExplorerBox.Items.Add(item.Name, this.ExplorerImageList.Images.Count-4);
                }
                //if current drive is fix
                if (item.DriveType.ToString() == DriveType.Fixed.ToString())
                {
                    this.ExplorerBox.Items.Add(item.Name, this.ExplorerImageList.Images.Count - 3);
                }
                //if current drive is cd
                if (item.DriveType.ToString() == DriveType.CDRom.ToString())
                {
                    this.ExplorerBox.Items.Add(item.Name, this.ExplorerImageList.Images.Count - 2);
                }
            }
        }
        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.treeView1.SelectedNode.Level == 0)
            {
                FillingDriversIntoExplorerBox();
            }
            else
            {
                FillingItems();
            }
        }
        private void краткийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in this.ExplorerBox.Items)
            {
                
            }
        }
        //events after tree view items selected
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeView1.SelectedNode.Level == 0)
            {
                FillingDriversIntoExplorerBox();
            }
            else
            {
                FillingItems();
            }
        }
        //menu items About :))
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A. NORENKO");
        }

        private void ExplorerBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(this.ExplorerBox.Items.);
        }

        private void ExplorerBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                string tex = itemsList[ExplorerBox.FocusedItem.Index];

                if (new FileInfo(tex).Exists)
                {
                    Process.Start(tex);
                }
                else
                {
                    FillingItems(tex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
