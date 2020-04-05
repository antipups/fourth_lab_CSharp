using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace fourth_lab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            treeView1.Visible = false;
            // заполняем дерево дисками
            
        }
		// событие перед раскрытием узла
        void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            string[] dirs;
            try
            {
                if (Directory.Exists(e.Node.FullPath))
                {
                    dirs = Directory.GetDirectories(e.Node.FullPath);
                    if (dirs.Length != 0)
                    {
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            TreeNode dirNode = new TreeNode(new DirectoryInfo(dirs[i]).Name);
                            FillTreeNode(dirNode, dirs[i]);
                            e.Node.Nodes.Add(dirNode);
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
		// событие перед выделением узла
        void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            string[] dirs;
            try
            {
                if(Directory.Exists(e.Node.FullPath))
                {
                    dirs = Directory.GetDirectories(e.Node.FullPath);
                    if (dirs.Length!= 0)
                    {
                        for(int i=0; i<dirs.Length;i++)
                        {
                            TreeNode dirNode = new TreeNode(new DirectoryInfo(dirs[i]).Name);
                            FillTreeNode(dirNode, dirs[i]);
                            e.Node.Nodes.Add(dirNode);
                        }
                    }
                } 
            }
            catch (Exception ex) { }
        }
		
		// получаем все диски на компьютере
        private void FillDriveNodes(String path)
        {
            try
            {
                foreach(string driveNode in Directory.GetDirectories(path))
                {
                    TreeNode treeNode = new TreeNode {Text = driveNode};
                    // treeNode.Text = driveNode.Remove(0, driveNode.LastIndexOf("\\") + 1);
                    FillTreeNode(treeNode, driveNode);
                    treeView1.Nodes.Add(treeNode);
                }
            }
            catch (Exception ex) { }
        }
		// получаем дочерние узлы для определенного узла
        private void FillTreeNode(TreeNode driveNode, string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    TreeNode dirNode = new TreeNode();
                    dirNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
                    driveNode.Nodes.Add(dirNode);
                }
            }
            catch (Exception ex) { }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
            // this.textBox1.KeyDown += new KeyEventHandler(this.textBox1_KeyDown);
        {
            if (e.KeyCode == Keys.Enter)
            {
                String path =  textBox1.Text;
                if (!Directory.Exists(path))
                {
                    textBox1.Text = "ВВЕДИТЕ КОРРЕКТНЫЙ ПУТЬ";
                }
                else
                {
                    treeView1.Nodes.Clear();
                    treeView1.Visible = true;
                    treeView1.BeforeSelect += treeView1_BeforeSelect;
                    treeView1.BeforeExpand += treeView1_BeforeExpand;
                    textBox1.Clear();
                    FillDriveNodes(path);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void textBox2_KeyDown(object sender, EventArgs e)
            // this.textBox2.KeyDown += new KeyEventHandler(this.textBox2_KeyDown);
        {
            // throw new System.NotImplementedException();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode CurrentNode = treeView1.SelectedNode;
            String path_to_move;
            try
            {
                path_to_move = get_path(CurrentNode).Substring(1) + "\\" + CurrentNode.Text;
            }
            catch (Exception exception)    // если нет родителя (if -- не для нас!)
            {
                path_to_move = treeView1.SelectedNode.Text;
            }

            textBox1.Text = path_to_move;
            
            // далее махинации с считыванием файла) (строка ниже - путь к rtf)
            String path = "C:\\Users\\kurku\\Documents\\C#Projects\\fourth_lab_C#\\fourth_lab\\fourth_lab\\rtfs";
            int amount_of_file = 0;
            textBox3.Text = "";
            foreach (String name_of_rtf in Directory.GetFiles(path))
            {
                RichTextBox RTB = new RichTextBox();
                RTB.Rtf = File.ReadAllText(name_of_rtf);
                textBox3.Text += "Открыт файл -- " + name_of_rtf + "\n";
                if (RTB.Text.IndexOf(textBox2.Text, StringComparison.Ordinal) >= 0)
                {
                    textBox3.Text += "В файле найдено совпадение -- " + textBox2.Text +  "\n";
                    File.Move(name_of_rtf, path_to_move + "\\" + name_of_rtf.Substring(name_of_rtf.LastIndexOf("\\", StringComparison.Ordinal) + 1));
                    amount_of_file += 1;
                }
            }

            textBox2.Text = "Перемещено - " + amount_of_file.ToString() + " файла.";
        }

        private string get_path(TreeNode CurrentNode)
        {
            if (CurrentNode.Parent == null)
            {
                return "";
            }
            else
            {
                String parent = get_path(CurrentNode.Parent);
                return parent + "\\" + CurrentNode.Parent.Text;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
            // throw new System.NotImplementedException();
        }
    }
}