using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Management;
using System.Windows.Forms;

namespace Project_1
{
    class WMI
    {   
        // lay danh sach card mang
        public static ArrayList laydanhsachcar()
        {
            ArrayList tencard = new ArrayList();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach( ManagementObject mo in moc )
            {
                if ((bool)mo["ipEnabled"])
                {
                    tencard.Add(mo["Caption"]);
                }
            }
            return tencard;
        }

        // lay thong tin NIC
        public static void GetNicInfo(string tencard, out string[] ipaddress, out string[] subnet, out string[] gateway, out string[] dns)
        {
            ipaddress = subnet = gateway = dns = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["ipEnabled"])
                {
                    if (mo["Caption"].Equals(tencard))
                    {
                        ipaddress = (string[])mo["IPAddress"];
                        subnet = (string[])mo["IPSubnet"];
                        gateway = (string[])mo["DefaultIPGateway"];
                        dns = (string[])mo["DNSServerSearchOrder"];
                        break;
                    }
                }
            }
        }

        // set thong tin ip , GW , DNS 
        public static void SetIP(string nicName, string IpAddresses, string SubnetMask, string Gateway, string DnsSearchOrder)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    if (mo["Caption"].Equals(nicName))
                    {
                        ManagementBaseObject newIP = mo.GetMethodParameters("EnableStatic");
                        ManagementBaseObject newGate = mo.GetMethodParameters("SetGateways");
                        ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");

                        newIP["IPAddress"] = new string[] { IpAddresses };
                        newIP["SubnetMask"] = new string[] { SubnetMask };
                        newGate["DefaultIPGateway"] = new string[] { Gateway };
                        newGate["GatewayCostMetric"] = new int[] { 1 };
                        newDNS["DNSServerSearchOrder"] = DnsSearchOrder.Split(',');

                        ManagementBaseObject setIP = mo.InvokeMethod("EnableStatic", newIP, null);
                        ManagementBaseObject setGateways = mo.InvokeMethod("SetGateways", newGate, null);
                        ManagementBaseObject setDNS = mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                        MessageBox.Show("thay doi thanh con", "thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                }
            }


        }

        //thiet lap 
        public static void SetDHCP(string nicName)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach(ManagementObject mo in moc )
            {
                if ((bool)mo["IPEnabled"])
                {
                    if (mo["Caption"].Equals(nicName))
                    {
                        ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");
                        newDNS["DNSServerSearchOrder"] = null;
                        ManagementBaseObject enableDHCP = mo.InvokeMethod("EnableDHCP", null, null);
                        ManagementBaseObject setDNS = mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                        MessageBox.Show("thay doi thanh cong ", "thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
