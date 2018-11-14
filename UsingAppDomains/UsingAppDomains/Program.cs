using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace UsingAppDomains
{
    static class Program
    {
        static AppDomain Drawer;
        static AppDomain TextWindow;
        static Assembly DrawerAsm;
        static Assembly TextWindowAsm;
        static Form DrawerWindow;
        static Form TextWindowWnd;

        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomain)]
        static void Main()
        {
            Application.EnableVisualStyles();

            Drawer = AppDomain.CreateDomain("Drawer");
            TextWindow = AppDomain.CreateDomain("TextWindow");

            DrawerAsm = Drawer.Load(AssemblyName.GetAssemblyName("TextDrawer.exe"));
            TextWindowAsm = Drawer.Load(AssemblyName.GetAssemblyName("TextWindow.exe"));

            DrawerWindow = Activator.CreateInstance(DrawerAsm.GetType("TextDrawer.Form1")) as Form;

            TextWindowWnd = Activator.CreateInstance(TextWindowAsm.GetType("TextWindow.Form1"), 
                new object[] { DrawerAsm.GetModule("TextDrawer.exe"), DrawerWindow }) as Form;

            (new Thread(new ThreadStart(RunVisualizer))).Start();

            (new Thread(new ThreadStart(RunDrawer))).Start();

            Drawer.DomainUnload += Drawer_DomainUnload;
        }

        private static void Drawer_DomainUnload(object sender, EventArgs e)
        {
            MessageBox.Show("Domain with name " + 
                (sender as AppDomain).FriendlyName + " has been succesfully unloaded!");
        }

        static void RunDrawer()
        {
            DrawerWindow.ShowDialog();
            AppDomain.Unload(Drawer);
        }

        static void RunVisualizer()
        {
            TextWindowWnd.ShowDialog();
            Application.Exit();
        }

    }
}
