using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace zad2
{
    public interface ISubscriber<T>
    {
        void Handle(T Notification);
    }
    
    public interface IEventAggregator
    {
        void AddSubscriber<T>(ISubscriber<T> Subscriber);
        void RemoveSubscriber<T>(ISubscriber<T> Subscriber);
        void Publish<T>(T Event);
    }

    public class EventAggregator : IEventAggregator
    {
        Dictionary<Type,List<object>> _subscribers = new Dictionary<Type, List<object>>();

        #region IEventAggregatorMembers
        public void AddSubscriber<T>(ISubscriber<T> Subscriber)
        {
            if(!_subscribers.ContainsKey(typeof(T)))
                _subscribers.Add(typeof(T), new List<object>());

            _subscribers[typeof(T)].Add(Subscriber);
        }
        public void RemoveSubscriber<T>(ISubscriber<T> Subscriber)
        {
            if(_subscribers.ContainsKey(typeof(T)))
                _subscribers[typeof(T)].Remove(Subscriber);
        }

        public void Publish<T>(T Event)
        {
            if(_subscribers.ContainsKey(typeof(T)))
                foreach(ISubscriber<T>subscriber in _subscribers[typeof(T)].OfType<ISubscriber<T>>())
                    subscriber.Handle(Event);
        }
        
        #endregion
    }

    public class Show_user_tree
    {
    
    }
    public class Show_user_list
    {

    }
    public class Show_user_info
    {
        public User new_user;
        public Show_user_info(User new_u)
        {
            this.new_user = new_u;
        }
    }
    public class Change_user_list
    {
        public List<User> new_user_list;

        public Change_user_list(List<User> new_user_l)
        {
            this.new_user_list = new_user_l;
        }
    }
    public class Show_add_user
    {
        public List<User> curr_users;
        public Show_add_user(List<User> users)
        {
            this.curr_users = users;
        }
    }
    public class Show_change_user
    {
        public User curr_user;
        public Show_change_user(User user)
        {
            this.curr_user = user;
        }
    }

    public class User_list_view : ISubscriber<Show_user_list>, ISubscriber<Change_user_list>
    {
        public EventAggregator ea;
        public SplitContainer main_split_continer;
        public ListView user_list;
        public Button button_dodaj;
        public SplitContainer splitContainer1; 
        public List<User> users;

        public User_list_view(EventAggregator eventA, SplitContainer main_sc, List<User> us)
        {
            ea = eventA;
            main_split_continer = main_sc;
            user_list = new ListView();
            button_dodaj = new Button();   
            splitContainer1 = new SplitContainer();
            users = us;
            // Set properties of ListView control.
            user_list.Dock = DockStyle.Fill;  
            user_list.TabIndex = 0;
            user_list.View = View.Details;
            user_list.GridLines = true;

            user_list.Columns.Add("Nazwisko", 120, HorizontalAlignment.Left);
            user_list.Columns.Add("Imię", 120, HorizontalAlignment.Left);
            user_list.Columns.Add("Data urodzenia", 120, HorizontalAlignment.Left);
            user_list.Columns.Add("Adres", 200, HorizontalAlignment.Center);

            foreach(User user in users)
                user_list.Items.Add(new ListViewItem(user.ToStringArray()));
            // set button
            button_dodaj.Height = 40;
            button_dodaj.Width = 300;
            button_dodaj.Location = new Point(150, 50);
            button_dodaj.Text = "Dodaj";
            button_dodaj.Name = "Dodaj_button";
            button_dodaj.Click += new EventHandler(this.Show_add_user_modal);
            // Set properties of first SplitContainer control.  
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;  
            splitContainer1.TabIndex = 1;  
            splitContainer1.SplitterWidth = 4;  
            splitContainer1.SplitterDistance = 150;  
            splitContainer1.Orientation = Orientation.Horizontal;  
            splitContainer1.Panel1.Controls.Add(this.user_list);
            splitContainer1.Panel2.Controls.Add(this.button_dodaj);
        }
        public void Handle(Show_user_list notification)
        {
            main_split_continer.Panel2.Controls.Clear();
            main_split_continer.Panel2.Controls.Add(this.splitContainer1);
        }

        public void Handle(Change_user_list notification)
        {
            users = notification.new_user_list;
            user_list.Items.Clear();
            foreach(User user in users)
                user_list.Items.Add(new ListViewItem(user.ToStringArray()));
        }

        private void Show_add_user_modal(object sender, EventArgs e)
        {
            ea.Publish<Show_add_user>(new Show_add_user(users));
        }
    }

    public class User_info_view : ISubscriber<Show_user_info>
    {
        public EventAggregator ea;
        public SplitContainer main_split_continer;
        public User user;
        public SplitContainer splitContainer1;
        public Button button_edit;
        public TextBox text_firstname;
        public Label label_firstname;
        public TextBox text_lastname;
        public Label label_lastname;
        public TextBox text_birthdate;
        public Label label_birthdate;
        public TextBox text_address;
        public Label label_address;
        public User_info_view(EventAggregator eventA, SplitContainer main_sc)
        {
            ea = eventA;
            main_split_continer = main_sc;
            splitContainer1 = new SplitContainer();
            button_edit = new Button();
            text_firstname = new TextBox();
            label_firstname = new Label();
            text_lastname = new TextBox();
            label_lastname = new Label();
            text_birthdate = new TextBox();
            label_birthdate = new Label();
            text_address = new TextBox();
            label_address = new Label();

            // Set properties of first SplitContainer control.  
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;  
            splitContainer1.TabIndex = 1;  
            splitContainer1.SplitterWidth = 4;  
            splitContainer1.SplitterDistance = 150;  
            splitContainer1.Orientation = Orientation.Horizontal;

            text_firstname.Location = new Point(120, 20);
            text_firstname.Size = new Size(300, 15);
            text_firstname.ReadOnly = true;
            splitContainer1.Panel1.Controls.Add(text_firstname);

            label_firstname.Text = "Imie: ";
            label_firstname.Location = new Point(20, 20);
            splitContainer1.Panel1.Controls.Add(label_firstname);

            text_lastname.Location = new Point(120, 60);
            text_lastname.Size = new Size(300, 15);
            text_lastname.ReadOnly = true;
            splitContainer1.Panel1.Controls.Add(text_lastname);

            label_lastname.Text = "Nazwisko: ";
            label_lastname.Location = new Point(20, 60);
            splitContainer1.Panel1.Controls.Add(label_lastname);

            text_birthdate.Location = new Point(120, 100);
            text_birthdate.Size = new Size(300, 15);
            text_birthdate.ReadOnly = true;
            splitContainer1.Panel1.Controls.Add(text_birthdate);

            label_birthdate.Text = "Data urodzenia: ";
            label_birthdate.Location = new Point(20, 100);
            splitContainer1.Panel1.Controls.Add(label_birthdate);

            text_address.Location = new Point(120, 140);
            text_address.Size = new Size(300, 15);
            text_address.ReadOnly = true;
            splitContainer1.Panel1.Controls.Add(text_address);

            label_address.Text = "Adres: ";
            label_address.Location = new Point(20, 140);
            splitContainer1.Panel1.Controls.Add(label_address);

            // set button
            button_edit.Height = 40;
            button_edit.Width = 300;
            button_edit.Location = new Point(150, 50);
            button_edit.Text = "Zmien";
            button_edit.Name = "zmien_button";
            button_edit.Click += new EventHandler(this.Show_change_user_modal);
            splitContainer1.Panel2.Controls.Add(button_edit);
        }
        private void Show_change_user_modal(object sender, EventArgs e)
        {
            ea.Publish<Show_change_user>(new Show_change_user(user));
        }

        public void Handle(Show_user_info notification)
        {
            user = notification.new_user;
            text_firstname.Text = user.firstname;
            text_lastname.Text = user.lastname;
            text_birthdate.Text = user.birth_date;
            text_address.Text = user.address;

            main_split_continer.Panel2.Controls.Clear();
            main_split_continer.Panel2.Controls.Add(this.splitContainer1);
        }
    }

    public class User_tree_view : ISubscriber<Show_user_tree>
    {
        public EventAggregator ea;
        public SplitContainer main_split_continer;
        public TreeView user_tree;
        public List<User> students;
        public List<User> teachers;

        public User_tree_view(EventAggregator eventA, SplitContainer main_sc, List<User> st, List<User> te)
        {
            ea = eventA;
            main_split_continer = main_sc;
            students = st;
            teachers = te;
            user_tree = new TreeView();
            // Set properties of TreeView control.  
            user_tree.Dock = DockStyle.Fill;  
            user_tree.TabIndex = 0;  
            user_tree.AfterSelect += new TreeViewEventHandler(this.TreeView_AfterSelect);
            user_tree.Nodes.Add("Studenci");
            user_tree.Nodes.Add("Wykładowcy");
            this.Add_users();
        }

        private void Add_users()
        {
            foreach(User student in students)
                user_tree.Nodes[0].Nodes.Add(student.firstname + " " + student.lastname);
            foreach(User teacher in teachers)
                user_tree.Nodes[1].Nodes.Add(teacher.firstname + " " + teacher.lastname);
        }
        public void Handle(Show_user_tree notification)
        {
            main_split_continer.Panel1.Controls.Clear();
            user_tree.Nodes[0].Nodes.Clear();
            user_tree.Nodes[1].Nodes.Clear();
            this.Add_users();
            main_split_continer.Panel1.Controls.Add(this.user_tree);
            user_tree.SelectedNode = null;
        }
        private void TreeView_AfterSelect(Object sender, TreeViewEventArgs e)
        {
            this.Update_views();
        }
        private void Update_views()
        {
            int group_index = user_tree.Nodes.IndexOf(user_tree.SelectedNode);
            if (group_index == 0) {    // studenci
                ea.Publish<Change_user_list>(new Change_user_list(students));
                ea.Publish<Show_user_list>(new Show_user_list());
            } else if (group_index == 1) { // wykladowcy
                ea.Publish<Change_user_list>(new Change_user_list(teachers));
                ea.Publish<Show_user_list>(new Show_user_list());
            } else {
                if (user_tree.Nodes[0].Nodes.Contains(user_tree.SelectedNode)) { // student
                    int index = user_tree.Nodes[0].Nodes.IndexOf(user_tree.SelectedNode);
                    ea.Publish<Show_user_info>(new Show_user_info(students[index]));
                } else if (user_tree.Nodes[1].Nodes.Contains(user_tree.SelectedNode)) { // wykladowca
                    int index = user_tree.Nodes[1].Nodes.IndexOf(user_tree.SelectedNode);
                    ea.Publish<Show_user_info>(new Show_user_info(teachers[index]));
                }
            }
        }
    }

    public class Add_user_modal : ISubscriber<Show_add_user>
    {
        public EventAggregator ea;
        public List<User> users;
        public Form form;
        public Button button_ok;
        public Button button_cancel;
        public SplitContainer splitContainer1;
        public TextBox text_firstname;
        public Label label_firstname;
        public TextBox text_lastname;
        public Label label_lastname;
        public TextBox text_birthdate;
        public Label label_birthdate;
        public TextBox text_address;
        public Label label_address;

        public Add_user_modal(EventAggregator eventA)
        {
            ea = eventA;
            form = new Form();
            splitContainer1 = new SplitContainer();
            button_ok = new Button();
            button_cancel = new Button();
            text_firstname = new TextBox();
            label_firstname = new Label();
            text_lastname = new TextBox();
            label_lastname = new Label();
            text_birthdate = new TextBox();
            label_birthdate = new Label();
            text_address = new TextBox();
            label_address = new Label();
            
            // Set properties of first SplitContainer control.  
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;  
            splitContainer1.TabIndex = 1;  
            splitContainer1.SplitterWidth = 4;  
            splitContainer1.SplitterDistance = 150;  
            splitContainer1.Orientation = Orientation.Horizontal;

            text_firstname.Location = new Point(120, 20);
            text_firstname.Size = new Size(300, 15);
            splitContainer1.Panel1.Controls.Add(text_firstname);

            label_firstname.Text = "Imie: ";
            label_firstname.Location = new Point(20, 20);
            splitContainer1.Panel1.Controls.Add(label_firstname);

            text_lastname.Location = new Point(120, 60);
            text_lastname.Size = new Size(300, 15);
            splitContainer1.Panel1.Controls.Add(text_lastname);

            label_lastname.Text = "Nazwisko: ";
            label_lastname.Location = new Point(20, 60);
            splitContainer1.Panel1.Controls.Add(label_lastname);

            text_birthdate.Location = new Point(120, 100);
            text_birthdate.Size = new Size(300, 15);
            splitContainer1.Panel1.Controls.Add(text_birthdate);

            label_birthdate.Text = "Data urodzenia: ";
            label_birthdate.Location = new Point(20, 100);
            splitContainer1.Panel1.Controls.Add(label_birthdate);

            text_address.Location = new Point(120, 140);
            text_address.Size = new Size(300, 15);
            splitContainer1.Panel1.Controls.Add(text_address);

            label_address.Text = "Adres: ";
            label_address.Location = new Point(20, 140);
            splitContainer1.Panel1.Controls.Add(label_address);

            // set button
            button_ok.Height = 40;
            button_ok.Width = 150;
            button_ok.Location = new Point(50, 50);
            button_ok.Text = "Dodaj";
            button_ok.Name = "ok_button";
            button_ok.Click += new EventHandler(this.Add_user_click);
            splitContainer1.Panel2.Controls.Add(button_ok);

            button_cancel.Height = 40;
            button_cancel.Width = 150;
            button_cancel.Location = new Point(300, 50);
            button_cancel.Text = "Anuluj";
            button_cancel.Name = "cancel_button";
            splitContainer1.Panel2.Controls.Add(button_cancel);

            // set form
            form.Text = "Dodaj użytkownika";
            form.Height = 400;
            form.Width = 500;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.AcceptButton = button_ok;
            form.CancelButton = button_cancel;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Controls.Add(splitContainer1);
        }

        public void Handle(Show_add_user Notification)
        {
            users = Notification.curr_users;
            form.ShowDialog();
        }
        private void Add_user_click(object sender, EventArgs e)
        {
            User new_user = new User(text_lastname.Text, text_firstname.Text, text_birthdate.Text, text_address.Text);
            users.Add(new_user);
            ea.Publish<Change_user_list>(new Change_user_list(users));
            ea.Publish<Show_user_list>(new Show_user_list());
            ea.Publish<Show_user_tree>(new Show_user_tree());
            form.Close();
        }
    }

    public class Change_user_modal : ISubscriber<Show_change_user>
    {
        public EventAggregator ea;
        public User user;
        public Form form;
        public Button button_ok;
        public Button button_cancel;
        public SplitContainer splitContainer1;
        public TextBox text_firstname;
        public Label label_firstname;
        public TextBox text_lastname;
        public Label label_lastname;
        public TextBox text_birthdate;
        public Label label_birthdate;
        public TextBox text_address;
        public Label label_address;

        public Change_user_modal(EventAggregator eventA)
        {
            ea = eventA;
            form = new Form();
            splitContainer1 = new SplitContainer();
            button_ok = new Button();
            button_cancel = new Button();
            text_firstname = new TextBox();
            label_firstname = new Label();
            text_lastname = new TextBox();
            label_lastname = new Label();
            text_birthdate = new TextBox();
            label_birthdate = new Label();
            text_address = new TextBox();
            label_address = new Label();
            
            // Set properties of first SplitContainer control.  
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;  
            splitContainer1.TabIndex = 1;  
            splitContainer1.SplitterWidth = 4;  
            splitContainer1.SplitterDistance = 150;  
            splitContainer1.Orientation = Orientation.Horizontal;

            text_firstname.Location = new Point(120, 20);
            text_firstname.Size = new Size(300, 15);
            splitContainer1.Panel1.Controls.Add(text_firstname);

            label_firstname.Text = "Imie: ";
            label_firstname.Location = new Point(20, 20);
            splitContainer1.Panel1.Controls.Add(label_firstname);

            text_lastname.Location = new Point(120, 60);
            text_lastname.Size = new Size(300, 15);
            splitContainer1.Panel1.Controls.Add(text_lastname);

            label_lastname.Text = "Nazwisko: ";
            label_lastname.Location = new Point(20, 60);
            splitContainer1.Panel1.Controls.Add(label_lastname);

            text_birthdate.Location = new Point(120, 100);
            text_birthdate.Size = new Size(300, 15);
            splitContainer1.Panel1.Controls.Add(text_birthdate);

            label_birthdate.Text = "Data urodzenia: ";
            label_birthdate.Location = new Point(20, 100);
            splitContainer1.Panel1.Controls.Add(label_birthdate);

            text_address.Location = new Point(120, 140);
            text_address.Size = new Size(300, 15);
            splitContainer1.Panel1.Controls.Add(text_address);

            label_address.Text = "Adres: ";
            label_address.Location = new Point(20, 140);
            splitContainer1.Panel1.Controls.Add(label_address);

            // set button
            button_ok.Height = 40;
            button_ok.Width = 150;
            button_ok.Location = new Point(50, 50);
            button_ok.Text = "Zmien";
            button_ok.Name = "ok_button";
            button_ok.Click += new EventHandler(this.Change_user_click);
            splitContainer1.Panel2.Controls.Add(button_ok);

            button_cancel.Height = 40;
            button_cancel.Width = 150;
            button_cancel.Location = new Point(300, 50);
            button_cancel.Text = "Anuluj";
            button_cancel.Name = "cancel_button";
            splitContainer1.Panel2.Controls.Add(button_cancel);

            // set form
            form.Text = "Zmien użytkownika";
            form.Height = 400;
            form.Width = 500;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.AcceptButton = button_ok;
            form.CancelButton = button_cancel;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Controls.Add(splitContainer1);
        }

        public void Handle(Show_change_user Notification)
        {
            user = Notification.curr_user;
            text_firstname.Text = user.firstname;
            text_lastname.Text = user.lastname;
            text_birthdate.Text = user.birth_date;
            text_address.Text = user.address;
            form.ShowDialog();
        }
        private void Change_user_click(object sender, EventArgs e)
        {
            user.firstname = text_firstname.Text;
            user.lastname = text_lastname.Text;
            user.birth_date = text_birthdate.Text;
            user.address = text_address.Text;

            ea.Publish<Show_user_info>(new Show_user_info(user));
            ea.Publish<Show_user_tree>(new Show_user_tree());
            form.Close();
        }
    }

    public class User
    {
        public string lastname;
        public string firstname;
        public string birth_date;
        public string address;

        public User(string Lastname, string Firstname, string Bitrth_date, string Address)
        {
            this.lastname = Lastname;
            this.firstname = Firstname;
            this.birth_date = Bitrth_date;
            this.address = Address;
        }

        public string[] ToStringArray()
        {
            return new string[] { lastname, firstname, birth_date, address};
        }
    }
    public class CMainForm : Form1
    {
        public EventAggregator ea;
        public User_list_view user_list_view; 
        public User_tree_view user_tree_view;
        public User_info_view user_info_view;
        public Add_user_modal add_user_modal;
        public Change_user_modal change_user_modal;
        private SplitContainer splitContainer2;  
        public CMainForm()
        {
            // Create an instance of each control being used.  
            splitContainer2 = new SplitContainer();
            // Set properties of second SplitContainer control.  
            splitContainer2.Dock = DockStyle.Fill;  
            splitContainer2.TabIndex = 4;  
            splitContainer2.SplitterWidth = 2;  
            splitContainer2.SplitterDistance = 40;  

            // Insert code here to hook up event methods. 

            List<User> students = new List<User>();
            students.Add(new User("Kochanowski", "Jan", "1999-10-10", "ul. Kwiatowa 1/10, Warszawa"));
            students.Add(new User("Mieszek", "Bob", "2000-10-10", "ul. Lata 10/2, Wrocław"));
            List<User> teachers = new List<User>();
            teachers.Add(new User("Silor", "Krzysztof", "1950-10-10", "ul. Kwiatowa 1/1, Wrocław"));
            teachers.Add(new User("Tomaszewski", "Jerzy", "1970-10-10", "ul. Miodowa 2/2, Wrocław"));

            ea = new EventAggregator();

            user_list_view = new User_list_view(ea, splitContainer2, teachers);
            ea.AddSubscriber<Show_user_list>(user_list_view);
            ea.AddSubscriber<Change_user_list>(user_list_view);

            user_tree_view = new User_tree_view(ea, splitContainer2, students, teachers);
            ea.AddSubscriber<Show_user_tree>(user_tree_view);

            ea.Publish<Show_user_list>(new Show_user_list());
            ea.Publish<Show_user_tree>(new Show_user_tree());

            ea.Publish<Change_user_list>(new Change_user_list(students));

            user_info_view = new User_info_view(ea, splitContainer2);
            ea.AddSubscriber<Show_user_info>(user_info_view);

            add_user_modal = new Add_user_modal(ea);
            ea.AddSubscriber<Show_add_user>(add_user_modal);

            change_user_modal = new Change_user_modal(ea);
            ea.AddSubscriber<Show_change_user>(change_user_modal);

            // Add the main SplitContainer control to the form.  
            this.Controls.Add(this.splitContainer2);
        }
        
    }
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new CMainForm());
        }
    }
}
