using System;
using System.Management; //Install-Package System.Management -Version 5.0.0

namespace USBDriveDeteaction
{
    class Program
    {
        static void Main(string[] args)
        {
            AddInsetUSBHandler();
            AddRemoveUSBHandler();
            for (; ; );
        }

        static ManagementEventWatcher w = null;

        public static void AddRemoveUSBHandler()
        {
            WqlEventQuery q;
            ManagementScope scope = new ManagementScope("root\\CIMV2");
            scope.Options.EnablePrivileges = true;
            try
            {
                q = new WqlEventQuery();
                q.EventClassName = "__InstanceDeletionEvent";
                q.WithinInterval = new TimeSpan(0, 0, 3);
                q.Condition = @"TargetInstance ISA 'Win32_USBHub'";
                w = new ManagementEventWatcher(scope, q);
                w.EventArrived += new EventArrivedEventHandler(USBRemoved);
                w.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (w != null)
                    w.Stop();
            }

        }

        static void AddInsetUSBHandler()
        {
            WqlEventQuery q;
            ManagementScope scope = new ManagementScope("root\\CIMV2");
            scope.Options.EnablePrivileges = true;

            try
            {
                q = new WqlEventQuery();
                q.EventClassName = "__InstanceCreationEvent";
                q.WithinInterval = new TimeSpan(0, 0, 3);
                q.Condition = @"TargetInstance ISA 'Win32_USBHub'";
                w = new ManagementEventWatcher(scope, q);
                w.EventArrived += new EventArrivedEventHandler(USBAdded);
                w.Start();

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                if (w != null)

                    w.Stop();
            }

        }



        public static void USBAdded(object sender, EventArgs e)
        {
            Console.WriteLine("A USB device inserted");
        }



        public static void USBRemoved(object sender, EventArgs e)
        {
            Console.WriteLine("A USB device removed");
        }
    }
}
